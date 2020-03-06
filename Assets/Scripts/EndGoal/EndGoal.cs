using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour
{
    #region summary
    /// <summary>
    /// Jared Moeller
    /// 3/5/2020
    /// End Goal Script
    /// </summary>
    #endregion
    #region variables
    public bool goalReached;
    public string playerTag;
    public float slowSpeed;
    public GameObject player;
    public float delayDuration;
    public string endSceneName;
    public float endTime;
    #endregion
    #region setup
    void Start()
    {
        
    }
    #endregion
    #region updates
    void Update()
    {
        if (goalReached == true)
        {
            if (Time.time<endTime)
            {
                Debug.Log("Slowdown");
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<PlayerController>().canJump = false;
                player.GetComponent<PlayerController>().grappling = true;
                player.GetComponent<Animator>().SetBool("Grappling", false);
                player.GetComponent<Animator>().SetBool("Run", false);
                //player.GetComponent<Animator>().SetBool("IsGrounded", false);

                // Save final time/score
                // stop time countdown

            }
            else
            {
                Debug.Log("Scene Change");
                SceneManager.LoadScene(endSceneName);
            }
        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Test");
        if (collision.gameObject.tag == playerTag && !goalReached)
        {
            Debug.Log("Trigger Restered");
            goalReached = true;
            player = collision.gameObject;
            endTime = Time.time + delayDuration;
        }
    }
}
