using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird_UIManager : MonoBehaviour
{
    public GameObject startPanel;

    void Start() 
    {
        Time.timeScale = 1f;
    }
    void Update(){    }

    public void GameStart()
    {    
        SceneManager.LoadScene("Bird_SelectScene");
    }

    public void Quit()
    {
        SceneManager.LoadScene("mainCity");
    }

    public void Tutorial()
    {    
        SceneManager.LoadScene("Bird_TutorialScnen");
    }
}

