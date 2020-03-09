﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTestPlayer : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// This script is a temporary player controller for grapple testing, it will not be commented further as it is merely for testing purposes.
    /// </summary>
    /// 
    #endregion
    #region variables
    public Rigidbody2D rb_Component;
    public Grapple grapple_Component;
    public SpriteRenderer sr_Component;

    public float moveSpeed;
    public float jumpSpeed;
    public float maxFallSpeed;

    public Vector2 facingDirection = Vector2.right;

    public bool grounded = true;
    public bool grappling = false;
    #endregion
    void Start()
    {
        rb_Component = GetComponent<Rigidbody2D>();
        grapple_Component = GetComponent<Grapple>();
        sr_Component = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

    }

    void GetInputs()
    {
        if (!sr_Component.flipX)
        {
            facingDirection = Vector2.right;
        }
        else
        {
            facingDirection = Vector2.left;
        }

        if (grapple_Component.currentGrappleState == Grapple.GrapplingState.Cooldown || grapple_Component.currentGrappleState == Grapple.GrapplingState.Ready)
        {
            grappling = false;
        }
        else
        {
            grappling = true;
        }

        

        if (!grappling)
        {
            rb_Component.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb_Component.velocity.y);
            if (rb_Component.velocity.y < maxFallSpeed)
            {
                rb_Component.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, maxFallSpeed);
            }
            if (Input.GetButtonDown("Jump") && grounded == true)
            {
                rb_Component.velocity = new Vector2(rb_Component.velocity.x, 0);
                rb_Component.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
        }
    }


}
