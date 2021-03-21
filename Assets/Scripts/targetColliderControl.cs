using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetColliderControl : MonoBehaviour
{
    public GameObject target;

    public int[] lineWorkLoad = new int[5] { 0, 0, 0, 0, 0 };

    public bool lineChanging;
    // same as SecondaryCarCollision
    public void Update()
    {
        if (lineChanging)
        {
            int currentLineNumber = target.GetComponent<targetControl>().horizontalLineNumber;
            if (currentLineNumber == 3)
            {
                if (lineWorkLoad[currentLineNumber + 1] == 0)
                {
                    gameObject.transform.parent.GetComponent<targetControl>().Right();
                    gameObject.transform.GetChild(0).GetComponent<TargetSideLinesCollision>().currentLineNumber += 1;
                    gameObject.transform.GetChild(1).GetComponent<TargetSideLinesCollision>().currentLineNumber += 1;
                    lineChanging = false;
                }
            }
            if (currentLineNumber == 4)
            {
                if (lineWorkLoad[currentLineNumber - 1] == 0)
                {
                    gameObject.transform.parent.GetComponent<targetControl>().Left();
                    gameObject.transform.GetChild(0).GetComponent<TargetSideLinesCollision>().currentLineNumber -= 1;
                    gameObject.transform.GetChild(1).GetComponent<TargetSideLinesCollision>().currentLineNumber -= 1;
                    lineChanging = false;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character"))
        {
            gameObject.GetComponentInParent<targetControl>().moving = false;
            if(other.gameObject.name == "SecondaryAccidentCoDirect")
            {
                lineChanging = true;
            }
        }
        if(other.CompareTag("CarStop"))
        {
            gameObject.GetComponentInParent<targetControl>().moving = false;
            other.gameObject.GetComponentInParent<CrossRoadControl>().stoppedCars.Add(gameObject.transform.parent.gameObject);
            other.gameObject.GetComponentInParent<CrossRoadControl>().stoppedCarsTags.Add("target");
        }
        if (other.CompareTag("BlockOfParallelInst"))//
        {
            other.gameObject.GetComponentInParent<CrossRoadControl>().perpendicularCarsCount += 1;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Character"))
        {
            StartCoroutine(StartMoveAfterTime(1f));
        }

        if (other.CompareTag("BlockOfParallelInst"))// 
        {
            other.gameObject.GetComponentInParent<CrossRoadControl>().perpendicularCarsCount -= 1;
        }
    }

    public IEnumerator StartMoveAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponentInParent<targetControl>().moving = true;
    }
}
