﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// Script for managing the hook and communicating grapple functions with main script.
    /// </summary>
    /// 
    #endregion
    #region variables
        // whether or not hook has hooked into wall
    public bool hooked = false;
    // reference to components
    public Grapple grapple_Component;
    public Rigidbody2D rb_Component;
    // point of instantiation used for distance reference
    Vector3 origin;
    // location for player to lock onto
    public Transform lockPoint;
    public float playerRadiusOffset;
    public Vector3 grappleOriginOffset;
    //fix parts
    public SpriteRenderer sr_Component;

    //retracting
    public bool retracting = false;
    public float retractCutoff;
    public float retractSpeed;
    #endregion
    #region setup
    void Start()
    {
        // get component/set origin
        origin = this.transform.position;
        rb_Component = GetComponent<Rigidbody2D>();
        sr_Component = GetComponent < SpriteRenderer>();
        retracting = false;
        RotationFix();
    }
    #endregion
    #region updates
    void Update()
    {
        RangeCheck();
        
        Retract();
    }
    #endregion
    #region functions
    void RangeCheck()
    {
        // check if range has been exceeded
        if (!hooked && (Mathf.Abs(Vector2.Distance(origin, this.transform.position)) >= grapple_Component.grappleRange)&&!retracting)
        {
            // retract
            //Debug.Log("RetractStart");
            retracting = true;
            //rb_Component.velocity = (Vector3.Normalize(rb_Component.velocity) * -retractSpeed);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        // on collision with wall when not hooked, hook into wall
        if (collision.gameObject.tag == "Wall" && !hooked && !retracting)
        {
            grappleOriginOffset = grapple_Component.grappleOrigin.transform.position - grapple_Component.transform.position;

            // set point for player to lock onto
            //lockPoint.transform.position = collision.ClosestPoint(grapple_Component.player_Component.transform.position);

            //lockPoint.transform.position = collision.otherCollider.ClosestPoint(transform.position);
            lockPoint.transform.position = collision.collider.ClosestPoint(transform.position);

            //lockPoint.transform.position += new Vector3(grapple_Component.player_Component.facingDirection.x * -.48f, 0,0);
            lockPoint.transform.position = (lockPoint.transform.position+(Vector3.Normalize(grapple_Component.grappleOrigin.transform.position-lockPoint.transform.position)*playerRadiusOffset));
            lockPoint.transform.position -= grappleOriginOffset;
            // hook into wall
            hooked = true;
            //  stop hook
            origin = this.transform.position;
            rb_Component.velocity = Vector2.zero;
            rb_Component.isKinematic = true;
            // hook lock
            grapple_Component.HookLocked();
        }
    }

    void RotationFix()
    {
        if (grapple_Component.player_Component.sr_Component.flipX)
        {
            sr_Component.flipY = true;
        }
        else
        {
            sr_Component.flipY = false;
        }
    }

    void Retract()
    {
        if (retracting)
        {
            rb_Component.velocity = Vector3.Normalize(origin- this.transform.position) * retractSpeed;
            if (Vector3.Distance(this.transform.position, origin)<=retractCutoff)
            {
                grapple_Component.HookBreak();
            }
            if (Vector3.Distance(this.transform.position, origin) <= (retractSpeed*Time.fixedDeltaTime))
            {
                grapple_Component.HookBreak();
            }
        }
    }
    #endregion
}
