using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flex_SceneChangeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginScene()
    {
        SceneManager.LoadScene("1.Login");
    }

    public void RegisterScene()
    {
        SceneManager.LoadScene("2.Register");
    }

    public void MainScene()
    {
        SceneManager.LoadScene("3-0.Main");
    }

    public void MeasurementScene()
    {
        SceneManager.LoadScene("3-1.Flexibility_Measurement");
    }

    public void RecentResultScene()
    {
        SceneManager.LoadScene("3-2.RecentResult");
    }

    public void ResultScene()
    {
        SceneManager.LoadScene("3-3.Result");
    }

    public void content1()
    {
        // Application.dataPath : 빌드 경로 사용하기
        System.Diagnostics.Process.Start(Application.dataPath + "/Demo/Limbo/85/Designteam_Game.exe");
    }

    public void content2()
    {
        // Application.dataPath : 빌드 경로 사용하기
        System.Diagnostics.Process.Start(Application.dataPath + "/Demo/Goal_demo/85/Designteam_Game.exe");
    }

    public void content3()
    {
        // Application.dataPath : 빌드 경로 사용하기
        System.Diagnostics.Process.Start(Application.dataPath + "/Demo/Kayak_demo/85/Designteam_Game.exe");
    }

    public void Exit()
    {
        //Application.Quit();
        SceneManager.LoadScene("MainCity");
    }
}
