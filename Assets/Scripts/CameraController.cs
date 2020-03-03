using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationSpeed = 30.0f;

    private float elapsedRotation = 0.0f;
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
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
        }
    }

    private void Rotate(int direction)
    {
        while (elapsedRotation < 90.0f)
        {
            elapsedRotation += rotationSpeed * Time.deltaTime;
            transform.RotateAround(playerTransform.position, Vector3.forward, direction * elapsedRotation);
        }

        elapsedRotation = 0.0f;
    }
}
