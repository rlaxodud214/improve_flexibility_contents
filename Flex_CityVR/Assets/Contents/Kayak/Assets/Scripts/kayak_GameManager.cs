using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kayak_GameManager : MonoBehaviour
{
    GameObject PlayerPrefab;

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
    GameObject CreatedShark;

    private static kayak_GameManager _game;
    public static kayak_GameManager Game
    {
        get { return _game; }
    }

    private void Awake()
    {
        _game = GetComponent<kayak_GameManager>();
        PauseState = true;
        // thisButton.onClick.AddListener(OnKeyHit);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartPointStone.GetComponent<Rigidbody>().useGravity = false;
        if (!GameObject.Find("KayakV2"))
        {
            CurrentPlayer = Instantiate(PlayerPrefab, PlayerStartPos, Quaternion.Euler(PlayerStartRot));
        }
        else
        {
            CurrentPlayer = GameObject.Find("KayakV2");
        }
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().Paddle.GetComponent<Animator>().speed = 0;
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().BoatWater.Stop();

        for(int i = 0; i < Path.transform.childCount; i++)
        {
            CreatedShark = Instantiate(SharkPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
            CreatedShark.GetComponent<PathFollower>().pathCreator = Path.transform.GetChild(i).GetComponent<PathCreator>();
            CreatedShark.transform.parent = SharkParent.transform;
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

    public void PlayerDeath()
    {
        DeathPanel.SetActive(true);
        //생존시간 * 현재속도 * 1.5=점수
        ScoreText.text = "점수 : " + (SurviveTime * CurrentPlayer.GetComponent<kayak_CharacterMovement>().speed * 1.5f + 10).ToString("N0") + "\n" + SurviveTimeText.text;
    }
    public void Revive()
    {
        //Destroy(CurrentPlayer.transform.Find("Main Camera").gameObject); // VR 미사용시 prefab에서 maincamera 켜기
        Destroy(CurrentPlayer.transform.Find("XR Rig").gameObject); // VR 사용시
        CurrentPlayer.GetComponent<kayak_CharacterMovement>().enabled = false;

        DeathPanel.SetActive(false);
        SurviveTime = 0;
        SurviveTimeText.text = "생존 시간 : " + SurviveTime.ToString("N1") + " 초";

        CurrentPlayer = Instantiate(PlayerPrefab, PlayerStartPos, Quaternion.Euler(PlayerStartRot));
        StartPointStone.transform.position = StonePos;
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

    public void StartGame() // VR 미사용
    {
        StartCoroutine(StartBtnOnClick());
    }

    IEnumerator StartBtnOnClick()
    {
        StartBtn.SetActive(false);
        StartTimeText.gameObject.SetActive(true);
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
