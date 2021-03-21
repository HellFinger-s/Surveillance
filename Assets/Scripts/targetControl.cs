using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class targetControl : MonoBehaviour
{
    public float speed;//скорость движения
    public float lineChangeSpeed;//скорость перемещения между линиями
    private float timer;

    public int horizontalLineNumber;

    private Rigidbody targetRb;

    private string speedState = "common";//состояние движения

    public bool moving = true;
    private bool horizontalMoveCondition = false;

    public GameObject frontArrow;
    public GameObject backArrow;
    public GameObject characterControlPanel;
    private GameObject character;
    public GameObject[] positions = new GameObject[0];

    public Vector2 accelerationTimeIntervalBorders;//временной промежуток ускорения
    public Vector2 brakingTimeIntervalBorders;//временной промежуток замедления

    void Start()
    {
        targetRb = gameObject.GetComponent<Rigidbody>();
        character = characterControlPanel.GetComponent<CharacterControl>().character;
    }

    public void FixedUpdate()
    {
        if (moving)//машина движется | target is moving
        {
            targetRb.velocity = new Vector3(-speed, 0, 0);
        }
        else
        {
            targetRb.velocity = new Vector3(0, 0, 0);
        }
        if (horizontalMoveCondition)//перемещение между линиями | changing line
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                Mathf.Lerp(
                    gameObject.transform.position.z,
                    positions[horizontalLineNumber].transform.position.z,
                    lineChangeSpeed * Time.deltaTime
                    )
                );
        }
        if (Math.Round(gameObject.transform.position.z, 2) == Math.Round(positions[horizontalLineNumber].transform.position.z, 2))//переместился ли игрок на позицию | moved to the position
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                positions[horizontalLineNumber].transform.position.z
                );
            horizontalMoveCondition = false;
        }
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if(timer > accelerationTimeIntervalBorders[0] && speedState != "acceleration")//пришло время ускоряться | time to accelerate
        {
            speed *= 1.2f;
            speedState = "acceleration";
        }

        if(timer > accelerationTimeIntervalBorders[1] && speedState == "acceleration")//возвращаемся в спокойное состояние | return to calm state
        {
            speed *= 0.8f;
            speedState = "common";
            accelerationTimeIntervalBorders[0] = 90f;
            accelerationTimeIntervalBorders[1] = 90f;
        }

        if (timer > brakingTimeIntervalBorders[0] && speedState != "braking")//пришло время снижения скорости | time to reduce speed
        {
            speed *= 0.8f;
            speedState = "braking";
        }

        if (timer > brakingTimeIntervalBorders[1] && speedState == "braking")//возвращаемся в спокойное состояние | return to calm state
        {
            speed *= 1.2f;
            speedState = "common";
            brakingTimeIntervalBorders[0] = 90f;
            brakingTimeIntervalBorders[1] = 90f;
        }

    }

    public void OnBecameInvisible()//машина цель за пределами видимости | target vehicle is out of sight
    {
        characterControlPanel.GetComponent<CharacterControl>().targetInvis = true; 
    }

    public void OnBecameVisible()//игрок видит машину | player see target car
    {
        characterControlPanel.GetComponent<CharacterControl>().targetInvis = false;
        frontArrow.SetActive(false);
        backArrow.SetActive(false);
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
