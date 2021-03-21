using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Description))]
public class WinCondition : MonoBehaviour
{
    public GameObject target;
    public GameObject character;
    public GameObject eventPanel;
    public GameObject winText;
    public GameObject uncoveredText;
    public GameObject accidentText;
    public GameObject lostText;

    public Text timerText;
    public Text distanceText;

    public float time = 1;
    public float timer;
    public float timeToWin;//время для победы
    public float timeToCrossroad;//время генерации перекрестка | time to crossroad generation
    public float modifer;//модификатор для перевода дистанции в корпуса, получен опытным путем 
                         //modifier for converting the distance between the player and the target into blocks, obtained experimentally

    public int uncoveredCount = 0;
    public int maxUncoveredCount;//количество столкновений для проигрыша | number of collisions to lose

    void Start()
    {
        character = gameObject.GetComponent<CharacterControl>().character;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeToWin)
        {
            Time.timeScale = 0;
            GameOver("win");
        }
        if(Mathf.Abs(character.transform.position.x - target.transform.position.x) > 90 && Time.timeScale != 0)
        {
            GameOver("lost");
        }
        if(timer > timeToCrossroad)
        {
            character.GetComponent<CharacterCollisionControl>().isCrossRoadTime = true;
            timeToCrossroad = timeToWin * 2;//чтобы больше не зашло в условие ставим время перекрестка больше, чем время сессии
                                            //in order not to enter the condition again, we set the intersection time to be greater than the session time.
        }

        if (uncoveredCount == maxUncoveredCount)
        {
            GameOver("uncovered");
        }

        timerText.text = Mathf.Round(timer).ToString();
        distanceText.text = Mathf.Round(Mathf.Abs(character.transform.position.x - target.transform.position.x)/modifer).ToString();
    }


    public void GameOver(string state)
    {
        Time.timeScale = 0;
        eventPanel.SetActive(true);
        switch (state)
        {
            case "uncovered":
                uncoveredText.SetActive(true);
                break;
            case "win":
                winText.SetActive(true);
                break;
            case "accident":
                accidentText.SetActive(true);
                break;
            case "lost":
                lostText.SetActive(true);
                break;
            default:
                break;
        }
    }
}
