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
    public GrappleTestPlayer player_Component;
    #endregion

    #region updates
    void Update()
    {
        SetBaseRotation();
    }
    #endregion
    #region functions
    void SetBaseRotation()
    {
        // set the grapple base to face the direction the player faces.
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(player_Component.facingDirection.y - 0, player_Component.facingDirection.x - 0) * Mathf.Rad2Deg);
    }
    #endregion

}
