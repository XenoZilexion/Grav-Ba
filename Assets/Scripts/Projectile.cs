﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    [SerializeField] private Transform targetTransform;
    private Vector3 target;

    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Destroy(other.gameObject);
        }
    }
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}