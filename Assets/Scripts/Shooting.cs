using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float timeBtwShots;
    private float startTimeBtwShots;

    public GameObject projectile;
    public Transform target;

    void Start()
    {
        startTimeBtwShots = timeBtwShots;
    }

    // Update is called once per frame
    void Update()
    { 
        if (timeBtwShots < 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}