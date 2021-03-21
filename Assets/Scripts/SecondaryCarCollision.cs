using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCarCollision : MonoBehaviour
{

    public int[] lineWorkLoad = new int[5] { 0, 0, 0, 0, 0 };

    public bool lineChanging;
    public bool isCoDirection;

    public void Update()
    {
        if(lineChanging)//необходимо перестроится в другую полосу | need to change line
        {
            int currentLineNumber = gameObject.transform.parent.GetComponent<SecondaryMachineControl>().horizontalLineNumber;//получаем текущий номер линии | get current line number
            if (currentLineNumber == 3)
            {
                if (lineWorkLoad[currentLineNumber + 1] == 0)//если линия свободна | future line is free
                {
                    gameObject.transform.parent.GetComponent<SecondaryMachineControl>().Right();//движение вправо | move right
                    gameObject.transform.GetChild(0).GetComponent<SecondaryCarSideLinesCollision>().currentLineNumber += 1;//увеличиваем номер линии боковых триггеров | increasing the line index of side triggers
                    gameObject.transform.GetChild(1).GetComponent<SecondaryCarSideLinesCollision>().currentLineNumber += 1;
                    lineChanging = false;
                }
                
            }
            if (currentLineNumber == 4)
            {
                if(lineWorkLoad[currentLineNumber - 1] == 0)
                {
                    gameObject.transform.parent.GetComponent<SecondaryMachineControl>().Left();
                    gameObject.transform.GetChild(0).GetComponent<SecondaryCarSideLinesCollision>().currentLineNumber -= 1;
                    gameObject.transform.GetChild(1).GetComponent<SecondaryCarSideLinesCollision>().currentLineNumber -= 1;
                    lineChanging = false;
                }
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "SecondaryCar" || other.tag == "Target" || other.tag == "Character")//машина столкнулась с другой машиной | collision with other car
        {
            gameObject.GetComponentInParent<SecondaryMachineControl>().moving = false;
            if (other.gameObject.name == "SecondaryAccidentCoDirect")//машина натолкнулась на две столкнувшиеся, надо перестроится в свободную соседнюю полосу | collision with car accident, need to switch line
            {
                lineChanging = true;
            }
        }
        if (other.tag == "CarStop")//машина встала перед перекрестком | car stopped in front of crossroad
        {
            gameObject.GetComponentInParent<SecondaryMachineControl>().moving = false;
            other.gameObject.GetComponentInParent<CrossRoadControl>().stoppedCars.Add(gameObject.transform.parent.gameObject);
            other.gameObject.GetComponentInParent<CrossRoadControl>().stoppedCarsTags.Add("secondary");
        }
        if (other.tag == "BlockOfParallelInst")//машина заехала на перекресток | car is at crossroad
        {
            other.gameObject.GetComponentInParent<CrossRoadControl>().perpendicularCarsCount += 1;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "SecondaryCar" || other.tag == "Target" || other.tag == "Character")//машина больше не контактирует с другими машинами | no collision with other cars
        {
            StartCoroutine(StartMoveAfterTime(1f));//через секунду продолжить движение | continue movement after second
        }

        if(other.tag == "BlockOfParallelInst")//машина уехала с перекрестка | car left the crossroad
        {
            other.gameObject.GetComponentInParent<CrossRoadControl>().perpendicularCarsCount -= 1;
        }
    }

    public IEnumerator StartMoveAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponentInParent<SecondaryMachineControl>().moving = true;
    }
}
