using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Chef_TutorialUIManager : MonoBehaviour
{
    public GameObject ExplainInventoryPanel, PlayerMoveExpalinPanel, ExplainInteractPanel;
    public GameObject pausePanel, successPanel, failPanel, iteminventory,CountTry, CompleteFood, UIPanel, interactPanel, returnPanel, stagePanel;
    public bool timerStart;
    public Text StageCount;
    public Text ExplainFood;
    public Image FoodImage;
    public Image CompleteImage;
    public string FoodExplain;
    public bool isExplain;


    #region Singleton 
    private static Chef_TutorialUIManager instance = null;
    public static Chef_TutorialUIManager _Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
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
        timerStart = true;
        Time.timeScale = 1f;
        isExplain = false;

        ExplainInventoryPanel.SetActive(false);
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
        pausePanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
        interactPanel.SetActive(false);
        stagePanel.SetActive(true);
        CountTry.SetActive(false);
        PlayerMoveExpalinPanel.SetActive(false);
        ExplainInteractPanel.SetActive(false);
        Invoke("PlayerMoveExpalinOn", 3f); // 게임의 전체적인 진행방식을 3초동안 띄운 뒤에 없어지게함

        //setExplain();
    }

    public void StagePanelOff()
    {
        stagePanel.SetActive(false);
        ExplainInventoryOn();   // 인벤토리 / 목숨 등의 UI를 설명하는 화면을 호출
    }

    public void ExplainInventoryOn()
    {
        ExplainInventoryPanel.SetActive(true);
        iteminventory.SetActive(true);
        CompleteFood.SetActive(true);
        UIPanel.SetActive(true);
        CountTry.SetActive(true);
        CountTry.SetActive(true);
        Time.timeScale = 0;
        //Invoke("ExplainInventoryOff", 3f); // UI설명하는 Panel을 3초 뒤에 자동으로 꺼지게함
    }

    public void ExplainInventoryOff()
    {
        ExplainInventoryPanel.SetActive(false);
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
        CountTry.SetActive(false);
        Time.timeScale = 1f;
        //PlayerMoveExpalinOn();
    }
    public void PlayerMoveExpalinOn()
    {
        stagePanel.SetActive(false);
        PlayerMoveExpalinPanel.SetActive(true);
        Time.timeScale = 0;
        //Invoke("PlayerMoveExpalinOff", 3f);
    }
    public void PlayerMoveExpalinOff()
    {
        Time.timeScale = 1f;
        PlayerMoveExpalinPanel.SetActive(false);
        pausePanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
        iteminventory.SetActive(true);
        CompleteFood.SetActive(true);
        UIPanel.SetActive(true);
        interactPanel.SetActive(false);
    }
    public void activeinteractionExplainOn()
    {
        if(isExplain == false)
        {
            ExplainInteractPanel.SetActive(true);
            Time.timeScale = 0;
            isExplain = true;
        }
        
    }
    public void activeinteractionExplainOff()
    {
        Time.timeScale = 1f;
        ExplainInteractPanel.SetActive(false);
    }

    void Update()
    {
        // timerStart가 true -> 타이머 작동
        if (timerStart)
        {
            Chef_StageManagement._Instance.playtime += Time.deltaTime;
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
    public void Pause() // 게임창에서 일시정지 버튼을 눌렀을때
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        iteminventory.SetActive(false);
        UIPanel.SetActive(false);
        CompleteFood.SetActive(false);
    }

    // 게임 재개
    public void Play() // 계속하기 버튼을 눌렀을때
    {
        Time.timeScale = 1f;
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
        successPanel.SetActive(true);
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
    }

    public void GameFail()
    {
        Time.timeScale = 0f;
        timerStart = false; // 타이머 종료
        failPanel.SetActive(true);
        iteminventory.SetActive(false);
        CompleteFood.SetActive(false);
        UIPanel.SetActive(false);
    }

    // 게임 재시작
    public void Restart()
    {
        SceneManager.LoadScene("Chef_Tutorial");
    }
    public void NextStage() // 음식의 숫자보다 적을 경우에는 다음 게임씬을 불러오고 아니면 End씬 호출
    {
            SceneManager.LoadScene("Chef_Game");
    }

    public void activeinteraction()
    {
        interactPanel.SetActive(true);
    }

    public void inactiveinteraction()
    {
        interactPanel.SetActive(false);
    }

    public void activereturn()
    {
        returnPanel.SetActive(true);
    }

    public void inactivereturn()
    {
        returnPanel.SetActive(false);
    }
    


    /*public void setExplain()
    {
        StageCount.text = "스테이지 " + (Chef_StageManagement._Instance.stageNum + 1).ToString() + " !";
        for (int i = 0; i < Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length; i++)  // 음식에 필요한 재료만큼 for 문을 반복하게함 
        {
            FoodExplain += Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients[i];
            if (i < (Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length - 1))
                FoodExplain += ", ";
        }
        ExplainFood.text = FoodExplain + "를 모아서 " + Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj.name + "를 만들어주세요!";
        FoodImage.sprite = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj;
        CompleteImage.sprite = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].FoodObj;
    }*/

    // 게임 종료
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator returnDelay()
    {
        yield return new WaitForSeconds(2f);
    }
}