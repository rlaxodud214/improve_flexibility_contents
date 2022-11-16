using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GKSceneManager : MonoBehaviour
{
    public GameObject pausePanel;
    public void Home()
    {
        SceneManager.LoadScene("mainCity");
        Time.timeScale = 1f;
    }


    public void Replay()
    {
        SceneManager.LoadScene("GoalKeeper");
        Time.timeScale = 1f;
    }
}
