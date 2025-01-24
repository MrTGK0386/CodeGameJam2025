using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileWithWeight
{
   public TileBase tile;
   [Range(0, 100)]
   public float weight = 1f;
}

public class TilemapVisualizer : MonoBehaviour
{
   [SerializeField]
   private Tilemap floorTilemap, wallTilemap;
   [SerializeField]
   private TileWithWeight[] floorTiles;
   [SerializeField]
   private TileWithWeight[] wallTopTiles;
   [SerializeField]
   private TileWithWeight[] wallBottomTiles;
   [SerializeField]
   private TileBase wallSideRight, wallSiderLeft, wallFull,
       wallInnerCornerDownLeft, wallInnerCornerDownRight,
       wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

   private TileBase GetRandomTileFromWeighted(TileWithWeight[] tiles)
   {
       float totalWeight = 0f;
       foreach (var tileWeight in tiles)
       {
           totalWeight += tileWeight.weight;
       }

       float random = UnityEngine.Random.Range(0f, totalWeight);
       float currentWeight = 0f;
           
       foreach (var tileWeight in tiles)
       {
           currentWeight += tileWeight.weight;
           if (random <= currentWeight)
           {
               return tileWeight.tile;
           }
       }
       return tiles[0].tile;
   }

   public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
   {
       foreach (var position in floorPositions)
       {
           PaintSingleTile(floorTilemap, GetRandomTileFromWeighted(floorTiles), position);
       }
   }

   internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
   {
       int typeAsInt = Convert.ToInt32(binaryType, 2);
       TileBase tile = null;
       if (WallTypesHelper.wallTop.Contains(typeAsInt))
       {
           tile = GetRandomTileFromWeighted(wallTopTiles);
       }
       else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
       {
           tile = wallSideRight;
       }
       else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
       {
           tile = wallSiderLeft;
       }
       else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
       {
           tile = GetRandomTileFromWeighted(wallBottomTiles);
       }
       else if (WallTypesHelper.wallFull.Contains(typeAsInt))
       {
           tile = wallFull;
       }

       if (tile != null)
           PaintSingleTile(wallTilemap, tile, position);
   }

   private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
   {
       var tilePosition = tilemap.WorldToCell((Vector3Int)position);
       tilemap.SetTile(tilePosition, tile);
   }

   public void Clear()
   {
       floorTilemap.ClearAllTiles();
       wallTilemap.ClearAllTiles();
   }

   internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
   {
       int typeASInt = Convert.ToInt32(binaryType, 2);
       TileBase tile = null;

       if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
       {
           tile = wallInnerCornerDownLeft;
       }
       else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
       {
           tile = wallInnerCornerDownRight;
       }
       else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
       {
           tile = wallDiagonalCornerDownLeft;
       }
       else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
       {
           tile = wallDiagonalCornerDownRight;
       }
       else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
       {
           tile = wallDiagonalCornerUpRight;
       }
       else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
       {
           tile = wallDiagonalCornerUpLeft;
       }
       else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
       {
           tile = wallFull;
       }
       else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
       {
           tile = GetRandomTileFromWeighted(wallBottomTiles);
       }

       if (tile != null)
           PaintSingleTile(wallTilemap, tile, position);
   }
}