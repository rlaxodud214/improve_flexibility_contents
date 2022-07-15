using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*public enum Character
{
    Chicken,Dragon,Condor
}*/

public class Bird_DataManager : MonoBehaviour
{
    public static Bird_DataManager instance_;

    private void Awake()
    {
        if (instance_ == null) instance_ = this;
        else if (instance_ != null) return;
        //DontDestroyOnLoad(gameObject);
    }

    public Character currentCharacter;

   public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

}
