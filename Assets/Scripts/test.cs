using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float fuTimer = 10;
    public float uTimer = 10;

    private void Update()
    {
        if(uTimer > 0)
        {
            uTimer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("UPDATE");
        }
    }

    private void FixedUpdate()
    {
        if(fuTimer > 0)
        {
            fuTimer -= 0.02f;
        }
        else
        {
            Debug.Log("FIXED UPDATE");
        }
    }
}
