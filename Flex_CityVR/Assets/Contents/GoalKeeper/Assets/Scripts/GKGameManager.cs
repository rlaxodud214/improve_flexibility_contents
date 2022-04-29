using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKGameManager : MonoBehaviour
{
    #region Public Fields

    public GameObject ball;
    public GameObject Kicker;

    public int level;

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
    public bool isTimerActive; // 타이머 작동 여부

    #endregion


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

        chance = 3; // 데모용 수정 5->2
        level = 1;

        // 게임 플레이 시간
        playtime = 0f;
        isTimerActive = false;
        GKUIManager.Instance.PlaytimeText.text = string.Format("00   00");   // 공백은 3개
    }

    // Update is called once per frame
    void Update()
    {
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
        if (level == 3)
        {
            yield return null;
        }

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
        yield return new WaitForSeconds(3f);    // 3초 대기

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
        }

        yield return new WaitForSeconds(2f);

        // 5번의 기회 동안 게임오버되지 않으면 다음 level로
        if (chance == 0)
        {
            chance = 3; // 데모용 수정 5->2
            level++;
            ballController.LevelUp(level);
            GKUIManager.Instance.ChangeLevel(level);
            GKUIManager.Instance.InitiateToggles();
        }

        isStart = true;
    }

    #endregion
}
