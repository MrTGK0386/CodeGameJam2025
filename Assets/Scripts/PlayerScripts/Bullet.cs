using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject bullet;

    // public GameObject impactEffet;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("aled");
        switch(other.gameObject.tag)
        {
            case "Wall":
                Destroy(gameObject);
                break;
                
            case "Enemy":
                Debug.Log("touched !");
                // other.GameObject.GetComponent<MyEnemyScript>().TakeDamage();
                // Handle Enemy Collision
                Impact();
                
                break;

        }
    }

    public void Impact()
    {
        // Instantiate(impactEffet, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HERE !");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
