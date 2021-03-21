using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testLoadLvl : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 0;
    }
    public void Pressed()
    {
        Debug.Log("press");
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(0);
    }
}
