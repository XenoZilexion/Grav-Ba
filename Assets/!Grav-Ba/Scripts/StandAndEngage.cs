using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAndEngage : MonoBehaviour
{
    public float speed;
    public float EngageDistance;
    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stand still player isnt close enough
        if (Vector3.Distance(transform.position, player.position) > EngageDistance)
        {
            transform.position = this.transform.position;
        }
        //Chase Player they have become close enough
        else if (Vector3.Distance(transform.position, player.position) < EngageDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}