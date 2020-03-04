using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMapping : MonoBehaviour
{
    private float horizontalInput;
    private float jumpInput;
    private PlayerController pc;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
    }

    void ControllerInput()
    {
        switch (pc.CurrentRotation){
            case 0:
                horizontalInput = Input.GetAxisRaw("Horizontal");
                break;
            case 90:
                horizontalInput = Input.GetAxisRaw("Vertical");
                break;
            case 180:
                horizontalInput = Input.GetAxisRaw("Horizontal") * -1;
                break;
            case 270:
                horizontalInput = Input.GetAxisRaw("Vertical") * -1;
                break;

        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            pc.Jump();
        }

        if (Input.GetButton("Rotate CW"))
        {

        }

        if (Input.GetButton("Rotate CCW"))
        {

        }

        if (Input.GetButton("Fire"))
        {

        }
    }
}
