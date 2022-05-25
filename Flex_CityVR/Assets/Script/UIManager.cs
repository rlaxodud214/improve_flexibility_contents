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
                string get_key;
                get_key = Player.instance.KeySearch();
                Player.instance.dic_contents[get_key] = false;
                setInformType(0);
                break;
            case EinformType.Shop:
                print("shop Cancle");
                Store.instance.button.enabled = true;
                Store.instance.button = null;
                break;
            case EinformType.Inventory:
                print("inventory Cancle");
                break;

        }
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
                setInformType(0);
                SceneChange.instance.contentsTelport();
                break;
            case EinformType.Shop:
                Store.instance.BuyItem();
                break;
            case EinformType.Inventory:
                ItemUse.instance.UseItem();
                break;
        }
        informPanel.SetActive(false);
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
