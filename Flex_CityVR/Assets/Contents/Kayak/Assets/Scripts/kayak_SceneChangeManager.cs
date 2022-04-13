using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class kayak_SceneChangeManager : MonoBehaviour
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

    public void Exit()
    {
        Application.Quit();
    }
}
