using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public GameObject player;

    private Transform playerTransform;
    private PlayerController pc;
    private int index = 0;
    private List<int> angles = new List<int>() { 0, 90, 180, 270 };
    private bool isRotating = false;
    private Quaternion targetRotation = Quaternion.AngleAxis(0, Vector3.forward);

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
        if (isRotating)
        {
            RotatePlayer();
        }
    }

    private void ControllerInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(--index < 0)
            {
                index = 3;
            }
            isRotating = true;
            targetRotation *= Quaternion.AngleAxis(-90, Vector3.forward);
            pc.CurrentRotation = angles[index];
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if(++index > 3)
            {
                index = 0;
            }
            isRotating = true;
            targetRotation *= Quaternion.AngleAxis(90, Vector3.forward);
            pc.CurrentRotation = angles[index];
        }
    }

    private void RotatePlayer()
    {
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
