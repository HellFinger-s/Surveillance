using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCarSideLinesCollision : MonoBehaviour
{
    public int currentLineNumber;//номер текущей линии

    public GameObject[] positions = new GameObject[0];

    public string state;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character") || other.CompareTag("Target"))//сбоку от родительской машины другая машина/игрок/цель | to the side of the parent car is another car/player/target
        {
            gameObject.GetComponentInParent<SecondaryCarCollision>().lineWorkLoad[currentLineNumber] += 1;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character") || other.CompareTag("Target"))//машина, с котором контактировали, выехала | no more contact with the collided car
        {
            gameObject.GetComponentInParent<SecondaryCarCollision>().lineWorkLoad[currentLineNumber] -= 1;
        }
    }


    public void LineNumberDetermining(string newState)//при генерации новой машины мы переопределяем номер текущей линии, поскольку он каждый раз разный | 
                                                      //when generating a new machine, we redefine the current line number, since it is different each time
    {
        if (state == "leftSide")
        {
            currentLineNumber = gameObject.transform.parent.transform.parent.GetComponent<SecondaryMachineControl>().horizontalLineNumber - 1;
        }
        if(state == "rightSide")
        {
            currentLineNumber = gameObject.transform.parent.transform.parent.GetComponent<SecondaryMachineControl>().horizontalLineNumber + 1;

        }
        state = newState;
    }
}
