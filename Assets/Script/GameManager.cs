using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private float timer;
    public Text scoreText;
    [SerializeField] private float score = 0;
    private string scoreFormat = "#";
    public Text multiplierText;
    public float multiplier;
    public float comboCount;
    public float addScore;
    public float comboWindow;

    // Update is called once per frame
    void Update()
    {
        UpdateHud();
    }

    void UpdateHud()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("Time: " + Mathf.FloorToInt(timer));
        scoreText.text = "Score: " + score.ToString(scoreFormat);
        multiplierText.text = multiplier.ToString("Multiplier: " + multiplier);
    }
    public void Score()
    {
        if (comboCount == 0)
        {
            multiplier *= 1;
            score += addScore;
            comboCount++;
            StartCoroutine("Combo");
        }

        else if (comboCount >= 5)
        {
            multiplier *= 2;
            score += (addScore * 2);
            comboCount++;
            StopCoroutine("Combo");
        }
       
        else if (comboCount >= 10)
        {
            multiplier *= 2;
            score += (addScore * 4);
            comboCount++;
            StopCoroutine("Combo");
        }
    }

    IEnumerator Combo()
    {
        yield return new WaitForSeconds(comboWindow);
        comboCount = 0;
    }
}
