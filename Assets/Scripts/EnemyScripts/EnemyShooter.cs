using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    public GameObject bullet;
    public Transform firePoint;
    private Transform playerTransform;
    private GameManager gameManager;

    [Header("Combat Settings")]
    public float detectionRange = 5f;
    public float fireRate = 1f;
    public float bulletSpeed = 14f;      // Ajout d'une variable pour la vitesse des projectiles

    private float nextFireTime = 0f;
    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Récupérer le GameManager et appliquer le multiplicateur de difficulté
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            float multiplier = gameManager.GetMultiplier();
            // Ajuster les stats
            fireRate *= multiplier;         // Tire plus rapidement avec la difficulté
            bulletSpeed *= multiplier;      // Les projectiles sont plus rapides
            detectionRange *= multiplier;   // Détecte le joueur de plus loin
            
            Debug.Log($"EnemyShooter stats adjusted for difficulty (x{multiplier}):");
            Debug.Log($"Fire Rate: {fireRate}");
            Debug.Log($"Bullet Speed: {bulletSpeed}");
            Debug.Log($"Detection Range: {detectionRange}");
        }
        else
        {
            Debug.LogWarning("GameManager not found! Difficulty scaling won't be applied.");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;  // Sécurité si le joueur n'existe plus
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        Vector2 shootDirection = (playerTransform.position - transform.position).normalized;
        GameObject projectile = Instantiate(bullet, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;  // Utilise la vitesse ajustée
    }
}