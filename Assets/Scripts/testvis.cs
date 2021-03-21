using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testvis : MonoBehaviour
{
    private void OnBecameVisible()
    {
        Debug.Log("VISIBLE");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("INVISIBLE");
    }
}
