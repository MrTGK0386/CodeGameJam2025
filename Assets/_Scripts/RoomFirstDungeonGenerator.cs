using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ItemPlacementRule
{
    public string itemName;
    public GameObject prefab;
    [Range(0, 1)]
    [SerializeField]
    public float spawnRate; // 0-1
    public int maxPerRoom;
    public float minDistanceFromWalls;
    public float minDistanceFromOtherItems;
}

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField] 
    private List<ItemPlacementRule> itemRules = new List<ItemPlacementRule>();

    private HashSet<Vector2Int> corridors;
    private List<BoundsInt> roomsList;
    private Dictionary<Vector2Int, GameObject> placedItems = new Dictionary<Vector2Int, GameObject>();
    protected HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

    protected override void RunProceduralGeneration()
    {
        ClearItems();
        CreateRooms();
        PlaceItems();
    }

    private void ClearItems()
    {
        foreach (var item in placedItems.Values)
        {
            if (item != null)
                Destroy(item);
        }
        placedItems.Clear();
        
        GameObject propsContainer = GameObject.Find("Grid/Props");
        if (propsContainer != null)
        {
            foreach (Transform child in propsContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    protected virtual void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPosition, 
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), 
            minRoomWidth, 
            minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        this.roomsList = roomsList;
        this.corridors = corridors;
        this.floor = floor;

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private void PlaceItems()
    {
        foreach (var room in roomsList)
        {
            foreach (var rule in itemRules)
            {
                int itemsPlaced = 0;
                int attempts = 0;
                int maxAttempts = 50;

                while (itemsPlaced < rule.maxPerRoom && attempts < maxAttempts)
                {
                    attempts++;
                    if (Random.value > rule.spawnRate) continue;

                    Vector2Int position = GetRandomPositionInRoom(room);
                    if (IsValidItemPosition(position, rule))
                    {
                        PlaceItem(position, rule);
                        itemsPlaced++;
                    }
                }
            }
        }
    }

    private Vector2Int GetRandomPositionInRoom(BoundsInt room)
    {
        int x = Random.Range(room.xMin + offset, room.xMax - offset);
        int y = Random.Range(room.yMin + offset, room.yMax - offset);
        return new Vector2Int(x, y);
    }

    private bool IsValidItemPosition(Vector2Int position, ItemPlacementRule rule)
    {
        if (!floor.Contains(position)) return false;

        foreach (var wallPos in GetNearbyWalls(position, Mathf.CeilToInt(rule.minDistanceFromWalls)))
        {
            if (Vector2.Distance(position, wallPos) < rule.minDistanceFromWalls)
                return false;
        }

        foreach (var item in placedItems)
        {
            if (Vector2.Distance(position, item.Key) < rule.minDistanceFromOtherItems)
                return false;
        }

        return true;
    }

    private HashSet<Vector2Int> GetNearbyWalls(Vector2Int position, int radius)
    {
        HashSet<Vector2Int> nearbyWalls = new HashSet<Vector2Int>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector2Int checkPos = position + new Vector2Int(x, y);
                if (!floor.Contains(checkPos))
                    nearbyWalls.Add(checkPos);
            }
        }
        return nearbyWalls;
    }

    private void PlaceItem(Vector2Int position, ItemPlacementRule rule)
    {
        GameObject propsContainer = GameObject.Find("Grid/Props");
        if (propsContainer == null)
        {
            propsContainer = new GameObject("Props");
            propsContainer.transform.parent = GameObject.Find("Grid").transform;
        }

        Vector3 worldPosition = new Vector3(position.x, position.y, 0);
        GameObject item = Instantiate(rule.prefab, worldPosition, Quaternion.identity);
        item.transform.parent = propsContainer.transform;
        placedItems.Add(position, item);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(
                Mathf.RoundToInt(roomBounds.center.x), 
                Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && 
                   position.x <= (roomBounds.xMax - offset) && 
                   position.y >= (roomBounds.yMin - offset) && 
                   position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    protected virtual HashSet<Vector2Int> GetCorridors() => corridors;
    protected virtual List<BoundsInt> GetRooms() => roomsList;
}