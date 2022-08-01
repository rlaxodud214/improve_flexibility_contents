using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chef_SceneChange: MonoBehaviour
{
    public void GameChange()
    {
        SceneManager.LoadScene("Chef_Game");     // 화면전환
    }
    public void TutorialChange()
    {
        SceneManager.LoadScene("Chef_Tutorial");
    }
    public void Quit()
    {
        SceneManager.LoadScene("mainCity");
    }


}
