using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoEffect : MonoBehaviour
{
    private float timeBtwSpawn;
    public float startTimeBtwSpawns;
    public GameObject echo;
    //private Player play;
    // Start is called before the first frame update
    void Start()
    {
        //Player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwSpawn <=0)
        {
            GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(instance, 0.2f);
            timeBtwSpawn = startTimeBtwSpawns;
          
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}
