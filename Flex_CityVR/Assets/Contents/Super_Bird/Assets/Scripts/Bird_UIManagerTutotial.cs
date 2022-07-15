using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird_UIManagerTutotial : MonoBehaviour
{
    void Start() { }
    void Update(){ }

    public void start()
    {
        SceneManager.LoadScene("Bird_SelectScene"); 
        //캐릭터 선택창 이동
    }

    public void back()
    {
        SceneManager.LoadScene("Bird_MainScene");
        //메인 창 이동
    }
}
