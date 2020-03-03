using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    /// <summary>
    /// Jared Moeller
    /// 3/3/2020
    /// Script for managing the hook and communicating grapple functions with main script.
    /// </summary>
    public bool hooked = false;
    public Grapple grapple_Component;
    Vector2 origin;
    Rigidbody2D rb_Component;



    // Start is called before the first frame update
    void Start()
    {
        origin = this.transform.position;
        rb_Component = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    { 
        RangeCheck();
    }

    void RangeCheck()
    {

        //Debug.Log(Mathf.Abs(Vector2.Distance(origin, this.transform.position)));
        //Debug.Log(grapple_Component.grappleRange);
        Debug.Log(origin);
        if (!hooked && (Mathf.Abs(Vector2.Distance(origin, this.transform.position)) >= grapple_Component.grappleRange))
        {
            grapple_Component.HookBreak();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" && !hooked)
        {
            hooked = true;
            origin = this.transform.position;
            rb_Component.velocity = Vector2.zero;
            grapple_Component.HookLocked();

        }
    }

    void HookBreak()
    {
        
    }
}
