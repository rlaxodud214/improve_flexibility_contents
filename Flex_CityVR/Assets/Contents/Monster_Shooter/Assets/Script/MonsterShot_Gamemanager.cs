using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShot_Gamemanager : MonoBehaviour
{
    public GameObject TrackingSpace;
    public bool isStart;
    public float playtime;
    public bool isLotation;
    public List<GameObject> Circle_UI;

    Measurement playerRange;
    // 싱글톤 패턴
    #region Singleton
    private static MonsterShot_Gamemanager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static MonsterShot_Gamemanager Instance
    {
        get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
    }
    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = this;  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
        isStart = true;
        playtime = 0f;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        TrackingSpace.transform.eulerAngles = new Vector3(0, 0, 0);
        isStart = false;
        isLotation = false;
        playerRange = UserDataManager.instance.recentData;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart)
        {
            playtime += Time.deltaTime;
            rotation_check();
        }
        
    }
    public void rotation_check()
    {
        var x = OpenZenMoveObject.Instance.sensorEulerData.x;
        var y = OpenZenMoveObject.Instance.sensorEulerData.y;
        // 좌우측 회전 값이 22를 넘거나 또는 y값이 -20~60이 아닐 때)
        if(Mathf.Abs(x) >= ((float)playerRange.leftRotation * (4f / 5f)) 
            || !(-((float)playerRange.extension * (4f / 5f)) <= y && y <= ((float)playerRange.flexion * (4f / 5f))))
        {
            //print("보정 값" + (float)playerRange.leftRotation * (4f / 5f));
            //print("보정 값" + -(float)playerRange.extension * (4f / 5f));
            //print("보정 값" + (float)playerRange.flexion * (4f / 5f));
            isLotation = true;
            Circle_UI[1].SetActive(false);
            Circle_UI[0].SetActive(true);
            
        }
        else
        {
            isLotation = false;
            Circle_UI[0].SetActive(false);
            Circle_UI[1].SetActive(true);
        }
    }
}
