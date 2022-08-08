using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class kayak_GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    public GameObject CurrentPlayer;
    public GameObject StartPointStone;
    Vector3 PlayerStartPos = new Vector3(131.4f, -2.077f, -23.86f);
    Vector3 PlayerStartRot = new Vector3(0, -79.871f, -0);
    Vector3 StonePos = new Vector3(117.94f, -3.6f, -28.4f);

    public GameObject PausePanel;
    //public GameObject DeathPanel;
    public GameObject RewardPanel;
    public GameObject StartPanel;
    public GameObject ExplainPanel;
    public Text SurviveTimeText;

    public GameObject StartBtn;
    public Text StartTimeText;

    public Text ScoreText;

    float SurviveTime = 0f;
    public bool PauseState;

    public GameObject SharkPrefab;
    public GameObject SharkParent;
    public GameObject Path;
    public OpenZenMoveObject openZenMoveObjectcs;
    GameObject CreatedShark;

    private int MinReward = 100;
    private int MaxReward = 500;

    public List<Image> StarImages;  // 인스펙터 창에서 별 이미지 오브젝트 넣기
    public Text RewardText;
    public Text TotalPlaytimeText;

    #region 
    // 색상값을 담을 Color 인스턴스
    Color RED;
    Color GREEN;
    Color DEFAULT;


    Color STARON, STAROFF;  // 별 색깔을 담을 Color 오브젝트

    #endregion

    public GameObject canvas;

    private static kayak_GameManager _game;
    public static kayak_GameManager Game
    {
        get { return _game; }
    }

    private void Awake()
    {
        _game = GetComponent<kayak_GameManager>();
        PauseState = true;
        canvas = GameObject.Find("Canvas (1)");
        // thisButton.onClick.AddListener(OnKeyHit);
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Shark Creator
        for (int i = 0; i < Path.transform.childCount; i++)
        {
            GameObject shark = Instantiate<GameObject>(SharkPrefab, SharkParent.transform);
            shark.transform.GetComponent<PathFollower>().pathCreator = Path.transform.GetChild(i).GetComponent<PathCreator>();
        }
        #endregion

        ExplainPanel.SetActive(true);
        //StartGame();
        // 색상값 초기화
        ColorUtility.TryParseHtmlString("#FF8888", out RED);
        ColorUtility.TryParseHtmlString("#8DFF88", out GREEN);
        ColorUtility.TryParseHtmlString("#FFFFFF", out DEFAULT);

        ColorUtility.TryParseHtmlString("#FFCC47", out STARON); // 노란별 색
        ColorUtility.TryParseHtmlString("#374355", out STAROFF); // 까만별 색
        //explainPanel.SetActive(true);

        for (int i = 0; i < StarImages.Count; i++)
        {
            StarImages[i].color = STAROFF;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseState)
        {
            if (!CurrentPlayer.GetComponent<kayak_CharacterMovement>().isDeath)
            {
                SurviveTime += Time.deltaTime;
                SurviveTimeText.text = "생존 시간 : " + SurviveTime.ToString("N1") + " 초";
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Revive();
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    Application.Quit();
                }
            }
        }

        #region 카메라 Rig 조정
        if (Input.GetKey(KeyCode.UpArrow))
            GameObject.Find("XR Rig").transform.position += new Vector3(0, 0, 0.01f);
        else if (Input.GetKey(KeyCode.DownArrow))
            GameObject.Find("XR Rig").transform.position -= new Vector3(0, 0, 0.01f);
        else if (Input.GetKey(KeyCode.LeftArrow))
            GameObject.Find("XR Rig").transform.position -= new Vector3(0.01f, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow))
            GameObject.Find("XR Rig").transform.position += new Vector3(0.01f, 0, 0);
        else if (Input.GetKey(KeyCode.Alpha1))
            GameObject.Find("XR Rig").transform.position += new Vector3(0, 0.01f, 0);
        else if (Input.GetKey(KeyCode.Alpha2))
            GameObject.Find("XR Rig").transform.position -= new Vector3(0, 0.01f, 0);
        #endregion
    }

