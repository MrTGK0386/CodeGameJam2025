using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject bullet;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Wall":
                Destroy(gameObject);
                break;
                
            case "Enemy":
                // other.GameObject.GetComponent<MyEnemyScript>().TakeDamage();
                // Handle Enemy Collision
                Destroy(gameObject);
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
