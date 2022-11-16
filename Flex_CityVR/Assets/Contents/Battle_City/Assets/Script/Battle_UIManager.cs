using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Battle_UIManager : MonoBehaviour
{
    #region Public Fields

    public static Battle_UIManager instance;

    public Text LevelText;
    public Text LifeText;
    public Text PlaytimeText;
    public Text EnemyLifeText;
    //public Slider HpBar;
    public UnityEngine.UI.Slider EnemyHpBar;
    //public GameObject ActivePanel;       //포탄 쐈을때 비활성화 패널
    public GameObject HitPanel;          
    public GameObject FailPanel;
    public GameObject SuccessPanel;
    public GameObject PausePanel;
    public GameObject RewardPanel;
    public GameObject ExplainPanel;
    public List<Image> StarImages;  // 인스펙터 창에서 별 이미지 오브젝트 넣기
    public Text RewardText;
    public Text TotalPlaytimeText;

    public int level;
    public float enemyLife;  // 적 체력
    public float playerLife; // 플레이어 체력
    public float playtime;
    public string min, sec;
    public bool isTimerActive;

    int maxReward, minReward;
    Color STARON, STAROFF;  // 별 색깔을 담을 Color 오브젝트
    #endregion

    #region Private Fields

    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
    }

    void Start()
    {
        ExplainPanel.SetActive(true);
        level = 1;
        LevelText.text = "Level : " + level.ToString();
        playerLife = 100f;
        LifeText.text = "Life : " + playerLife.ToString();
        playtime = 0f;
        PlaytimeText.text = "Playtime : " + "00:00";
        isTimerActive = false;
        RewardText.text = "0";
        maxReward = 500;
        minReward = 100;

        InitEnemyLife();
        InitPanels();
    }

    void Update()
    {
        if (InputBridge.Instance.BButtonDown)
        {           
            PausePanel.SetActive(true);
            Battle_Fire.instance.fireActive = false;
            Time.timeScale = 0f;
        }

        if (isTimerActive)
        {
            playtime += Time.deltaTime;
            min = ((int)playtime / 60).ToString("D2");
            sec = ((int)playtime % 60).ToString("D2");
            PlaytimeText.text = string.Format("Playtime : {0}:{1}", min, sec);
        }
    }

    #endregion

    #region Public Methods

    public void NextLevel()
    {
        level++;
        LevelText.text = "Level : " + level.ToString();
    }

    /// <summary>
    /// 적과 플레이어의 생명 초기화
    /// 플레이어 생명은 100으로 고정, 적의 생명은 기본값 100
    /// </summary>
    public void InitEnemyLife()
    {
        EnemyHpBar = GameObject.FindWithTag("EnemySlider").GetComponent<UnityEngine.UI.Slider>();
        enemyLife = 100f;
        EnemyLifeText.text = enemyLife.ToString();
        EnemyHpBar.value = enemyLife;
        //HpBar.value = playerLife;
    }

    /// <summary>
    /// 적 또는 플레이어의 생명을 감소시키는 메소드
    /// </summary>
    /// <param name="target">누구의 생명을 감소시킬지</param> 
    /// <param name="decrement">감소치</param> 
    public void DecreaseLife(string target, float decrement)
    {
        // 적 체력 하락
        if (target.Equals("enemy"))
        {
            enemyLife -= decrement;
            if (enemyLife < 0)
                enemyLife = 0f;
            EnemyLifeText.text = enemyLife.ToString();
            EnemyHpBar.value = enemyLife;

        }
        // 플레이어 체력 하락
        else
        {
            playerLife -= decrement;
            if (playerLife <= 0)
            {
                playerLife = 0;
                GameOver();
            }
            else
            {
                LifeText.text = "Life: " + playerLife.ToString();
            }
            //HpBar.value = playerLife;
        }
    }

    public void InitPanels()
    {
        HitPanel.SetActive(false);
        FailPanel.SetActive(false);
        SuccessPanel.SetActive(false);
        PausePanel.SetActive(false);
        RewardPanel.SetActive(false);

        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }
    }

    public void IsVictory(string state)
    {
        StartCoroutine(ShowResult(state));
    }

    public void GameStart()
    {
        isTimerActive = true;
        Battle_Fire.instance.fireActive = true;
        Time.timeScale = 1f;
    }

    public void Home()
    {
        SceneManager.LoadScene("mainCity");
    }

    public void Retry()
    {
        SceneManager.LoadScene("BattleCity");
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Battle_Fire.instance.fireActive = true;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Battle_Fire.instance.fireActive = false;

        int reward = minReward + (level - 3) * ((maxReward - minReward) / (5 - 3));      
        if (reward >= 100)
            StarImages[0].color = STARON;
        if (reward >= 234)
            StarImages[1].color = STARON;
        if (reward >= 367)
            StarImages[2].color = STARON;
        if (reward < 0)
            reward = 0;
        RewardText.text = reward.ToString();
        TotalPlaytimeText.text = string.Format("{0}:{1}", min, sec);

        RewardPanel.SetActive(true);
    }

    #endregion

    #region Coroutines

    public IEnumerator ShowResult(string state)
    {
        if (state.Equals("Success"))
        {
            Battle_Fire.instance.fireActive = false;
            yield return new WaitForSeconds(1f);        
            Enemy.instance.refreshPos = true;   // 목표 격추 시에만 적의 위치 변경
            SuccessPanel.SetActive(true);
            NextLevel();
            yield return new WaitForSeconds(4.5f);
            Battle_EnemySpawner.instance.InstantiateEnemy();
            InitEnemyLife();            
            Battle_Fire.instance.fireActive = true;
        }
        else if (state.Equals("Hit"))
        {
            Battle_Fire.instance.fireActive = false;
            yield return new WaitForSeconds(1f);
            Enemy.instance.refreshPos = true;
            HitPanel.SetActive(true);
            yield return new WaitForSeconds(4.5f);
            Battle_Fire.instance.fireActive = true;
        }
        else if (state.Equals("Fail"))
        {
            Battle_Fire.instance.fireActive = false;
            FailPanel.SetActive(true);
            yield return new WaitForSeconds(4.5f);
            Battle_Fire.instance.fireActive = true;
        }
        else
        {
            Debug.LogError("Battle_UIManager.cs: ShowResult의 state값이 올바르지 않습니다.");
        }
    }

    #endregion
}
