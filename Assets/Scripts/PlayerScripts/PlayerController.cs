using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera sceneCamera;
    public float moveSpeed;
    public Rigidbody2D rb;

    // public Weapon weapon;
    public Vector2 moveDirection;
    private Vector2 mousePosition;

    // WEAPON BEHAVIOR
    public GameObject bullet;
    public Transform firePoint;

    public float fireForce;

    public void Fire()
    {
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

    }

    /// <summary>
    /// Update every fixed frame
    /// </summary>
    void FixedUpdate()
    {
        // Physics Calculation
        Move();
    }

    void ProcessInputs(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButtonDown(0)){
            Fire();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        // Rotate player to follow mouse
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
        
    }
    
    
}
