using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bulletDamage = 10f;
    public float bulletLifetime = 3f;  // Durée de vie de la balle en secondes

    void Start()
    {
        // Si aucun Rigidbody2D n'est assigné dans l'inspecteur, on en cherche un automatiquement
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // On améliore la détection des collisions pour éviter que les balles ne traversent les ennemis
        // En 2D, on utilise collisionDetectionMode au lieu de collisionDetection
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        Destroy(gameObject, bulletLifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // On ajoute des logs pour mieux comprendre quand les collisions se produisent
        Debug.Log($"Bullet a touché : {other.gameObject.name} avec le tag {other.gameObject.tag}");

        switch(other.gameObject.tag)
        {
            case "Wall":
                Debug.Log("Collision avec un mur - destruction de la balle");
                Destroy(gameObject);
                break;
                
            case "Enemy":
                Debug.Log("Collision avec un ennemi !");
                EnemyController enemyController = other.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(bulletDamage);
                    Debug.Log($"Dégâts infligés à l'ennemi : {bulletDamage}");
                }
                Impact();
                break;
            
        }
    }

    public void Impact()
    {
        Destroy(gameObject);
    }
}
