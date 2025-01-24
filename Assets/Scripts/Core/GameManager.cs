using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player = null;
    
    // Start is called before the first frame update
    //void Start()
   

    // Update is called once per frame
    //void Update()
    

    public void StartGame()
    {
        ResetEnnemyStats();
        ResetPlayerPosition();
        //ApplyPlayerStats();
    }

    private void ResetEnnemyStats()
    {
        //Ici mettre le tableau des stats à zéro
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = Vector2.zero; //adapté en fonction de la position de spawn dans le donjon
    }

    private void ApplyPlayerStats()
    {
        player.moveSpeed = newMoveSpeed;
        player.dashSpeed = newDashSpeed;
        player.dashCooldown = newDashCooldown;

        player.maxHealth = newMaxHealth;
        player.currentHealth = player.maxHealth;
        
    }
    
    
}
