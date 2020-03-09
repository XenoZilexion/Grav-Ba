using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlinkingText : MonoBehaviour
{
    //public float rotateSpeed = 150f;
    public Text text;
    public float blinkTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartBlinking();
    }

    void Update()
    {
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotateSpeed);
    }
    IEnumerator Blink()
    {
        while (true)
        {
            switch (text.color.a.ToString())
            {
                case "0":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                    yield return new WaitForSeconds(blinkTime);
                    break;
                case "1":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    yield return new WaitForSeconds(blinkTime);
                    break;
            }
        }
    }
        void StartBlinking()
        {
            StopCoroutine("Blink");
            StartCoroutine("Blink");
        }

        void StopBlinking()
        {

            StartCoroutine("Blink");
        }     
}
