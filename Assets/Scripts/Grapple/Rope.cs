using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// Simple script to manage the rope connecting the hook to the player.
    /// </summary>
    #endregion
    #region variables
    // location of the player's grapple origin
    public Transform grappleOrigin;
    // current location of the hook
    public Transform hookLocation;
    // sprite renderer component
    public SpriteRenderer sr_Component;
    // Grapple
    public Grapple grapple_Component;
    #endregion
    #region setup
    void Start()
    {
        // get sprite renderer
        sr_Component = GetComponent<SpriteRenderer>();
    }
    #endregion
    #region updates
    void Update()
    {
        AdjustRope();
    }
    #endregion
    #region functions
    public void AdjustRope()
    {
        // set position to center between the player and the hook
        this.transform.position = (grappleOrigin.position + ((hookLocation.position - grappleOrigin.position) / 2));
        // set length to the distance between the player and the hook
        sr_Component.size = new Vector2(Vector2.Distance(grappleOrigin.position, hookLocation.position), sr_Component.size.y);
        // sets rotation so that both ends of the rope will touch the origin/hook respectively
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(hookLocation.position.y - grappleOrigin.position.y, hookLocation.position.x - grappleOrigin.position.x) * Mathf.Rad2Deg);
        // set rope orientation to match player facing direction
        if (grapple_Component.player_Component.sr_Component.flipX)
        {
            sr_Component.flipY = true;
        }
        else
        {
            sr_Component.flipY = false;
        }
    }
    #endregion
}
