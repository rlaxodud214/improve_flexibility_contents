using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class GKUIManager : MonoBehaviour
{
    #region Public Static Fields

    static GKUIManager instance = null;

    public static GKUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = (GKUIManager)FindObjectOfType(typeof(GKUIManager));
            return instance;
        }

        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Public Fields

    public Text PlaytimeText;
    public Text LevelText;
    public Button PauseBtn;
    public ToggleGroup ChancesTG;
    public List<Toggle> toggles;

    public GameObject ResultPanel;
    public List<Image> StarImages;  // 인스펙터 창에서 별 이미지 오브젝트 넣기
    public Text RewardText;
    public Text TotalPlaytimeText;


    public GameObject explainPanel;
    public int toggleIdx;   // toggles 리스트에 접근하기 위한 idx

    #endregion

    #region Private Fields
    // 색상값을 담을 Color 인스턴스
    Color RED;
    Color GREEN;
    Color DEFAULT;


    Color STARON, STAROFF;  // 별 색깔을 담을 Color 오브젝트

    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 색상값 초기화
        ColorUtility.TryParseHtmlString("#FF8888", out RED);
        ColorUtility.TryParseHtmlString("#8DFF88", out GREEN);
        ColorUtility.TryParseHtmlString("#FFFFFF", out DEFAULT);

        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색
        explainPanel.SetActive(true);

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }

        // 토글 초기화
        InitiateToggles();
    }

    #endregion

    #region Public Fields

    public void Pause(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 결과화면 띄우기
    /// </summary>
    /// <param name="reward">리워드</param>
    /// <param name="playtime">플레이타임</param>
    public void ShowResult(int reward, string playtime)
    {
        Time.timeScale = 0f;    // 게임정지

        // 리워드 범위 별로 별 추가
        if (reward < 100)
            RewardText.text = "0";
        if (reward >= 100)
            StarImages[0].color = STARON;
        if (reward >= 234)
            StarImages[1].color = STARON;
        if (reward >= 367)
            StarImages[2].color = STARON;

        if (reward > 500)
            reward = 500;

        RewardText.text = reward.ToString();
        TotalPlaytimeText.text = playtime;

        ResultPanel.SetActive(true);
    }
    
    // 레벨 변경
    public void ChangeLevel(int level)
    {
        LevelText.text = level.ToString();
    }

    // 토글 초기화
    public void InitiateToggles()
    {
        toggleIdx = 0;

        for (int i = 0; i < toggles.Count; i++)
        {
            ColorBlock cb = toggles[i].colors;
            cb.disabledColor = DEFAULT;
            toggles[i].colors = cb;


            toggles[i].interactable = false;
        }

        toggles[toggleIdx].isOn = true;
    }

    // 골 막기 성공 여부 -> 토글 색으로 표현
    public void ChangeToggleColor(bool isSuccess)
    {
        ColorBlock cb = ChancesTG.ActiveToggles().FirstOrDefault().colors;
        if (isSuccess)
        {
            cb.disabledColor = GREEN;

        }
        else
        {
            cb.disabledColor = RED;
        }
        toggles[toggleIdx].colors = cb; // 이전 토글 색상 변경

        toggleIdx++;

        // 인덱스 범위 제한
        if (toggleIdx < toggles.Count)
            toggles[toggleIdx].isOn = true; // 다음 토글로 포커스 가도록
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
}
