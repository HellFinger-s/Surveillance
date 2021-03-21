using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterControl : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public GameObject sceneController;
    public GameObject character;
    public GameObject[] characterPositions = new GameObject[0];//объекты, между которыми движется игрок | objects between which the player moves horizontally
    public GameObject target;
    public List<GameObject> notAvaiableSegments = new List<GameObject> { };//неактивные сегменты дороги | not active road segments
    public GameObject currentSegment;
    public GameObject backArrow;
    public GameObject frontArrow;

    public int horizontalLineNumber;//номер текущей полосы | number of current line

    public string speedCondition = "acceleration";//тип движения | move type

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    bool isTouch;
    bool isSwipe;
    bool startTimer;
    private bool horizontalMoveCondition = false;
    public bool targetInvis;//машина цель сзади, и нужно включить заднюю стрелку | target car is invisible, need to enable direction arrow


    public float speedNow;
    public float accelerationSpeed;
    public float slowBrakingSpeed;
    public float lineChangeSpeed;
    public float timer;

    public System.Random random = new System.Random();


    Rigidbody characterRb;

    void Start()
    {
        characterRb = character.GetComponent<Rigidbody>();
        speedCondition = "acceleration";
    }


    public void FixedUpdate()
    {
        characterRb.velocity = new Vector3(-speedNow, 0, 0);

        if (horizontalMoveCondition)
        {
            //ниже - строчка, которая перемещает игрока к определенной линии при свайпе | move player to new line if swipe
            character.transform.position = new Vector3(
                character.transform.position.x,
                character.transform.position.y,
                Mathf.Lerp(character.transform.position.z, characterPositions[horizontalLineNumber].transform.position.z, lineChangeSpeed * Time.deltaTime * sceneController.GetComponent<SceneController>().time)
                );
        }
        if (Math.Round(character.transform.position.z, 2) == Math.Round(characterPositions[horizontalLineNumber].transform.position.z, 2))//переместился ли игрок на позицию | has the player moved to the position
        {
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, characterPositions[horizontalLineNumber].transform.position.z);
            horizontalMoveCondition = false;
        }
    }



    void Update()
    {
        if (targetInvis)
        {
            if(character.transform.position.x > target.transform.position.x && Mathf.Abs(character.transform.position.x - target.transform.position.x) > 60 && frontArrow.activeInHierarchy == false)
            {
                frontArrow.SetActive(true);
                backArrow.SetActive(false);
                
            }
            if(character.transform.position.x < target.transform.position.x && Mathf.Abs(character.transform.position.x - target.transform.position.x) > 30 && backArrow.activeInHierarchy == false)
            {
                backArrow.SetActive(true);
                frontArrow.SetActive(false);
            }
        }

        if(speedCondition == "acceleration")
        {
            speedNow += accelerationSpeed * Time.deltaTime;//ускоряем игрока | speed up
        }
        if(speedCondition == "slowBraking" && speedNow > 0)
        {
            if(speedNow < 1)
            {
                speedNow = 0;
            }
            else
            {
                speedNow -= slowBrakingSpeed * Time.deltaTime;
            }
        }
        if (speedCondition == "fastBraking")
        {
            if(speedNow < 1)
            {
                speedNow = 0;
            }
            else
            {
                speedNow -= speedNow * 0.25f;
            }
            speedCondition = "acceleration";
        }

        if(timer > 0.5f)//игрок производит долгое нажатие | long touch
        {
            speedCondition = "slowBraking";
        }

        if (startTimer && !horizontalMoveCondition)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {

            startTouchPos = Input.mousePosition;
            startTimer = true;//запустили отсчет времени, которое палец провел на экране | started the countdown of the time that the finger spent on the screen
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            startTimer = false;
            speedCondition = "acceleration";
            timer = 0;
        }

        if (Vector3.Distance(startTouchPos, endTouchPos) < 0.1f & startTouchPos.x != -1f)
        {
            isTouch = true;
            isSwipe = false;
        }
        if (Vector3.Distance(startTouchPos, endTouchPos) > 3f)
        {
            isTouch = false;
            isSwipe = true;
        }
        if (isTouch)
        {
            if(timer < 1)//игрок тапнул на экран | tap
            {
                speedCondition = "fastBraking";
            }
            timer = 0;
            startTouchPos.x = -1f;
            isTouch = false;
        }
        if (isSwipe)
        {
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((Mathf.Abs(eventData.delta.x)) > (Mathf.Abs(eventData.delta.y)))//произошел горизонтальный свайп | horizontal swipe
        {
            if (eventData.delta.x > 0)
            {
                if (horizontalLineNumber < (characterPositions.Length - 1))
                {
                    Right();//перемещаем вправо | move right
                }

            }
            if (eventData.delta.x < 0)
            {
                if (horizontalLineNumber > 0)
                {
                    Left();//перемещаем влево | move left
                }

            }
        }
        else
        {
        }
    }
    public void OnDrag(PointerEventData eventData)
    {

    }




    public void Right()
    {
        horizontalLineNumber += 1;
        horizontalMoveCondition = true;
    }

    public void Left()
    {
        horizontalLineNumber -= 1;
        horizontalMoveCondition = true;
    }
}
