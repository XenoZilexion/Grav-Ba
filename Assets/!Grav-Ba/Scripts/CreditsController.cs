using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public Text creditText;
    public float credits;
    // Start is called before the first frame update
    void Start()
    {
        creditText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("3"))
        {
            credits += 0.25f;               
        }
        if (credits <= 0.99)
        {
            creditText.text = ("Credits: " + credits);          
        }
        else
        {
            creditText.text = ("Press any button to play " + credits);
            if (Input.GetKey("1"))
            {
                SceneManager.LoadScene(1);
            }
        }       
    }
}
