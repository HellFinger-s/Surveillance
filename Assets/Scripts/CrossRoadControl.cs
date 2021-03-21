using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRoadControl : MonoBehaviour
{
    public GameObject onCommingTrigger;
    public GameObject coDirectionTrigger;
    public GameObject fromLeftTrigger;
    public GameObject fromRightTrigger;
    public GameObject characterControlPanel;
    private GameObject character;
    public GameObject leftCrossSpawn;
    public GameObject rightCrossSpawn;

    public List<GameObject> stoppedCars = new List<GameObject>() { };
    public List<string> stoppedCarsTags = new List<string>() { };

    public float activateDist;//дистанция активации перекрестка | distance of crossroad activation
    public float timer;
    public float greenLightTime;//время проезда нашей и паралелльной полос
    public float redLightTime;//время проезда перпендикулярных полос

    public int perpendicularCarsCount;

    public bool active;

    public string state = "stop";

    void Start()
    {
        character = characterControlPanel.GetComponent<CharacterControl>().character;
    }

    void Update()
    {
        if(character.transform.position.x < gameObject.transform.position.x)//игрок уехал от перекрестка | player is far away from crossroad
        {
            active = false;
            //ниже - выключение перпендикулярной генерации машин на перекрестке | disable perpendicular cars generation on crossroad
            leftCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = false;
            rightCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = false;
        }
        if(gameObject.transform.position.x - character.transform.position.x >= activateDist && gameObject.transform.position.z != 150.2f)//игрок подъехал к перекрестку | player drove to crossroad
        {
            active = true;
            //ниже - включение перпендикулярной генерации машин на перекрестке | enable perpendicular cars generation on crossroad
            leftCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = true;
            rightCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = true;
        }
        if (active)//перекресток активен | crossroad is active
        {
            timer += Time.deltaTime;
            if (timer > redLightTime && state == "stop")//переход к фазе движения нашей и параллельных полос | transition to the phase of movement of our and parallel lines
            {
                leftCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = false;
                rightCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = false;
                if(perpendicularCarsCount == 0)//все машины, двигающиеся перпендикулярно уехали с перекрестка | all cars moving perpendicular left the crossroad
                {
                    state = "go";
                    timer = 0;
                    onCommingTrigger.SetActive(false);//разрешаем движение по встречной полосе | allow traffic in the oncoming
                    coDirectionTrigger.SetActive(false);//разрешаем движенмие по нашей полосе | allow traffic in our lane
                    fromLeftTrigger.SetActive(true);//запрещаем движение по перпернидкулярным полосам | prohibit traffic in the perpendicular lines
                    fromRightTrigger.SetActive(true);//запрещаем движение по перпендикулярным полосам | -//-
                    for (int i = 0; i < stoppedCars.Count; i++)//включаем возможность движения у машин с параллельных полос | enabling the ability to move cars from parallel lines
                    {
                        if (stoppedCarsTags[i] == "target")
                        {
                            stoppedCars[i].GetComponent<targetControl>().moving = true;
                        }
                        else
                        {
                            stoppedCars[i].GetComponent<SecondaryMachineControl>().moving = true;
                        }
                    }
                    stoppedCars.Clear();
                    stoppedCarsTags.Clear();
                }
                
            }
            if (timer > greenLightTime && state == "go" && perpendicularCarsCount == 0)//переход к фазе движения перпендикулярных полос | transition to the phase of movement perpendicular lines
            {
                leftCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = true;
                rightCrossSpawn.GetComponent<TrafficGenerator>().isCrossSpawn = true;
                state = "stop";
                timer = 0;
                onCommingTrigger.SetActive(true);//запрещаем движение по встречной полосе | prohibit traffic on the oncoming
                coDirectionTrigger.SetActive(true);//зарпещаем движение по нашей полосе | prohibit traffic on our line
                fromLeftTrigger.SetActive(false);//разрешаем движение по перпендикулярным полосам | alow traffic on perpendicular lines
                fromRightTrigger.SetActive(false);//разрешаем движение по перпендикулярным полосам | -//-
            }
        }

    }
}
