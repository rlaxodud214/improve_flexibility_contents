using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GKSceneManager : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("mainCity");
    }


    public void Replay()
    {
        SceneManager.LoadScene("GoalKeeper");
    }
}
