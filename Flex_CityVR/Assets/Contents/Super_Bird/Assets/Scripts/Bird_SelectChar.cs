using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_SelectChar : MonoBehaviour
{
    public Character character;
    public string Charname;

    public GameObject ChickenCheck;
    public GameObject CondorCheck;
    public GameObject DragonCheck;
    void Start()
    {
        ChickenCheck.SetActive(false);
        CondorCheck.SetActive(false);
        DragonCheck.SetActive(false);

        Charname = "Chicken";
        OnSelect();
    }

    private void OnMouseUpAsButton()
    {
        Bird_DataManager.instance_.currentCharacter = character;
        print(character);
        Charname = character.ToString();
        OnSelect();
    }


    void OnSelect()
    {
       //화살표 표시
        if(Charname.Equals("Chicken"))
        {
            ChickenCheck.SetActive(true);
            CondorCheck.SetActive(false);
            DragonCheck.SetActive(false);
        }
        else if (Charname.Equals("Condor"))
        {
            ChickenCheck.SetActive(false);
            CondorCheck.SetActive(true);
            DragonCheck.SetActive(false);
        }
        else
        {
            ChickenCheck.SetActive(false);
            CondorCheck.SetActive(false);
            DragonCheck.SetActive(true);
        }
    }
}
