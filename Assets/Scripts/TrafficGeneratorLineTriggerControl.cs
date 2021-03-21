using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficGeneratorLineTriggerControl : MonoBehaviour
{
    public int lineNumber;//номер текущей полосы | current line number
    public GameObject trafficGenerator;
    private string generatorType;


    public void Start()
    {
        generatorType = trafficGenerator.GetComponent<TrafficGenerator>().generatorType;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Target") || other.CompareTag("BlockOfParallelInst"))//машина находится на месте спауна новой | car is located at the spawn site of the new one
        {
            if(generatorType == "back")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().backLineWorkload[lineNumber] += 1;//повышаем нагрузку соответствующей линии | increasing the load of the corresponding line
            }
            if(generatorType == "frontOnComing" || generatorType == "frontCoDirectional")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().frontLineWorkload[lineNumber] += 1;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SecondaryCar") || other.CompareTag("Target") || other.CompareTag("BlockOfParallelInst"))//машина вышла из места спауна | the car left the spawn site
        {
            if (generatorType == "back")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().backLineWorkload[lineNumber] -= 1;//уменьшаем нагрузку соответствующей линии | reducing the load of the corresponding line
            }
            if (generatorType == "frontOnComing" || generatorType == "frontCoDirectional")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().frontLineWorkload[lineNumber] -= 1;
            }

            if (generatorType == "left")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().leftLineWorkload -= 1;
            }
            if (generatorType == "right")
            {
                trafficGenerator.GetComponent<TrafficGenerator>().rightLineWorkload -= 1;
            }
        }
    }
}
