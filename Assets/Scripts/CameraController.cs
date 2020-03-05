using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public GameObject player;
    public float rotationThreshold = 1.0f;

    private Transform playerTransform;
    private PlayerController pc;
    private Rigidbody2D rb;
    private int index = 0;
    private List<int> angles = new List<int>() { 0, 90, 180, 270 };
    public bool isRotating = false;
    private bool rotatable = false;
    private Quaternion targetRotation = Quaternion.AngleAxis(0, Vector3.forward);

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
        if (rotatable)
        {
            RotatePlayer();
        }
    }

    private void ControllerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(--index < 0)
            {
                index = 3;
            }
            if (!pc.grappling)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            isRotating = true;
            rotatable = true;
            targetRotation *= Quaternion.AngleAxis(-90, Vector3.forward);
            pc.ChangeGravity(angles[index]);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(++index > 3)
            {
                index = 0;
            }
            if (!pc.grappling)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            isRotating = true;
            rotatable = true;
            targetRotation *= Quaternion.AngleAxis(90, Vector3.forward);
            pc.ChangeGravity(angles[index]);
        }
    }

    private void RotatePlayer()
    {
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(Quaternion.Angle(playerTransform.rotation, targetRotation)) <= rotationThreshold && isRotating)
        {
            if (!pc.grappling)
            {
                rb.gravityScale = 1;
            }
            isRotating = false;
        }

        /*
        if (Mathf.Abs(playerTransform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) <= rotationThreshold && isRotating)
        {
            if (!pc.grappling)
            {
                rb.gravityScale = 1;
            }
            isRotating = false;
        }
        */

        if (playerTransform.rotation == targetRotation)
        {
            rotatable = false;
            if (!pc.grappling)
            {
                rb.gravityScale = 1;
            }
            isRotating = false;
        }
    }
}
