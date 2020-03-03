using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private float timer;

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("Time: " + Mathf.FloorToInt(timer));
    }
}
