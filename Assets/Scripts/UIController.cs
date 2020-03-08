using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class UIController : MonoBehaviour
{
    public Text timerText;
    public Text ScoreText;
    public float Score;
    public float addedScore = 10;
    public float timeRemaining = 300;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timerText.text = "Time Remaining: " + Mathf.Round(timeRemaining);

        ScoreText.text = (Score.ToString());
    }

    void addScore()
    {
        Score += addedScore;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This is checking the collision for a gameObject that has the
        //Script 'EnemyHealth' Attached to it
        GameObject checkCollision = collision.gameObject;
        EnemyHealth attacked = checkCollision.GetComponent<EnemyHealth>();

        Collectable pickUp = checkCollision.GetComponent<Collectable>();

        if (attacked != null)
        {
            print("Attacked Enemy");
            Destroy(collision.gameObject);
            addScore();
        }

        if (pickUp != null)
        {
            AudioSource source = pickUp.gameObject.GetComponent<AudioSource>();
            if (!source.isPlaying)
            {
                source.Play();
            }
            print("Coin Grabbed");
            Destroy(collision.gameObject);
            addScore();
        }
    }
}


