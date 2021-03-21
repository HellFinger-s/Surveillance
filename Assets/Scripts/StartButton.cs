using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StartButton : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject obj;
    void Start()
    {
        Time.timeScale = 0;
    }

    public void Pressed()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
    }
}
