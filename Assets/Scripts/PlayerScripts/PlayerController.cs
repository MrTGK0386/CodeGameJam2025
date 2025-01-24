using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera sceneCamera;
    public float moveSpeed;
    public Rigidbody2D rb;

    // DASH
    private bool canDash = true;
    private bool isDashing = false;
    public float dashPower;
    public float dashTime;
    public float dashCd;

    // MOVES
    private Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        if(isDashing){
            return;
        }

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

        moveDirection = new Vector2(moveX, moveY).normalized;

        // dash
        if (Input.GetButton("Jump") && canDash)
        {
            
            StartCoroutine(Dash());
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed); 
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Rapid, short burst of speed
        float originalSpeed = moveSpeed;
        moveSpeed *= dashPower; // Temporary speed boost

        // Short duration of accelerated movement
        yield return new WaitForSeconds(dashTime);

        // Restore original speed
        moveSpeed = originalSpeed;

        // Cooldown
        yield return new WaitForSeconds(dashCd);

        isDashing = false;
        canDash = true;
}
    
    
}
