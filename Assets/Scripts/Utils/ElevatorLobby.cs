using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLobby : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance != null) // Ajoutez cette vérification
        {
            if (other.CompareTag("Player"))
            {        
                GameManager.Instance.StartGame();
                Debug.Log("Le joueur est entré dans la zone");
            }
        }
        else
        {
            Debug.LogError("GameManager est manquant !");
        }
    }
}

