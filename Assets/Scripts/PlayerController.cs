using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public Vector2 jumpVector;

    public bool grounded;
    public LayerMask whatIsGround;
    public bool stoppedJumping;

    public Transform groundCheck;
    public float groundCheckRadius;

    private float horizontalInput;
    private Rigidbody2D rb;
    private Vector3 positionChange;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        ControllerInput();
    }

    private void ControllerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        positionChange = transform.position;

        positionChange.x = positionChange.x + (horizontalInput * moveSpeed);

        rb.MovePosition(positionChange);

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
        rb.velocity = jumpVector;
    }

}
