using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Character
{
    Chicken, Dragon, Condor
}

public class Bird_merge : MonoBehaviour
{
    public string Charname;

    public GameObject ChickenCheck;
    public GameObject CondorCheck;
    public GameObject DragonCheck;

    public int currentCharacter;
    public static Bird_merge instance_;

    private void Awake()
    {
        if (instance_ == null) instance_ = this;
        else if (instance_ != null) return;
    }

    void Start()
    {
        ChickenCheck.SetActive(false);
        CondorCheck.SetActive(false);
        DragonCheck.SetActive(false);

        Charname = "Chicken";
        OnSelect(Charname);
    }

    public void OnSelect(string charname)
    {
        Charname = charname;
        ChickenCheck.SetActive(false);
        CondorCheck.SetActive(false);
        DragonCheck.SetActive(false);
        //화살표 표시
        if (Charname.Equals("Chicken"))
        {
            ChickenCheck.SetActive(true);
            currentCharacter = 0;
            Bird_DataManager.instance_.currentCharacter = Character.Chicken;
        }
        else if (Charname.Equals("Condor"))
        {
            CondorCheck.SetActive(true);
            currentCharacter = 1;
            Bird_DataManager.instance_.currentCharacter = Character.Condor;
        }
        else
        {
            DragonCheck.SetActive(true);
            currentCharacter = 2;
            Bird_DataManager.instance_.currentCharacter = Character.Dragon;
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Bird_GameScene");
    }
}
