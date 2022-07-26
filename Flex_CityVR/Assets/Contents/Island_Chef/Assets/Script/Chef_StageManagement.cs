using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Chef_StageManagement : MonoBehaviour
{
    public int stageNum;
    public int tryNum;
    public float playtime;
    public GameObject stageNumObject;
    #region Singleton 
    private static Chef_StageManagement instance = null;
    public static Chef_StageManagement _Instance                
    {
        get { return instance; }                     
    }
    private void Awake()  
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            /*SceneManager.sceneLoaded += OnSceneLoaded;*/
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        //stageNum = 0;
    }
    #endregion
    public void Start()
    {
        stageNum = 0;   // 게임 전체적으로 stage 관리를 위한 staeNum을 설정
        //playtime = 0;   // 전체 플레이 시간
        tryNum = 0;     // 재료를 제출한 횟수를 정리
    }
}

