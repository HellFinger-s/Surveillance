using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficGenerator : MonoBehaviour
{
    public int[] backLineWorkload = new int[5] { 0, 0, 0, 0, 0 };//наличие машин в местах спауна новых | availability of cars in the spawn areas of new ones
    public int[] frontLineWorkload = new int[5] { 0, 0, 0, 0, 0 };

    [Space(10)]
    public GameObject[] spawnPositions = new GameObject[0];

    [Space(10)]
    public List<GameObject> availableCars = new List<GameObject>();//машины, которыми мы можем переставлять на новое место
                                                                   //machines that we can move to a new location

    public List<GameObject> unAvailableCars = new List<GameObject>();//машины, которые участвуют в движении, их мы трогать не можем
                                                                     //cars that are involved in the movement, we can not touch them

    [Space(10)]
    public GameObject controlPanel;
    private GameObject character;
    public GameObject backGroup;//пул машин для генерации сзади | pool of machines to generate data
    public GameObject frontCoDirectionalGroup;//пул машин для генерации спереди на сонаправленных полосах 
                                              //pool of machines for generating traffic on co-directional lanes

    public GameObject frontOnComingGroup;//пул машин для генерации спереди на встречных полосах
                                         //pool of cars to generate traffic in front of oncoming lanes

    public GameObject leftGroup;//пул машин для генерации на перекрестке с направлением движения слева направо
                                //pool of cars to generate at the intersection with the direction of traffic from left to right

    public GameObject rightGroup;//пул машин для генерации на перекрестке с направлением движения справа налево
                                 //pool of cars to generate at the intersection with the direction of traffic from right to left

    public GameObject leftCrossroadInstPlace;//место для генерации машин на перекрестке с направление слева направо
                                             //place to generate cars at the intersection with direction from left to right

    public GameObject rightCrossRoadInstPlace;//место для генерации машин на перекрестке с направлением справа налево
                                              //a place to generate cars at an intersection with a right-to-left direction


    public GameObject secondaryCarAccident;//объект - столкновение двух машин | car accident

    [Space(10)]
    public float offsetX;//смещение для генератора | generator ofsset
    private float spawnTimer;
    public float spawnDelay;//интервал времени между генерацией машин
    public float secondaryAccidentTime;//время, через сколько сгенерируется столкновение вторичных машин
                                       //the time after which a secondary car collision will be generated

    private float secondaryAccidentTimer;

    [Space(10)]
    public bool isCrossSpawn;

    [Space(10)]
    public string generatorType;//тип траффик генератора

    public System.Random random = new System.Random();

    private int backLineNumber;
    private int frontLineNumberCoDirectional;
    private int frontLineNumberOnComing;
    private int carAccidentLineNumber = 0;
    public int leftLineWorkload;//наличие машин в месте спауна на перекрестке направление слева направо
    public int rightLineWorkload;//наличие машин в месте спауна на перекрестке направление справа налево


    public void Start()
    {
        if(controlPanel != null)
        {
            character = controlPanel.GetComponent<CharacterControl>().character;
        }
        if (generatorType == "back")
        {
            foreach(Transform child in backGroup.GetComponentInChildren<Transform>())//заполняем список доступных машин | filling in the list of available machines
            {
                availableCars.Add(child.gameObject);
            }
        }
        if(generatorType == "frontCoDirectional")
        {
            foreach (Transform child in frontCoDirectionalGroup.GetComponentInChildren<Transform>())
            {
                availableCars.Add(child.gameObject);
            }
        }
        if (generatorType == "frontOnComing")
        {
            foreach (Transform child in frontOnComingGroup.GetComponentInChildren<Transform>())
            {
                availableCars.Add(child.gameObject);
            }
        }
        if (generatorType == "left")
        {
            foreach (Transform child in leftGroup.GetComponentInChildren<Transform>())
            {
                availableCars.Add(child.gameObject);
            }
        }
        if(generatorType == "right")
        {
            foreach (Transform child in rightGroup.GetComponentInChildren<Transform>())
            {
                availableCars.Add(child.gameObject);
            }
        }
    }

    public void Update()
    {
        if(character != null)
        {
            gameObject.transform.position = new Vector3(character.transform.position.x + offsetX, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        spawnTimer += Time.deltaTime;
        secondaryAccidentTimer += Time.deltaTime;
        if(secondaryAccidentTimer > secondaryAccidentTime)
        {
            if(generatorType == "frontCoDirectional")
            {
                //пока сделана генерация столкнувшихся машин только на сонаправленных полосах, поэтому ниже рандом от 3 до 5
                //so far, the generation of colliding cars has been made only on co-directional lanes, so the rand is from 3 to 5 below
                if (carAccidentLineNumber == 0)
                {
                    carAccidentLineNumber = random.Next(3, 5);
                }
                if (frontLineWorkload[carAccidentLineNumber] == 0)//место спауна свободно | spawn place is free
                {
                    if (carAccidentLineNumber < 2)//это встречные полосы | these are oncoming lanes
                    {
                        secondaryCarAccident.transform.position = new Vector3(gameObject.transform.position.x, 2.1f, spawnPositions[carAccidentLineNumber].transform.position.z);
                        secondaryCarAccident.transform.rotation = Quaternion.Euler(0, 0, 0);
                        secondaryCarAccident.SetActive(true);

                    }
                    if (carAccidentLineNumber > 2)//это сонаправленные полосы | these are co-directional bands
                    {
                        secondaryCarAccident.transform.position = new Vector3(gameObject.transform.position.x, 2.1f, spawnPositions[carAccidentLineNumber].transform.position.z);
                        secondaryCarAccident.transform.rotation = Quaternion.Euler(0, 180, 0);
                        secondaryAccidentTime = 90f;
                        secondaryCarAccident.SetActive(true);
                    }
                    
                }
            }
            
        }
        if (spawnTimer > spawnDelay)
        {
            
            if(generatorType == "back")
            {
                backLineNumber = random.Next(3, 5);
                if (backLineWorkload[backLineNumber] == 0)//на месте спауна нет автомобиля | there is no car at spawn place
                {
                    availableCars[0].transform.position = new Vector3(gameObject.transform.position.x, 2.1f, spawnPositions[backLineNumber].transform.position.z);
                    availableCars[0].GetComponent<SecondaryMachineControl>().horizontalLineNumber = backLineNumber;//задаем номер полосы автомобиля | set car line number
                    availableCars[0].GetComponent<SecondaryMachineControl>().moving = true;

                    availableCars[0].transform.GetChild(0).GetChild(0).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("leftSide");//см SecondaryCarSideLinesCollision | see SecondaryCarSideLinesCollision
                    availableCars[0].transform.GetChild(0).GetChild(1).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("rightSide");
                    
                    availableCars[0].GetComponent<SecondaryMachineControl>().xDirection = -1;
                    availableCars[0].SetActive(true);
                    unAvailableCars.Add(availableCars[0]);
                    availableCars.RemoveAt(0);
                }
            }
            
            if(generatorType == "frontCoDirectional")
            {
                frontLineNumberCoDirectional = random.Next(3, 5);
                if (frontLineWorkload[frontLineNumberCoDirectional] == 0)
                {
                    availableCars[0].transform.position = new Vector3(gameObject.transform.position.x, 2.1f, spawnPositions[frontLineNumberCoDirectional].transform.position.z);
                    availableCars[0].GetComponent<SecondaryMachineControl>().horizontalLineNumber = frontLineNumberCoDirectional;
                    availableCars[0].GetComponent<SecondaryMachineControl>().moving = true;
                    availableCars[0].transform.GetChild(0).GetChild(0).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("leftSide");
                    availableCars[0].transform.GetChild(0).GetChild(1).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("rightSide");
                    availableCars[0].SetActive(true);
                    unAvailableCars.Add(availableCars[0]);
                    availableCars.RemoveAt(0);

                }
            }

            if (generatorType == "frontOnComing")
            {
                frontLineNumberOnComing = random.Next(0, 2);
                if (frontLineWorkload[frontLineNumberOnComing] == 0)
                {
                    availableCars[0].transform.position = new Vector3(gameObject.transform.position.x, 2.1f, spawnPositions[frontLineNumberOnComing].transform.position.z);
                    availableCars[0].GetComponent<SecondaryMachineControl>().horizontalLineNumber = frontLineNumberOnComing;
                    availableCars[0].GetComponent<SecondaryMachineControl>().moving = true;
                    availableCars[0].transform.GetChild(0).GetChild(0).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("rightSide");
                    availableCars[0].transform.GetChild(0).GetChild(1).GetComponent<SecondaryCarSideLinesCollision>().LineNumberDetermining("leftSide");
                    availableCars[0].SetActive(true);
                    unAvailableCars.Add(availableCars[0]);
                    availableCars.RemoveAt(0);
                }
            }

            if (generatorType == "left" && isCrossSpawn)
            {
                if(leftLineWorkload == 0)
                {
                    availableCars[0].transform.position = new Vector3(leftCrossroadInstPlace.transform.position.x, 2.1f, leftCrossroadInstPlace.transform.position.z);
                    leftLineWorkload += 1;
                    availableCars[0].SetActive(true);
                    unAvailableCars.Add(availableCars[0]);
                    availableCars.RemoveAt(0);
                }
            }

            if (generatorType == "right" && isCrossSpawn)
            {
                if(rightLineWorkload == 0)
                {
                    availableCars[0].transform.position = new Vector3(rightCrossRoadInstPlace.transform.position.x, 2.1f, rightCrossRoadInstPlace.transform.position.z);
                    rightLineWorkload += 1;
                    availableCars[0].SetActive(true);
                    unAvailableCars.Add(availableCars[0]);
                    availableCars.RemoveAt(0);
                }
            }

            spawnTimer = 0;
        }
    }
}
