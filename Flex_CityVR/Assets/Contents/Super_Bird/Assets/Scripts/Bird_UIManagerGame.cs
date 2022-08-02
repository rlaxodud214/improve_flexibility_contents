using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bird_UIManagerGame : MonoBehaviour
{
    public GameObject playPanel, pausePanel, endPanel, timePanel, pauseBtn;
    public Text gamePlayTime;
    public Text Cleartime_Text;
    public Text RewardText; //리워드

    public float playtime;
    public bool timerStart = false;
    public int reward=0;


    public List<Image> StarImages;
    Color STARON, STAROFF;

    #region instance
    public static Bird_UIManagerGame _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() 
    {
        Time.timeScale = 0f;

        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        pauseBtn.SetActive(false);
        /*YSlider.SetActive(true);*/
        playPanel.SetActive(true);
        timePanel.SetActive(false);

        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            playtime += Time.deltaTime;
            if ((int)playtime < 60)
            {
                gamePlayTime.text = "Play Time  " + playtime.ToString("N2"); // 소주점 지정 N
            }
            else
            {
                gamePlayTime.text = "Play Time  " + ((int)playtime / 60) + ":"+ ((int)playtime % 60);

            }
            
        }
    }

    public void Play()
    {
        Time.timeScale = 1f;
        playtime = 0f;
        timerStart = true;
        pauseBtn.SetActive(true);;
        playPanel.SetActive(false);
        timePanel.SetActive(true);
        OpenZenMoveObject.Instance.runstart();
    }

    public void Pause()
    {
        Bird_SoundManager.instance.EffectOn();
        Time.timeScale = 0f;
        timerStart = false;
        pausePanel.SetActive(true);
    }

    public void PausePlay()
    {
        Bird_SoundManager.instance.EffectOn();
        Time.timeScale = 1f;
        timerStart = true;
        pausePanel.SetActive(false);
    }

    public void PauseRestart()
    {
        Bird_SoundManager.instance.EffectOn();
        SceneManager.LoadScene("Bird_GameScene");
    }

    public void PauseMainSecen()
    {
        Bird_SoundManager.instance.EffectOn();
        SceneManager.LoadScene("Bird_MainScene");     
    }

    public void GameOver()
    {
        if ((int)playtime < 60)
        {
            Cleartime_Text.text = playtime.ToString("N2"); // 소주점 지정 N
        }
        else
        {
            Cleartime_Text.text = ((int)playtime / 60) + ":" + ((int)playtime % 60);

        }
        endPanel.SetActive(true);
        Time.timeScale = 0f;
        timerStart = false;
        pauseBtn.SetActive(false);
        //YSlider.SetActive(false);
        gamePlayTime.text = "";

        reward = 100 + ((int)playtime - 30) * ((500 - 100) / (90 - 30));
        print("reward : "+reward);
        if (reward < 100)
            reward = 0;
        if (reward >= 100)
            StarImages[0].color = STARON;
        if (reward >= 234)
            StarImages[1].color = STARON;
        if (reward >= 367)
            StarImages[2].color = STARON;

        if (reward > 500)
            reward = 500;

        RewardText.text = "" + reward;


       /* string resultPlaytime = DBManager.TimeToString((int)playtime);  // 플레이타임을 DB에 넣을 수 있는 형태(string)으로 변경
                                                                        // 게임결과 정보를 담은 gameResult 객체를 만듦
                                                                        // 매개변수 1: 게임이름, 2: 플레이타임, 3: 리워드
        GameResult gameResult = DBManager.CreateGameResult("Goalkeeper", resultPlaytime, reward);
        StartCoroutine(DBManager.SaveGameResult(gameResult));   // DB저장 시도*/

    }

    
}
