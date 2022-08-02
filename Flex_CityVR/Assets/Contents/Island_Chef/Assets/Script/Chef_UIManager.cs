using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Chef_UIManager : MonoBehaviour
{
    public GameObject pausePanel, successPanel, failPanel, iteminventory, CompleteFood, UIPanel, interactPanel, returnPanel, stagePanel, ReWard, warningPanel, StartPanel;
    public bool timerStart;
    public Text Playtime_Text;
    public float playtime;
    public Text StageCount;
    public Text ExplainFood;
    public Image FoodImage;
    public Image CompleteImage;
    public string FoodExplain;
    public float Reward = 0;
    public Text RewardText;
    public List<Image> StarImages;  // 인스펙터 창에서 별 이미지 오브젝트 넣기
    Color STARON, STAROFF; // 별 색깔을 담을 Color 오브젝트


    #region Singleton 
    private static Chef_UIManager instance = null;
    public static Chef_UIManager _Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return instance; }                     // Chef_SoundManager 객체 리턴
    }
    private void Awake()  //싱글톤 생성
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
            /*SceneManager.sceneLoaded += OnSceneLoaded;*/
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        playtime = 0f;  // 플레이타임 초기화
        timerStart = false; //true
        pausePanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
        Time.timeScale = 1f;
        iteminventory.SetActive(true);
        CompleteFood.SetActive(true);
        UIPanel.SetActive(true);
        interactPanel.SetActive(false);
        stagePanel.SetActive(true);
        ReWard.SetActive(false);
        warningPanel.SetActive(false);
        Invoke("StagePanelOff", 3f);
        //StartCoroutine(returnDelay());
        //stagePanel.SetActive(false);
        print(Chef_StageManagement._Instance.stageNum);
        setExplain();
        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }

    }
    void Update()
    {
        // timerStart가 true -> 타이머 작동
        if (timerStart)
        {
            playtime += Time.deltaTime;
        }
    }

    // 게임 시작 화면
    /*public void GameStart()
    {
        Time.timeScale = 1f;
        //startPanel.SetActive(false);
        iteminventory.SetActive(true);
        CompleteFood.SetActive(true);
        UIPanel.SetActive(true);
        //startPanel.SetActive(false);    // Panel 끄기
    }*/

    public void PlayStart() // 게임 첫 시작시
    {
        Time.timeScale = 1f;
        timerStart = true;
        StartPanel.SetActive(false);
    }

    public void Pause() // 게임창에서 일시정지 버튼을 눌렀을때
    {
        Time.timeScale = 0f;
        timerStart = false;
        pausePanel.SetActive(true);
        iteminventory.SetActive(false);
        UIPanel.SetActive(false);
        CompleteFood.SetActive(false);
    }

    // 게임 재개
    public void Play() // 계속하기 버튼을 눌렀을때
    {
        Time.timeScale = 1f;
        timerStart = true;
        pausePanel.SetActive(false);
        iteminventory.SetActive(true);
        CompleteFood.SetActive(true);
        UIPanel.SetActive(true);
    }

    // 게임 결과 화면
    public void GameSuccess()
    {
        Time.timeScale = 0f;
        timerStart = false; // 타이머 종료
        if (Chef_StageManagement._Instance.stageNum < Chef_ReturnFood._Instance.food.Count - 1)
        {
            Chef_StageManagement._Instance.stageNum++;
            successPanel.SetActive(true);
        }
        else
        {
            ReWard.SetActive(true);
        }
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
    }

    public void GameFail()
    {
        Time.timeScale = 0f;
        Chef_StageManagement._Instance.stageNum++;
        timerStart = false; // 타이머 종료
        failPanel.SetActive(true);
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
    }

    // 게임 재시작
    public void Restart()
    {
        Chef_StageManagement._Instance.stageNum--;
        SceneManager.LoadScene("Chef_Game");
    }
    public void NextStage() // 음식의 숫자보다 적을 경우에는 다음 게임씬을 불러오고 아니면 End씬 호출
    {
        if (Chef_StageManagement._Instance.stageNum < Chef_ReturnFood._Instance.food.Count-1) 
        {
            //Chef_StageManagement._Instance.stageNum++;
            SceneManager.LoadScene("Chef_Game");
        }
    }

    public void activeinteraction()
    {
        interactPanel.SetActive(true);
    }

    public void inactiveinteraction()
    {
        interactPanel.SetActive(false);
    }
    public void activewarning()
    {
        warningPanel.SetActive(true);
    }

    public void inactivewarning()
    {
        warningPanel.SetActive(false);
    }

    public void activereturn()
    {
        returnPanel.SetActive(true);
    }

    public void inactivereturn()
    {
        returnPanel.SetActive(false);
    }
    public void StagePanelOff()
    {
        stagePanel.SetActive(false);
    }
    public void setReward()
    {
        ReWard.SetActive(true);
        Time.timeScale = 0f;    // 게임정지
        Reward = 100 + (Chef_StageManagement._Instance.stageNum - 1) * 100;  
        // 리워드 범위 별로 별 추가
        if (Reward >= 100)
            StarImages[0].color = STARON;
        if (Reward >= 234)
            StarImages[1].color = STARON;
        if (Reward >= 367)
            StarImages[2].color = STARON;

        if (Reward > 500)
            Reward = 500;

        RewardText.text = "+" + Reward;
        //Playtime_Text.text = playtime/60 + "분" + playtime%60 + "초";
        if ((int)playtime < 60)
        {
            Playtime_Text.text = playtime.ToString("N2") + "초"; // 소주점 지정 N
        }
        else
        {
            Playtime_Text.text = " " + ((int)playtime / 60) + "분" + ((int)playtime % 60) + "초";

        }

        /* string resultPlaytime = DBManager.TimeToString((int)playtime);  // 플레이타임을 DB에 넣을 수 있는 형태(string)으로 변경
                                                                        // 게임결과 정보를 담은 gameResult 객체를 만듦
                                                                        // 매개변수 1: 게임이름, 2: 플레이타임, 3: 리워드
        GameResult gameResult = DBManager.CreateGameResult("Goalkeeper", resultPlaytime, reward);
        StartCoroutine(DBManager.SaveGameResult(gameResult));   // DB저장 시도*/

    }

    public void setExplain()
    {
        StageCount.text = "스테이지 " + (Chef_StageManagement._Instance.stageNum + 1).ToString() + " !";
        for (int i = 0; i < Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length; i++)  // 음식에 필요한 재료만큼 for 문을 반복하게함 
        {
            FoodExplain += Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients[i];
            if( i < (Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length -1 ))
                FoodExplain += ", ";
        }
        ExplainFood.text = FoodExplain + "를 모아서 " + Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj.name + "를 만들어주세요!";
        FoodImage.sprite = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj;
        CompleteImage.sprite = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj;
    }

    // 게임 종료
    public void Quit()
    {
        SceneManager.LoadScene("Chef_Main");
    }

    IEnumerator returnDelay()
    {
        yield return new WaitForSeconds(2f);
    }
}