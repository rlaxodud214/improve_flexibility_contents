using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //private float powerUI = Fire.instance.speed;
    public Text lifeText;
    public Text levelText;
    public Text EnemyLifeText;
    //public Text powerText;
    public int life;
    public int EnemyLife;
    public Slider hpbar;
    public Slider EnemyHpBar;
    public GameObject Active;       //포탄 쐈을때 비활성화 패널
    public GameObject Victory;      
    public GameObject Fail;
    public static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        
    }

    void Start()
    {
        // check = false;
        life = 100;
        EnemyLife = 100;
        Victory.SetActive(false);
        Fail.SetActive(false);
    }

    void Update()
    {   
        hpbar.value = life;
        EnemyHpBar.value = EnemyLife;
        lifeText.text = "HP : " + life.ToString();
        levelText.text = Enemy.instance.level.ToString() + ".Level";
        EnemyLifeText.text = EnemyLife.ToString();
    }
}
