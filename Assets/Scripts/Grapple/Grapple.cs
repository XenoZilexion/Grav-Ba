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
    public enum GrapplingState { Shooting, Reeling, Holding, Cooldown, Ready }
    public GrapplingState currentGrappleState = GrapplingState.Ready;

    public Rigidbody2D rb_Component;
    public GrappleTestPlayer player_Component;

    public float grappleCooldown;
    public float grappleRechargeTime;

    public float grappleShootDelay;
    public float grappleShootSpeed;
    public float grappleReelDelay;
    public float grappleReelSpeed;

    public float grappleRange;

    public GameObject hookPrefab;
    public GameObject ropePrefab;
    public GameObject basePrefab;
    public Transform grappleOrigin;
    GameObject currentHook;
    GameObject currentBase;
    GameObject currentRope;

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
        HoldCheck();

        Debug.Log(currentGrappleState);
        Debug.Log(player_Component.rb_Component.velocity);
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


        ShootGrapple();


    }

    public void ShootGrapple()
    {
        GameObject newBase = Instantiate(basePrefab, grappleOrigin);
        newBase.GetComponent<GrappleBase>().player_Component = player_Component;
        currentBase = newBase;

        GameObject newHook = Instantiate(hookPrefab, grappleOrigin.position, Quaternion.identity);

        currentHook = newHook;

        GameObject newRope = Instantiate(ropePrefab, grappleOrigin.position, Quaternion.identity);
        newRope.GetComponent<Rope>().grappleOrigin = currentBase.transform;
        newRope.GetComponent<Rope>().hookLocation = currentHook.transform;
        currentRope = newRope;
    }
    #endregion
    #region reeling
    public void BeginReel()
    {

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
            grappleRechargeTime = Time.time + grappleCooldown;
            Destroy(currentRope);
            Destroy(currentHook);
            Destroy(currentBase);

        }
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
