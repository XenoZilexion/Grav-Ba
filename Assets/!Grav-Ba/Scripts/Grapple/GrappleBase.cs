using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBase : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// Simple script to set rotation of the grapple base. to match the player's rotation.
    /// </summary>
    #endregion
    #region variables
    // reference to parent
    public PlayerController player_Component;
    SpriteRenderer sr_Component;
    #endregion
    #region setup
    private void Start()
    {
        sr_Component = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion
    #region updates
    void Update()
    {
        // sets rotation to direction player is facing
        SetBaseRotation();
    }
    #endregion
    #region functions
    void SetBaseRotation()
    {
        // set the grapple base to face the direction the player faces.
        // this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.facingDirection.y - 0, player_Component.facingDirection.x - 0) * Mathf.Rad2Deg);
        // sets rotation so that both ends of the rope will touch the origin/hook respectively
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.grapple_Component.currentHook.transform.position.y - this.transform.position.y, player_Component.grapple_Component.currentHook.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg);
        // set rope orientation to match player facing direction
        if (player_Component.sr_Component.flipX)
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
