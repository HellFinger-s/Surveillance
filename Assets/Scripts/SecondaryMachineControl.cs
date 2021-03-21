using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondaryMachineControl : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject parentGenerator;
    private GameObject character;
    private GameObject trigger;

    public bool moving;
    public bool isCrossroadCar;//машина перекрестка(движение - перпендикулярно нашему)
    private bool horizontalMoveCondition = false;

    private string state;

    public float carSpeed;

    public float xDirection;
    public float yDirection;
    public float zDirection;
    public float lineChangeSpeed;//скорость перемещения между линиями

    private int carIndex;


    public int horizontalLineNumber;

    private Rigidbody rb;
    private Rigidbody triggerRb;

    public GameObject[] positions = new GameObject[0];

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        trigger = gameObject.transform.GetChild(0).gameObject;
        triggerRb = trigger.GetComponent<Rigidbody>();
        character = characterPanel.GetComponent<CharacterControl>().character;
    }

    void FixedUpdate()
    {
        if(positions.Length == 0)
        {
            Debug.Log("INSERT POSITIONS");
        }
        if(Math.Abs(character.transform.position.x - gameObject.transform.position.x) > 200)//если машина находится далеко от игрока, она выключается и переиспользуется | if car is far away from player it is disabled and reused
        {
            moving = false;
            carIndex = parentGenerator.GetComponent<TrafficGenerator>().unAvailableCars.IndexOf(gameObject);
            parentGenerator.GetComponent<TrafficGenerator>().unAvailableCars.RemoveAt(carIndex);
            parentGenerator.GetComponent<TrafficGenerator>().availableCars.Add(gameObject);
            gameObject.SetActive(false);
        }

        if (moving)
        {
            rb.velocity = new Vector3(xDirection, yDirection, zDirection) * carSpeed;
            triggerRb.velocity = new Vector3(xDirection, yDirection, zDirection) * carSpeed;
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            triggerRb.velocity = new Vector3(0, 0, 0);
        }

        if (horizontalMoveCondition)//запущено перемещение между полосами | line changing started
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
        if (positions.Length != 0 &&
            Math.Round(gameObject.transform.position.z, 2) == Math.Round(positions[horizontalLineNumber].transform.position.z, 2) &&
            horizontalMoveCondition)//переместился ли объект на позицию | object moved to the position
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                positions[horizontalLineNumber].transform.position.z
                );
            horizontalMoveCondition = false;
        }
    }

    public void Right()
    {
        horizontalLineNumber += 1;
        horizontalMoveCondition = true;
        state = "right";
    }

    public void Left()
    {
        horizontalLineNumber -= 1;
        horizontalMoveCondition = true;
        state = "left";
    }

    public void LineChange(int side)
    {
        horizontalLineNumber += side;
        horizontalMoveCondition = true;
    }


}
