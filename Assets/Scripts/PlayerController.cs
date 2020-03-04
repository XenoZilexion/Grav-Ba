using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    public float speedLimit = 5.0f;
    public float smoothingValue = 0.1f;

    [Header("Jump Settings")]
    public float jumpForce = 1.0f;
    public float fallMultiplier = 2.0f;

    [Header("Ground Collision Settings")]
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool grounded;


    private float horizontalInput;
    private Rigidbody2D rb;
    private Vector3 positionChange;
    private Vector3 currentVelocity = Vector3.zero;



    private class RotationEvent : UnityEvent<int> { } //empty class; just needs to exist
    private RotationEvent OnWorldRotation = new RotationEvent();

    private int currentRotation = 0;
    public int CurrentRotation
    {
        get { return currentRotation; }
        set
        {
            if (currentRotation == value) return;
            currentRotation = value;
            if (OnWorldRotation != null)
                OnWorldRotation.Invoke(currentRotation);
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        OnWorldRotation.AddListener(RotateWorld);
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
            Vector3 targetVelocity = new Vector2();
            switch (currentRotation)
            {
                case 0:
                    targetVelocity = new Vector2(moveSpeed * horizontalInput, rb.velocity.y);
                    break;
                case 90:
                    targetVelocity = new Vector2(rb.velocity.x, moveSpeed * horizontalInput);
                    break;
                case 180:
                    targetVelocity = new Vector2(-1 * moveSpeed * horizontalInput, rb.velocity.y);
                    break;
                case 270:
                    targetVelocity = new Vector2(rb.velocity.x, -1 * moveSpeed * horizontalInput);
                    break;
            }

            
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothingValue);
        }
        else
        {
            switch (currentRotation)
            {
                case 0:
                    rb.velocity = new Vector2(0.0f, rb.velocity.y);
                    break;
                case 90:
                    rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                    break;
                case 180:
                    rb.velocity = new Vector2(0.0f, rb.velocity.y);
                    break;
                case 270:
                    rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                    break;
            }
        }
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void Jump()
    {
        Vector2 force = new Vector2();
        switch (currentRotation)
        {
            case 0:
                force = Vector2.up * jumpForce;
                break;
            case 90:
                force = Vector2.left * jumpForce;
                break;
            case 180:
                force = Vector2.down * jumpForce;
                break;
            case 270:
                force = Vector2.right * jumpForce;
                break;
        }

        if (grounded)
        {
            rb.AddForce(force);
            grounded = false;
        }
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
    

    void RotateWorld(int i)
    {
        currentRotation = i;

        switch (currentRotation)
        {
            case 0:
                Physics2D.gravity = new Vector2(0.0f, -9.81f);
                break;

            case 90:
                Physics2D.gravity = new Vector2(9.81f, 0.0f);
                break;

            case 180:
                Physics2D.gravity = new Vector2(0.0f, 9.81f);
                break;

            case 270:
                Physics2D.gravity = new Vector2(-9.81f, 0);
                break;
        }
    }

    public void RotatePlayer()
    {

    }

}
