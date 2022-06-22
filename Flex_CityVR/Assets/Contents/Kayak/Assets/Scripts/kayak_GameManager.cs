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
    public GameObject DeathPanel;
    public GameObject StartPanel;

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
        //StartGame();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("mainCity");
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

    public void PlayerDeath()
    {
        // PauseState = true;
        DeathPanel.SetActive(true);
        //생존시간 * 현재속도 * 1.5=점수
        ScoreText.text = "점수 : " + (SurviveTime * CurrentPlayer.GetComponent<kayak_CharacterMovement>().speed * 1.5f + 10).ToString("N0") + "\n" + SurviveTimeText.text;
    }
    public void Revive()
    {
        print("Revive()");
        //Destroy(CurrentPlayer.transform.Find("Main Camera").gameObject); // VR 미사용시 prefa b에서 maincamera 켜기
        Destroy(CurrentPlayer.transform.Find("XR Rig").gameObject); // VR 사용시
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().enabled = false;

        DeathPanel.SetActive(false);
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
        DeathPanel = canvas.transform.Find("DeathPanel").gameObject;
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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit(); // 어플리케이션 종료
#endif
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
