using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Text UI
    public Text informText; // 알림창 Text
    public Text informText_simple; // 알림창 Text2

    // Panel
    public GameObject informPanel; // 알림창 패널
    public GameObject informPanel_simple; // 알림창 패널2 

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
        switch (UISetting.informType)
        {
            case EinformType.None:
                break;
            case EinformType.Contents:
                string key = Player.instance.KeySearch();
                Player.instance.dic_contents[key] = false;
                break;
            case EinformType.Shop:
                break;

        }
        informPanel.SetActive(false);
        // 취소 버튼을 눌렀을 때
/*        print("선택이 취소되었습니다.");
        string key = Player.instance.KeySearch();
        Player.instance.dic_contents[key] = false;
        informPanel.SetActive(false);*/

    }

    public void check()
    {
        informPanel.SetActive(false);
        // 확인 버튼을 눌렀을 때 스위치문으로 informType 따라 수행
        switch (UISetting.informType)
        {
            case EinformType.None:
                break;
            case EinformType.Contents:
                setInformType(0);
                SceneChange.instance.contentsTelport();
                break;
            case EinformType.Shop:
                Store.instance.BuyItem();
                break;
        }
        //setInformType(0); //초기화
    }

    public void checkTrigger()
    {

    }

    public void setInformType(int type)     // 알림창 타입 변경/설정 시
    {
        UISetting.informType = (EinformType)type;
    }

    public IEnumerator SimpleInform()
    {
        informPanel_simple.SetActive(true);
        yield return new WaitForSeconds(3f);
        informPanel_simple.SetActive(false);
    }
}
