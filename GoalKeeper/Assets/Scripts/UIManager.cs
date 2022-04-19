using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class UIManager : MonoBehaviour
{
    #region Public Static Fields

    static UIManager instance = null;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = (UIManager)FindObjectOfType(typeof(UIManager));
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

    public int toggleIdx;   // toggles 리스트에 접근하기 위한 idx

    #endregion

    #region Private Fields
    // 색상값을 담을 Color 인스턴스
    Color RED;
    Color GREEN;
    Color DEFAULT;

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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 색상값 초기화
        ColorUtility.TryParseHtmlString("#FF8888", out RED);
        ColorUtility.TryParseHtmlString("#8DFF88", out GREEN);
        ColorUtility.TryParseHtmlString("#FFFFFF", out DEFAULT);

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

    #endregion
}
