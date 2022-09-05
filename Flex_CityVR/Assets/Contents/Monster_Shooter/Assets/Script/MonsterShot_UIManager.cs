using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterShot_UIManager : MonoBehaviour
{
    public int user_hp;                                  // 남은 목숨, 하트 갯수
    public GameObject[] Life = new GameObject[4];     // 남은 목숨을 저장할 배열
    public GameObject GetDamage; // 데미지 입었을 때 패널
    public GameObject ResultPanel; // 리워드 패널
    public GameObject PausePanel; // 일시정지 패널
    public GameObject StartPanel; // 게임시작 패널
    public GameObject ExpalinPanel; // 설명 패널
    public Text RewardText;
    public Text TotalPlaytimeText;
    public int reward;
    public float playtime;
    int MinReward, MaxReward;
    public List<Image> StarImages;  // 인스펙터 창에서 별 이미지 오브젝트 넣기
    Color STARON, STAROFF;  // 별 색깔을 담을 Color 오브젝트


    // 싱글톤 패턴
    #region Singleton
    private static MonsterShot_UIManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static MonsterShot_UIManager Instance
    {
        get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
    }
    #endregion
    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = this;  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
        ExpalinPanel.SetActive(true);
        MinReward = 100;
        MaxReward = 500;
        reward = 0;
        playtime = 0;
        Invoke("sss", 2f);
        
    }
    void sss()
    {
        Time.timeScale = 0;
    }

    void Start()
    {
        user_hp = 4;
        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }
        StartPanel.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        Time.timeScale = 1;
        OpenZenMoveObject.Instance.runstart();
        MonsterShot_Gamemanager.Instance.isStart = true;
    }

    // 일시 정지창 띄우기
    public void Pause() 
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene("MonsterShot_GameScene");
    }

    // 메인시티로 나가기 버튼
    public void Exit()
    {
        SceneManager.LoadScene("mainCity");
    }

    public void ShowResult()
    {
        Time.timeScale = 0f;    // 게임정지

        print(reward);
        // 리워드 범위 별로 별 추가
        if (reward >= 100)
            StarImages[0].color = STARON;
        if (reward >= 234)
            StarImages[1].color = STARON;
        if (reward >= 367)
            StarImages[2].color = STARON;

        var h = ((int)(playtime / 60f)).ToString("N0");
        var m = ((int)(playtime % 60f)).ToString("N0");
        // PadLeft : 남는 자리 0으로 채우기
        TotalPlaytimeText.text = h.PadLeft(2, '0') + ":" + m.PadLeft(2, '0');
        RewardText.text = reward.ToString("N0");
        ResultPanel.SetActive(true);
    }

    // 결과창 띄우고 DB 연동해서 결과 저장하기
    public void Result()
    {
        playtime = MonsterShot_Gamemanager.Instance.playtime;
        reward = (int)(MinReward + (playtime - 50) * ((MaxReward - MinReward) / (150 - 50)));
        if (reward > MaxReward)
            reward = MaxReward;
        if (reward < MinReward)
            reward = 0;

        ShowResult();
    }

    // 몬스터 충돌 시 hp 감소
    public void HpDown()
    {
        user_hp--;
        if (user_hp < 1)
        {
            Life[user_hp].SetActive(false);
            Result();
            return;
        }
        // UI 하트 1개씩 사라지게
        else
        {
            Life[user_hp].SetActive(false);
            GetDamage.SetActive(true);
            StartCoroutine("DamagePanelEffect");
        }
    }
    IEnumerator DamagePanelEffect()
    {
        yield return new WaitForSeconds(0.50f);
        GetDamage.SetActive(false);
    }
}
