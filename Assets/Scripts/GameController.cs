using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{

    public enum gravity
    {
        down,
        right,
        up,
        left
    };

    public gravity currentGravity = gravity.down;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
