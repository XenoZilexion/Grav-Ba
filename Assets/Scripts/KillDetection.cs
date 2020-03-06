using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDetection : MonoBehaviour
{
    public Vector3 restartPosition = new Vector3(-4.5f, 2.5f, 0);

    public void respawnPlayer()
    {
        Vector3 pScale = Vector3.one;
        transform.position = restartPosition;
        transform.localScale = new Vector3(1 / pScale.x, 1 / pScale.y, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This is checking the collision for a gameObject that has the
        //Script 'KillPlayer' Attached to it
        GameObject checkCollision = collision.gameObject;
        KillPlayer canKill = checkCollision.GetComponent<KillPlayer>();

        if (canKill != null)
        {
            respawnPlayer();
        }

        RespawnPoint respawnPoint = checkCollision.GetComponent<RespawnPoint>();

        if (respawnPoint != null)
        {
            restartPosition = transform.position;
        }
    }
    
}
