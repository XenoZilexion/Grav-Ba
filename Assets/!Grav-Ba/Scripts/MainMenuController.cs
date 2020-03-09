using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //All these functoins are attacked to buttons to transition scenes
    public void play()
    {
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        SceneManager.LoadScene(2);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            SceneManager.LoadScene(1);
        }
        //if left control is pressed go back (joystick left)
        if (Input.GetKey("left"))
        {
            SceneManager.LoadScene(0);
        }       
    }
}
