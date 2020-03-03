using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator cameraStateMachine;
    private int index = 0;
    private List<int> angles = new List<int>() { 0, 90, 180, 270 };
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
    }

    private void ControllerInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(--index < 0)
            {
                index = 3;
            }
            cameraStateMachine.SetTrigger("AngleChanged");
            cameraStateMachine.SetInteger("Angle", angles[index]);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(++index > 3)
            {
                index = 0;
            }
            cameraStateMachine.SetTrigger("AngleChanged");
            cameraStateMachine.SetInteger("Angle", angles[index]);
        }
    }
}
