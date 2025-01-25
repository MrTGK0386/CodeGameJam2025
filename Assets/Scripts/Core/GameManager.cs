using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject player;
    public RandomMusic musicManager;
    public RandomColors discObject;
    public float scaler = 1.5f;
    
    private float difficulty = 1.0f;
    private static int  compteurEtage = 0;
    private bool isBoss = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
            musicManager = FindObjectOfType<RandomMusic>();
            discObject = FindObjectOfType<RandomColors>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SetMusic();
        SetDisc();
        SceneManager.LoadScene("Main");
        //dungeonGenerator.RegenerateDungeon();
    }

    public void NextStage()
    {
        compteurEtage++;
        UpDifficulty();
        SceneManager.LoadScene("Main");
    }

    public void EndGame()
    {
        compteurEtage = 0;
        difficulty = 1f;
        SceneManager.LoadScene("Lobby");
    }

    private void UpDifficulty()
    {
        difficulty += scaler;
        
        if (compteurEtage % 5 == 0)
        {
            isBoss = true;
        }
        else
        {
            isBoss = false;
        }
    }

    private void SetMusic()
    {
        if (musicManager != null)
        {
            Debug.Log("Musique !"); 
            musicManager.PlayRandomSound();
        }
        else
        {
            Debug.LogWarning("Music Manager is null");
        }
    }

    private void SetDisc()
    {
        if (discObject != null)
        {
            discObject.GenerateColor();
        }
        else
        {
            Debug.LogWarning("Disc Object is null");
        }
    }


    public float GetMultiplier()
    {
        return difficulty;
    }

    public float GetFloor()
    {
        return compteurEtage;
    }

    public bool GetIsBoss()
    {
        return isBoss;
    }
}
