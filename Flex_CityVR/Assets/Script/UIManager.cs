using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Text UI
    public Text informText; // 알림창 Text

    // Panel
    public GameObject informPanel; // 알림창 패널

    public static UIManager instance;   // 싱글톤 
    void Awake()
    {
        UIManager.instance = this;
        UISetting.informType = EinformType.None;    //알림창 타입 초기화
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cancle()
    {
        // 취소 버튼을 눌렀을 때
        print("선택이 취소되었습니다.");
        string key = Player.instance.KeySearch();
        Player.instance.dic_contents[key] = false;
        informPanel.SetActive(false);
    }

    public void check()
    {
        // 확인 버튼을 눌렀을 때 스위치문으로 informType 따라 수행
        switch (UISetting.informType)
        {
            case EinformType.None:
                break;
            case EinformType.Contents:
                SceneChange.instance.contentsTelport();
                break;
        }
        setInformType(0);
    }

    public void checkTrigger()
    {

    }

    public void setInformType(int type)     // 알림창 타입 변경/설정 시
    {
        UISetting.informType = (EinformType)type;
    }
}
