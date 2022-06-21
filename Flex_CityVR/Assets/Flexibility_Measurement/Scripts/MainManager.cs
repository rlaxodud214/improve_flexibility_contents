using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public User user;

    public Text nameText;
    public Text ageText;
    public Text genderText;

    private void Awake()
    {
        user = UserDataManager.instance.user;
    }

    void Start()
    {      
        nameText.text = "이름: " + user.name;
        ageText.text = "나이: " + user.age.ToString();
        genderText.text = "성별: " + (user.gender ? "남성" : "여성");    
    }

/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("3-3.RecentResult");
        }
    }*/
}
