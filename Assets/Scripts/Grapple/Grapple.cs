using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// Main Grapple Script
    /// </summary>
    #endregion
    #region variables
    // enum to manage the states of the grappling cycle
    public enum GrapplingState { Shooting, Reeling, Holding, Cooldown, Ready }
    public GrapplingState currentGrappleState = GrapplingState.Ready;
    // components
    public Rigidbody2D rb_Component;
    public PlayerController player_Component;
    // cooldown timer
    public float grappleCooldown;
    public float grappleRechargeTime;
    // timing of the grapple
    public float grappleShootDelay;
    public float grappleShootSpeed;
    public float grappleReelDelay;
    public float grappleReelSpeed;
    // range limit for the hook
    public float grappleRange;
    // prefabs
    public GameObject hookPrefab;
    public GameObject ropePrefab;
    public GameObject basePrefab;
    // locatio grappling instantiates from
    public Transform grappleOrigin;
    // current hook
    GameObject currentHook;
    GameObject currentBase;
    GameObject currentRope;
    // bool recording whether the player is holding the input
    public bool holding = false;
    // input button to use for grappling
    public string buttonName;
    #endregion
    #region setup
    void Start()
    {
        // get components
        rb_Component = GetComponent<Rigidbody2D>();
        player_Component = GetComponent<PlayerController>();
        // starting state is set to cooldown, this was done to prevent shooting when just starting playing, mostly for testing convenience sake.
        grappleRechargeTime = Time.time + 1;
    }
    #endregion
    #region updates
    void Update()
    {
        GetInputs();
        CheckCooldown();
        Reel();
        HoldCheck();
    }
    #endregion
    #region functions
    #region input
    public void GetInputs()
    {
        // Grapple input and holding
        if (Input.GetButtonDown(buttonName))
        {
            if (currentGrappleState == Grapple.GrapplingState.Ready)
            {
                StartGrapple();
            }
            else
            {
                holding = true;
            }
        }

        if (Input.GetButtonUp(buttonName))
        {
            ReleaseGrapple();
        }
    }
    #endregion
    #region shooting
    public void StartGrapple()
    {
        //begin hold, start shooting
        holding = true;
        currentGrappleState = GrapplingState.Shooting;
        // delay shoot if there is a delay, otherwise, shoot
        if (grappleShootDelay > 0)
        {
            Invoke("ShootGrapple", grappleShootDelay);
        }
        else
        {
            ShootGrapple();
        }
    }
    public void ShootGrapple()
    {
        // freeze player movement
        //rb_Component.isKinematic = true;

        rb_Component.gravityScale = 0;
        rb_Component.velocity = Vector2.zero;

        // create grapple base
        GameObject newBase = Instantiate(basePrefab, grappleOrigin);
        newBase.GetComponent<GrappleBase>().player_Component = player_Component;
        newBase.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.facingDirection.y - 0, player_Component.facingDirection.x - 0) * Mathf.Rad2Deg);
        currentBase = newBase;

        // create the grapple hook
        GameObject newHook = Instantiate(hookPrefab, grappleOrigin.position, Quaternion.identity);
        newHook.GetComponent<Hook>().grapple_Component = this;
        newHook.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.facingDirection.y - 0, player_Component.facingDirection.x - 0) * Mathf.Rad2Deg);
        newHook.GetComponent<Rigidbody2D>().velocity = player_Component.facingDirection * grappleShootSpeed;
        currentHook = newHook;

        // create the connecting section
        GameObject newRope = Instantiate(ropePrefab, grappleOrigin.position, Quaternion.identity);
        newRope.GetComponent<Rope>().grappleOrigin = currentBase.transform;
        newRope.GetComponent<Rope>().hookLocation = currentHook.transform;
        currentRope = newRope;
    }
    #endregion
    #region reeling
    public void HookLocked()
    {
        // old location of movement lock
        /*
        rb_Component.isKinematic = true;
        rb_Component.velocity = Vector2.zero;
        */
        // delay start of reeling if delay, otherwise, begin reeling in
        if (grappleReelDelay>0)
        {
            Invoke("BeginReel", grappleReelDelay);
        }
        else
        {
            BeginReel();
        }
        
    }
    public void BeginReel()
    {
        // set reel state
        currentGrappleState = GrapplingState.Reeling;
    }
    public void Reel()
    {
        // if reeling, 
        if (currentGrappleState == GrapplingState.Reeling)
        {
            // get direction to the hook, create velocity in that direction
            Vector3 newVelocity;
            newVelocity = (currentHook.GetComponent<Hook>().lockPoint.transform.position - this.transform.position);
            newVelocity = Vector3.Normalize(newVelocity)*grappleReelSpeed;
            rb_Component.velocity = newVelocity;

            // if going to overshoot the lockpoint, lock onto the lockpoint
            if ((Vector2.Distance(this.transform.position, currentHook.GetComponent<Hook>().lockPoint.transform.position)) <= (grappleReelSpeed * Time.fixedDeltaTime))
            {
                rb_Component.velocity = Vector2.zero;
                rb_Component.position = currentHook.GetComponent<Hook>().lockPoint.transform.position;
                currentGrappleState = GrapplingState.Holding;
            }
        }

        
    }
    public void ReleaseGrapple()
    {
        holding = false;
    }

    void HoldCheck()
    {
        // end hook hold
        if (!holding && currentGrappleState == GrapplingState.Holding)
        {
            currentGrappleState = GrapplingState.Cooldown;
            //rb_Component.isKinematic = false;

            rb_Component.gravityScale = 1;

            grappleRechargeTime = Time.time + grappleCooldown;
            Destroy(currentRope);
            Destroy(currentHook);
            Destroy(currentBase);
        }
    }

    public void HookBreak()
    {
        //break hook manually
        currentGrappleState = GrapplingState.Cooldown;
        //rb_Component.isKinematic = false;

        rb_Component.gravityScale = 1;

        grappleRechargeTime = Time.time + grappleCooldown;
        Destroy(currentRope);
        Destroy(currentHook);
        Destroy(currentBase);
    }

    #endregion
    #region cooldown
    void GrappleCooldownStart()
    {
        // cooldown set
        grappleRechargeTime = Time.time + grappleCooldown;
    }

    void CheckCooldown()
    {
        // check end of cooldown
        if (currentGrappleState == GrapplingState.Cooldown)
        {
            if (Time.time >= grappleRechargeTime)
            {
                currentGrappleState = GrapplingState.Ready;
            }
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentGrappleState == GrapplingState.Reeling)
        {
            rb_Component.velocity = Vector2.zero;
            currentGrappleState = GrapplingState.Holding;
        }
    }
    #endregion
}
