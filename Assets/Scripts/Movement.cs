using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 7f;
    private Rigidbody2D playerRb;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }
    
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerRb.MovePosition(playerRb.position + (movement_vector * moveSpeed) * Time.deltaTime);
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;      
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
    }
}
