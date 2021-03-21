using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMove : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(-speed, 0, 0);
    }
}
