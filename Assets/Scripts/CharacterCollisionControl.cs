using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionControl : MonoBehaviour
{
    public List<GameObject> notAvaiableSegments = new List<GameObject> { };//неактивные сегменты уровня | not active level segments

    public GameObject currentSegment;
    public GameObject croosRoad;
    public GameObject characterControllPanel;
    public GameObject uncoveredText;


    public System.Random random = new System.Random();

    public bool isCrossRoadTime;
    


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("NewSegmentMarker"))//нужно передвинуть новый сегмент уровня | need to move new level segment
        {
            int newSegmentIndex = random.Next(0, notAvaiableSegments.Count);
            if (isCrossRoadTime)//нужно поставить перекресток + за ним поставить сегмент уровня | need to put crossroad + put a level segment behind it
            {
                GameObject crossroadPlace = currentSegment.GetComponent<LvlPrefabInfo>().crosroadPlace;
                croosRoad.transform.position = crossroadPlace.transform.position;
                notAvaiableSegments.Add(currentSegment);
                currentSegment = croosRoad;
                isCrossRoadTime = false;
                //ниже ставим сегмент уровня | put the level segment below
                GameObject newPlace = currentSegment.GetComponent<LvlPrefabInfo>().instantiateSegmentPlace;
                notAvaiableSegments[newSegmentIndex].transform.position = newPlace.transform.position;
                currentSegment = notAvaiableSegments[newSegmentIndex];
                notAvaiableSegments.RemoveAt(newSegmentIndex);

            }
            else
            {
                GameObject newPlace = currentSegment.GetComponent<LvlPrefabInfo>().instantiateSegmentPlace;
                notAvaiableSegments[newSegmentIndex].transform.position = newPlace.transform.position;
                notAvaiableSegments.Add(currentSegment);
                currentSegment = notAvaiableSegments[newSegmentIndex];
                notAvaiableSegments.RemoveAt(newSegmentIndex);
            }

        }
        if (collision.gameObject.CompareTag("TargetViewZone"))//игрок был замечен автомобилем-целью | player was spotted by the target
        {
            characterControllPanel.GetComponent<WinCondition>().uncoveredCount += 1;
            uncoveredText.SetActive(true);
            StartCoroutine(disableUncoveredText(2f));
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SecondaryCar") || collision.gameObject.CompareTag("Target") || collision.gameObject.CompareTag("Obstacle"))
        {
            characterControllPanel.GetComponent<WinCondition>().GameOver("accident");
        }

    }

    public IEnumerator disableUncoveredText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        uncoveredText.SetActive(false);
    }
}
