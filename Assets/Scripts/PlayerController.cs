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
    private Animator anim;
    private Vector3 positionChange;

    private Vector3 currentVelocity = Vector3.zero;

    // grapple implementation
    public Vector2 facingDirection;
    public SpriteRenderer sr_Component;
    public Grapple grapple_Component;
    public bool grappling;

    private enum gravityDirection
    {
        down,
        right,
        up,
        left
    };

    private gravityDirection currentGravity = gravityDirection.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr_Component = GetComponent<SpriteRenderer>();
        grapple_Component = GetComponent<Grapple>();
    }

    void Update()
    {
        CheckGround();
        Fall();
        FacingDirection();
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        ControllerInput();
    }

    private void ControllerInput()
    {
        if (!grappling)
        {
            if (horizontalInput != 0)
            {
                anim.SetBool("Run", true);
                Vector3 targetVelocity = new Vector2();
                switch (currentGravity)
                {
                    case gravityDirection.down:
                        targetVelocity = new Vector2(moveSpeed * horizontalInput, rb.velocity.y);
                        break;
                    case gravityDirection.right:
                        targetVelocity = new Vector2(rb.velocity.x, moveSpeed * horizontalInput);
                        break;
                    case gravityDirection.up:
                        targetVelocity = new Vector2(-1 * moveSpeed * horizontalInput, rb.velocity.y);
                        break;
                    case gravityDirection.left:
                        targetVelocity = new Vector2(rb.velocity.x, -1 * moveSpeed * horizontalInput);
                        break;
                }


                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothingValue);
            }
            else
            {
                anim.SetBool("Run", false);
                switch (currentGravity)
                {
                    case gravityDirection.down:
                        rb.velocity = new Vector2(0.0f, rb.velocity.y);
                        break;
                    case gravityDirection.right:
                        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                        break;
                    case gravityDirection.up:
                        rb.velocity = new Vector2(0.0f, rb.velocity.y);
                        break;
                    case gravityDirection.left:
                        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.C) && grounded)
            {
                Jump();
            }
        }
    }

    private void CheckGround()
    {
        if (!grappling) {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    private void Jump()
    {
        if (!grappling) {
            grounded = false;
            anim.SetBool("Jump", true);
            switch (currentGravity)
            {
                case gravityDirection.down:
                    rb.AddForce(Vector2.up * jumpForce);
                    break;
                case gravityDirection.right:
                    rb.AddForce(Vector2.left * jumpForce);
                    break;
                case gravityDirection.up:
                    rb.AddForce(Vector2.down * jumpForce);
                    break;
                case gravityDirection.left:
                    rb.AddForce(Vector2.right * jumpForce);
                    break;
            }
        }
    }

    private void Fall()
    {
        if (!grappling) {
            float fallVelocity = 0.0f;
            switch (currentGravity)
            {
                case gravityDirection.down:
                    fallVelocity = rb.velocity.y;
                    break;
                case gravityDirection.right:
                    fallVelocity = -rb.velocity.x;
                    break;
                case gravityDirection.up:
                    fallVelocity = -rb.velocity.y;
                    break;
                case gravityDirection.left:
                    fallVelocity = rb.velocity.x;
                    break;
            }

            if (fallVelocity < 0)
            {
                rb.gravityScale = fallMultiplier;
                anim.SetBool("Fall", true);
                anim.SetBool("Jump", false);
            }
            else
            {
                rb.gravityScale = 1.0f;
                anim.SetBool("Fall", false);
            }
        }
    }

    public void ChangeGravity(int angle)
    {
        switch (angle)
        {
            case 0:
                Physics2D.gravity = new Vector2(0.0f, -9.81f);
                currentGravity = gravityDirection.down;
                break;

            case 90:
                Physics2D.gravity = new Vector2(9.81f, 0.0f);
                currentGravity = gravityDirection.right;
                break;

            case 180:
                Physics2D.gravity = new Vector2(0.0f, 9.81f);
                currentGravity = gravityDirection.up;
                break;

            case 270:
                Physics2D.gravity = new Vector2(-9.81f, 0);
                currentGravity = gravityDirection.left;
                break;
        }
    }

    void FacingDirection()
    {
        if (!sr_Component.flipX)
        {
            facingDirection = this.transform.localToWorldMatrix * Vector3.right;
        }
        else
        {
            facingDirection = this.transform.localToWorldMatrix * Vector3.left;
        }

        if (grapple_Component.currentGrappleState == Grapple.GrapplingState.Cooldown || grapple_Component.currentGrappleState == Grapple.GrapplingState.Ready)
        {
            grappling = false;
        }
        else
        {
            grappling = true;
        }
    }
}
