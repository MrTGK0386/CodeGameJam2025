using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float detectionRange = 5f;
    public float fireRate = 1f;
    public GameObject bullet;
    public Transform firePoint;

    private Transform playerTransform;
    private float nextFireTime = 0f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
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
        projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * 14f;
    }
}