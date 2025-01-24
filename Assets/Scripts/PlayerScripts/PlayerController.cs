using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera sceneCamera;
    public float moveSpeed;
    public Rigidbody2D rb;

    // MOVES
    private Vector2 moveDirection;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing = false;
    bool canDash;

    // Ajoutez les autres variables de script
    private PlayerStats stats;


    // Update is called once per frame
    void Update()
    {
        // Physics Calculation
        if(isDashing)
        {
            return;
        }

        ProcessInputs();
    }

    private void Start()
    {
        canDash = true;
    }

    /// <summary>
    /// Update every fixed frame
    /// </summary>
    void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        // Physics Calculation
        Move();
    }

    void ProcessInputs(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if(Input.GetButtonDown("Jump") && canDash)
        {
            Debug.Log("here");
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
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    } 

}
