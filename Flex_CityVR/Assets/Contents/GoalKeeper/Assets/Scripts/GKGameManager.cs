using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GKGameManager : MonoBehaviour
{
    #region Public Fields

    public GameObject ball;
    public GameObject Kicker;

    public int level;   // 몇 단계인지
    public int life;    // 생명
    public int shootingCount;   // Kicker가 공을 찬 횟수(리워드 산정용)
    public Image backImg;
    public GameObject XR_Rig;

    #endregion

    #region Private Fields

    Animator KickerAnim;
    BallController ballController;

    bool isStart;
    bool backwardAnim;
    bool forwardAnim;
    int chance;

    float playtime; // 플레이타임
    string min, sec;
    bool isTimerActive; // 타이머 작동 여부

    #endregion

    const int minReward = 100;
    const int maxReward = 500;

    #region MonoBehaviour Callbacks

    void Awake()
    {
        ballController = ball.GetComponent<BallController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        KickerAnim = Kicker.GetComponent<Animator>();

        isStart = false;
        backwardAnim = false;
        forwardAnim = false;

        chance = 5; // 데모용 수정 5->2
        level = 1;
        life = 5;
        shootingCount = 0;

        // 게임 플레이 시간
        playtime = 0f;
        isTimerActive = false;
        GKUIManager.Instance.PlaytimeText.text = string.Format("00   00");   // 공백은 3개
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("mainCity");
        }*/
        // 타이머
        if (isTimerActive)
        {
            playtime += Time.deltaTime;
            min = ((int)playtime / 60).ToString("D2");
            sec = ((int)playtime % 60).ToString("D2");
            GKUIManager.Instance.PlaytimeText.text = string.Format("{0}   {1}", min, sec);
        }

        if (isStart)
        {
            isStart = false;
            ballController.stageDone = false;
            StartCoroutine(StartKicking());
        }
        
        // 뒤로 무르는 애니메이션
        if (backwardAnim)
        {
            Kicker.transform.Translate(Vector3.back * Time.deltaTime);
        }
        
        // 앞으로 달려나오는 애니메이션
        else if (forwardAnim)
        {
            Kicker.transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        }

        //if (BNG.InputBridge.Instance.AButton && BNG.InputBridge.Instance.)
    }

    #endregion

    #region Public Methods

    public void GameStart()
    {
        isStart = true;
        isTimerActive = true;
    }

    #endregion

    #region Coroutines

    IEnumerator StartKicking()
    {
        shootingCount++;

        KickerAnim.SetTrigger("backward");
        backwardAnim = true;
        yield return new WaitForSeconds(2f);
        backwardAnim = false;

        KickerAnim.SetTrigger("forward");
        forwardAnim = true;
        yield return new WaitForSeconds(1f);
        forwardAnim = false;

        KickerAnim.SetTrigger("kick");
        ballController.KickingBall();

        yield return new WaitUntil(() => ballController.stageDone == true);

        // 골 막기 성공 여부
        if (ballController.isSuccess)
        {
            GKUIManager.Instance.ChangeToggleColor(true);
            chance--;
        }
        else
        {
            GKUIManager.Instance.ChangeToggleColor(false);
            chance--;
            life--;
        }

        yield return new WaitForSeconds(2f);

        if (life <= 0)
        {
            string totalPlaytime = min + ":" + sec;
            int reward;

            // 최소 기준치 미달 시 reward 지급 X
            if (shootingCount < 8)
                reward = 0;
            else
            {
                reward = minReward + (shootingCount - 8) * ((maxReward - minReward) / (14 - 8));
            }

            // 최대값을 넘을 경우 reward를 최댓값으로 설정
            if (reward > 500)
                reward = 500;

            
            string resultPlaytime = DBManager.TimeToString((int)playtime);  // 플레이타임을 DB에 넣을 수 있는 형태(string)으로 변경
            // 게임결과 정보를 담은 gameResult 객체를 만듦
            // 매개변수 1: 게임이름, 2: 플레이타임, 3: 리워드
            GameResult gameResult = DBManager.CreateGameResult("Goalkeeper", resultPlaytime, reward);
            StartCoroutine(DBManager.SaveGameResult(gameResult));   // DB저장 시도

            GKUIManager.Instance.ShowResult(reward, totalPlaytime);
        }

        else
        {
            // 5번의 기회 동안 게임오버되지 않으면 다음 level로
            if (chance == 0)
            {
                chance = 5; // 데모용 수정 5->2
                level++;
                ballController.LevelUp(level);
                GKUIManager.Instance.ChangeLevel(level);
                GKUIManager.Instance.InitiateToggles();

                
            }

            yield return StartCoroutine(FadeInCamera(backImg, 1f));
            XR_Rig.transform.position = new Vector3(-120f, 0.2f, 62.116f);
            //XR_Rig.transform.Translate(-120f, 0.5f, 62.406f);
            yield return StartCoroutine(FadeOutCamera(backImg, 1f));
            isStart = true;
        }
    }

    IEnumerator FadeInCamera(Image img, float fadeInTime) // 페이드 인 : 투명 > 불투명
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함

        // 투명하게 초기화
        img.gameObject.SetActive(true);
        Color temp = img.color;
        temp.a = 0;
        img.color = temp;
        //
        float t = 0f; // 0~1 일때 t=0; 0.5~1일때 t=0.5 와 같이 선형보간 값 구함
        temp.a = Mathf.Lerp(0f, 1f, t); // 투명~불투명

        while (temp.a < 1f)
        {
            t += Time.deltaTime / fadeInTime;
            temp.a = Mathf.Lerp(0f, 1f, t);
            img.color = temp;
            yield return null;
        }
    }

    IEnumerator FadeOutCamera(Image img, float fadeOutTime) // 페이드 아웃 : 불투명 > 투명
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함

        // 불투명하게 초기화
        if (img.gameObject.activeSelf == false)
            img.gameObject.SetActive(true);
        Color temp = img.color;
        temp.a = 255;
        img.color = temp;
        //

        float t = 0f;
        temp.a = Mathf.Lerp(1f, 0f, t); // 불투명~투명

        while (temp.a > 0f)
        {
            t += Time.deltaTime / fadeOutTime;
            temp.a = Mathf.Lerp(1f, 0f, t);
            img.color = temp;
            yield return null;
        }
        img.gameObject.SetActive(false);
    }

    #endregion
}
