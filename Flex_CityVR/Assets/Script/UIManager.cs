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

    // MainCity 입장 소개창
    private int index = 0;
    public Button ButtonL, ButtonR;
    public List<GameObject> explainPanel;
    private Button previousBtn;
    public List<GameObject> AxisContentPanel;
    private GameObject previousAxisContent;
    private Color Blue, White;
    //
    void Awake()
    {
        UIManager.instance = this;

        #region 변수 초기화
        UISetting.informType = EinformType.None;    //알림창 타입 초기화
        index = 0;
        previousBtn = null;
        previousAxisContent = null;
        ColorUtility.TryParseHtmlString("2A3542", out Blue);
        ColorUtility.TryParseHtmlString("FFFFFF", out White);
        #endregion
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
            case EinformType.PetSafe:
                print("PetSafe Cancle");
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
            case EinformType.PetSafe:
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

    #region 메인시티 소개창 UI
    public void OnExplain()
    {
        InitExplain(explainPanel);
        InitExplain(AxisContentPanel);
    }
    public void InitExplain(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0)
            {
                list[index].SetActive(true);
            }
            else
            {
                list[index].SetActive(false);
            }
        }
    }
    public void ExplainLeftBtn()
    {
        explainPanel[index].SetActive(false); // 현재 켜져있는 패널 끄기
        index -= 1;
        if (index < 0)
        {
            index = explainPanel.Count - 1;
        }
        explainPanel[index].SetActive(true);
    }

    public void ExplainRightBtn()
    {
        explainPanel[index].SetActive(false);
        index += 1;
        if (index > explainPanel.Count -1)
        {
            index = 0;
        }
        explainPanel[index].SetActive(true);
    }

    public void SelectAxisBtn(Button nowBtn)
    {
        if(previousBtn != null)
        {
            previousBtn.image.color = White;
            previousBtn.transform.GetChild(0).GetComponent<Text>().color = Blue;
            previousAxisContent.SetActive(false);
        }
        nowBtn.image.color = Blue;
        nowBtn.transform.GetChild(0).GetComponent<Text>().color = White;
        previousBtn = nowBtn;

        int index = int.Parse(nowBtn.name.Substring(0, 1)); // 1개, 2개, 3개 축 -> 1,2,3
        AxisContentPanel[index - 1].SetActive(true);
        previousAxisContent = AxisContentPanel[index - 1].gameObject;
    }
    #endregion
}
