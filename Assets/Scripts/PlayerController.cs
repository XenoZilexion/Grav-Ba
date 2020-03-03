using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float speedLimit = 5.0f;
    public float smoothingValue = 0.1f;

    public float jumpForce = 1.0f;
    public float fallMultiplier = 2.0f;


    public bool grounded;
    public LayerMask whatIsGround;

    public Transform groundCheck;
    public float groundCheckRadius;

    private float horizontalInput;
    private Rigidbody2D rb;
    private Vector3 positionChange;

    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        Fall();

        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        ControllerInput();
    }

    private void ControllerInput()
    {

        if (horizontalInput != 0)
        {
            Vector3 targetVelocity = new Vector2(moveSpeed * horizontalInput, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothingValue);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            Jump();
        }
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void Jump()
    {
        grounded = false;
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void Fall()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else
        {
            rb.gravityScale = 1.0f;
        }
    }

}