/*    public void PlayerDeath()
    {
        // PauseState = true;
        DeathPanel.SetActive(true);
        //생존시간 * 현재속도 * 1.5=점수
        ScoreText.text = "점수 : " + (SurviveTime * CurrentPlayer.GetComponent<kayak_CharacterMovement>().speed * 1.5f + 10).ToString("N0") + "\n" + SurviveTimeText.text;
    }*/

    public void Reward()
    {
        string min = ((int)SurviveTime / 60).ToString("D2");
        string sec = ((int)SurviveTime % 60).ToString("D2");
        int reward = MinReward + ((int)SurviveTime - 40) * ((MaxReward - MinReward) / (120 - 40));
        Time.timeScale = 0f;    // 게임정지

        // 리워드 범위 별로 별 추가
        if (reward < 100)
        {
            RewardText.text = "0";
            reward = 0;
        }
        if (reward >= 100)
            StarImages[0].color = STARON;
        if (reward >= 234)
            StarImages[1].color = STARON;
        if (reward >= 367)
            StarImages[2].color = STARON;

        if (reward > 500)
            reward = 500;


        RewardText.text = reward.ToString();
        TotalPlaytimeText.text = string.Format("{0}:{1}", min, sec);
        RewardPanel.SetActive(true);
    }
    public void Revive()
    {
        print("Revive()");
        //Destroy(CurrentPlayer.transform.Find("Main Camera").gameObject); // VR 미사용시 prefa b에서 maincamera 켜기
        Destroy(CurrentPlayer.transform.Find("XR Rig").gameObject); // VR 사용시
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().enabled = false;

        RewardPanel.SetActive(false);
        SurviveTime = 0;
        SurviveTimeText.text = "생존 시간 : " + SurviveTime.ToString("N1") + " 초";

        CurrentPlayer = Instantiate(PlayerPrefab, PlayerStartPos, Quaternion.Euler(PlayerStartRot));
        StartPointStone.transform.position = StonePos;

        // 복제하기 전에 참조해서 오류나는 듯 그래서 Invoke 처리함
        Invoke("RREinit", 1.5f);

        // , "SurviveTimeText", "StartBtn", "StartTimeText", "ScoreText"
        // var String_Array = new string[] { "PausePanel", "DeathPanel", "StartPanel"};
        // Reinit(String_Array);
    }

    void RREinit()
    {
        canvas = GameObject.Find("KayakV2 1(Clone)").transform.Find("Canvas (1)").gameObject;
        print(canvas);
        PausePanel = canvas.transform.Find("PausePanel").gameObject;
        RewardPanel = canvas.transform.Find("Reward").gameObject;
        StartPanel = canvas.transform.Find("StartPanel").gameObject;
        SurviveTimeText = canvas.transform.Find("surviveTimeText").GetComponent<Text>();
        StartBtn = canvas.transform.Find("StartBtn").gameObject;
        StartTimeText = canvas.transform.Find("startTimeText").GetComponent<Text>();
        ScoreText = canvas.transform.Find("Scoretext").GetComponent<Text>();
    }

    // 넘겨받은 스트링 배열의 변수 객체를 다시 초기화 해주는 함수
    public void Reinit(string[] name) // ex) name = ["PausePanel", "DeathPanel", "StartPanel"] 
    { 
        for(int i=0; i<name.Length; i++)
        {
            FieldInfo a = GetType().GetField(name[i]);
            object tmp = new GameObject();
            a.SetValue(tmp, canvas.transform.Find(name[i]).gameObject);
        }
    }


    public void PauseTheGame()
    {                                      
        PausePanel.SetActive(true);
        PauseState = true;
        Time.timeScale = 0;
    }
    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PauseState = false;

    }
    public void Quit()
    {
        Time.timeScale = 1;
        PauseState = false;
        SceneManager.LoadScene("mainCity");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
#else
                        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene("KayakGame");
    }

    public void StartGame() // VR 미사용 -> 사용
    {
        print("버튼 눌림");
        StartCoroutine(StartBtnOnClick());
    }

    IEnumerator StartBtnOnClick()
    {
        StartBtn.SetActive(false);
        StartTimeText.gameObject.SetActive(true);
        openZenMoveObjectcs.runstart();
        StartTimeText.text = "3";
        yield return new WaitForSeconds(1f);
        StartTimeText.text = "2";
        yield return new WaitForSeconds(1f);
        StartTimeText.text = "1";
        yield return new WaitForSeconds(1f);

        StartPanel.SetActive(false);
        PauseState = false;
        StartPointStone.GetComponent<Rigidbody>().useGravity = true;
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().Paddle.GetComponent<Animator>().speed = 1;
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().BoatWater.Play();
    }

}
