using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public Camera sceneCamera;
    public Rigidbody2D rb;
    private Vector2 mousePosition;
    public GameObject bullet;
    public Transform firePoint;

    public float fireForce;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Aim();
    }

    public void Fire()
    {
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

        // PLAY SOUND HERE
    }

    void ProcessInputs(){
        if(Input.GetMouseButtonDown(0)){
            Fire();
        }
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Aim()
    {
        //
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
