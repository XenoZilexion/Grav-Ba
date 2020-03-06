using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float speedLimit = 5.0f;
    public float smoothingValue = 0.1f;

    public float jumpForce = 1.0f;
    public float jumpMultiplier = 1.5f;
    public float fallMultiplier = 2.5f;


    public bool grounded;
    public LayerMask whatIsGround;

    public Transform groundCheck;
    public float groundCheckRadius;


    private float horizontalInput;
    private Rigidbody2D rb;
    private Animator anim;
    private CameraController cc;
    private Vector3 positionChange;

    private Vector3 currentVelocity = Vector3.zero;

    // grapple implementation
    public Vector2 facingDirection;
    public SpriteRenderer sr_Component;
    public Grapple grapple_Component;
    public bool grappling;

    //input buffering/coyote time
    public bool canJump = false;
    public bool jumping = false;
    public bool attemptJump = false;
    public float coyoteDuration;
    public float bufferDuration;
    public float coyoteEnd = -1;
    public float bufferEnd = -1;
    public float jumpCooldown;
    public float jumpEnd = -1;

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
        cc = GetComponent<CameraController>();
        sr_Component = GetComponent<SpriteRenderer>();
        grapple_Component = GetComponent<Grapple>();
    }

    void Update()
    {
        //Debug.Log();
        ControllerInput();

        CheckGround();
        Fall();
        FacingDirection();
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {

    }

    private void ControllerInput()
    {
        if (!grappling)
        {
            if (horizontalInput != 0)
            {
                anim.SetBool("Run", true);
                if (horizontalInput > 0)
                {
                    sr_Component.flipX = false;
                }
                else
                {
                    sr_Component.flipX = true;
                }
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

            /*
            if (Input.GetKeyDown(KeyCode.C) && grounded)
            {
                Jump();
            }
            */

            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Key presed");
                attemptJump = true;
                bufferEnd = Time.time + bufferDuration;
            }
            


        }
    }

    private void CheckGround()
    {
        //if (!grappling)
        // {

        if (Time.time >= jumpEnd) {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
        else {
            grounded = false;
        }


        if (grounded == true)
        {

            canJump = true;
            jumping = false;
            coyoteEnd = Time.time + coyoteDuration;

        }
        else if (grounded == false && coyoteEnd>=Time.time)
        {
            canJump = true;
            jumping = false;
        }
        else
        {
            canJump = false;
            //jumping = false;
        }

        /*
        if (grounded == true)
        {
            
            canJump = true;
            jumping = false;
            coyoteEnd = Time.time + coyoteDuration;
        }
        else if (grounded == false && jumping == false && anim.GetBool("IsGrounded") == true)
        {
            coyoteEnd = Time.time + coyoteDuration;
        }
        


        if (coyoteEnd >= Time.time && !jumping && canJump)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        */
        //Debug.Log("buffer "+bufferEnd);
        //Debug.Log("time " + Time.time);
        //Debug.Log("can " + canJump);
        if (bufferEnd >= Time.time && canJump)
        {
            //Debug.Log("Jump Check 1");

            Jump();

            
        }

        if (bufferEnd < Time.time)
        {
            //Debug.Log("input buffer end by time");
            attemptJump = false;
        }
        // }
        anim.SetBool("IsGrounded", grounded);

    }

    private void Jump()
    {

        if (!grappling)
        {
            //Debug.Log("Jump Check 2");

            //Debug.Log("input buffer end by jump");
            jumping = true;
            attemptJump = false;
            canJump = false;
            bufferEnd = 0;
            coyoteEnd = 0;
            jumpEnd = Time.time + jumpCooldown;


            grounded = false;
            rb.gravityScale = jumpMultiplier;
            //Debug.Log("another check");
            switch (currentGravity)
            {
                case gravityDirection.down:
                    //Debug.Log("another another check");
                    rb.velocity = new Vector2(rb.velocity.x,0);
                    rb.AddForce(Vector2.up * jumpForce);
                    break;
                case gravityDirection.right:
                    //Debug.Log("another another check");
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    rb.AddForce(Vector2.left * jumpForce);
                    break;
                case gravityDirection.up:
                    //Debug.Log("another another check");
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.down * jumpForce);
                    break;
                case gravityDirection.left:
                    //Debug.Log("another another check");
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    rb.AddForce(Vector2.right * jumpForce);
                    break;
            }
        }
    }

    private void Fall()
    {
        if (!grappling && !cc.isRotating)
        {
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

            //Debug.Log(fallVelocity);
            if (fallVelocity < -0.0002f)
            {
                rb.gravityScale = fallMultiplier;
            }
            else
            {
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
