using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camContr : MonoBehaviour
{
    public Transform target;
    public float xOffset;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x - xOffset, gameObject.transform.position.y, target.transform.position.z);
    }
}
