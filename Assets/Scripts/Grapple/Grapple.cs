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
    public GrappleTestPlayer player_Component;
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
    #endregion
    #region setup
    void Start()
    {
        rb_Component = GetComponent<Rigidbody2D>();
        player_Component = GetComponent<GrappleTestPlayer>();
        grappleRechargeTime = Time.time + 1;
    }
    #endregion
    #region updates
    void Update()
    {
        CheckCooldown();
        Reel();
        HoldCheck();

        Debug.Log(currentGrappleState);
        //Debug.Log(player_Component.rb_Component.velocity);
    }
    #endregion
    #region functions
    #region shooting
    public void StartGrapple()
    {
        holding = true;
        currentGrappleState = GrapplingState.Shooting;
        //player_Component.rb_Component.simulated = false;
        //player_Component.rb_Component.isKinematic = true;
        //player_Component.rb_Component.velocity = new Vector2(0,0);

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
        GameObject newBase = Instantiate(basePrefab, grappleOrigin);
        newBase.GetComponent<GrappleBase>().player_Component = player_Component;
        currentBase = newBase;

        GameObject newHook = Instantiate(hookPrefab, grappleOrigin.position, Quaternion.identity);
        newHook.GetComponent<Hook>().grapple_Component = this;
        newHook.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.facingDirection.y - 0, player_Component.facingDirection.x - 0) * Mathf.Rad2Deg);
        newHook.GetComponent<Rigidbody2D>().velocity = player_Component.facingDirection * grappleShootSpeed;
        currentHook = newHook;

        GameObject newRope = Instantiate(ropePrefab, grappleOrigin.position, Quaternion.identity);
        newRope.GetComponent<Rope>().grappleOrigin = currentBase.transform;
        newRope.GetComponent<Rope>().hookLocation = currentHook.transform;
        currentRope = newRope;
    }
    #endregion
    #region reeling
    public void HookLocked()
    {
        rb_Component.isKinematic = true;
        rb_Component.velocity = Vector2.zero;
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
        currentGrappleState = GrapplingState.Reeling;
    }
    public void Reel()
    {
        if (currentGrappleState == GrapplingState.Reeling)
        {

            //Debug.Log("this position: " + this.transform.position);
            //Debug.Log("Normalized: "+ Vector3.Normalize((this.transform.position + ((currentHook.transform.position - this.transform.position)/2))));
            //Debug.Log("Magnitude: " + Vector3.Magnitude(Vector3.Normalize((this.transform.position + ((currentHook.transform.position - this.transform.position))))));
            Vector3 newVelocity;
            newVelocity = (currentHook.transform.position - this.transform.position);
            newVelocity = Vector3.Normalize(newVelocity)*grappleReelSpeed;
            rb_Component.velocity = newVelocity;

            Debug.DrawLine(this.transform.position, (((this.transform.position + ((currentHook.transform.position - this.transform.position))))), Color.red,.1f);
            //Debug.DrawLine(this.transform.position, this.transform.position + (Vector3.Normalize((this.transform.position + ((currentHook.transform.position - this.transform.position) / 2))) * grappleReelSpeed), Color.red, .1f);
            //Debug.DrawLine(this.transform.position, currentHook.transform.position, Color.blue, .1f);

            if ((Vector2.Distance(this.transform.position, currentHook.transform.position)) <= (grappleReelSpeed * Time.fixedDeltaTime))
            {
                rb_Component.velocity = Vector2.zero;
                rb_Component.position = currentHook.transform.position;
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
        if (!holding && currentGrappleState == GrapplingState.Holding)
        {
            currentGrappleState = GrapplingState.Cooldown;
            rb_Component.isKinematic = false;
            grappleRechargeTime = Time.time + grappleCooldown;
            Destroy(currentRope);
            Destroy(currentHook);
            Destroy(currentBase);
            Debug.Log("Hold Release");
        }
    }

    public void HookBreak()
    {
        currentGrappleState = GrapplingState.Cooldown;
        grappleRechargeTime = Time.time + grappleCooldown;
        Destroy(currentRope);
        Destroy(currentHook);
        Destroy(currentBase);
        Debug.Log("Out Of Range");
    }

    #endregion
    #region cooldown
    void GrappleCooldownStart()
    {
        grappleRechargeTime = Time.time + grappleCooldown;
    }

    void CheckCooldown()
    {
        if (currentGrappleState == GrapplingState.Cooldown)
        {
            if (Time.time >= grappleRechargeTime)
            {
                currentGrappleState = GrapplingState.Ready;
            }
        }
    }
    #endregion
    #endregion
}
