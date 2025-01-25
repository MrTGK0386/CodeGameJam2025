using UnityEngine;

public class MapRegenerator : MonoBehaviour
{
    //public RoomFirstDungeonGenerator dungeonGenerator;
    public float regenerationInterval = 5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= regenerationInterval)
        {
            //dungeonGenerator.RegenerateDungeon();
            timer = 0f;
        }
    }
}