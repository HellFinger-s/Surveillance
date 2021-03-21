using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSideLinesCollision : MonoBehaviour
{
    public int currentLineNumber;

    public GameObject[] positions = new GameObject[0];

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character"))
        {
            gameObject.GetComponentInParent<targetColliderControl>().lineWorkLoad[currentLineNumber] += 1;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character"))
        {
            gameObject.GetComponentInParent<targetColliderControl>().lineWorkLoad[currentLineNumber] -= 1;
        }
    }


    public void LineNumberDetermining()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if(gameObject.transform.position.z == positions[i].transform.position.z)
            {
                currentLineNumber = i;
                break;
            }
        }
    }
}
