using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // Store을 열거나 종료할 경우 InfromType 변경은 버튼 onClick에서 수행
    //public GameObject content;
    //public GameObject petItemPrefab;

    [HideInInspector]
    public int UserMoney; //임시
    public GameObject PetShop;
    public Text UserMoneyText;
    private int itemCost;
    bool canBuy = false;

    public static Store instance;
    private void Awake()
    {
        Store.instance = this;
        UserMoney = 100000;
        // UserMoney 연동
        UserMoneyText.text = GetThousandComma(UserMoney).ToString();
    }

    void Start()
    {
        /// 동적 Item 추가///
/*        for (int i = 0; i < 3; i++)
        {
            Instantiate<GameObject>(this.petItemPrefab, content.transform);
        }*/

    }

    void Update()
    {
        
    }

    // 1000단위 comma
    public string GetThousandComma(int data)
    {
        return string.Format("{0:#,###}",data);
    }

    public void ItemClick(int itemMoney)
    {
        UIManager.instance.setInformType(2); // Shop
        UIManager.instance.informPanel.SetActive(true);
        UIManager.instance.informText.text = "아이템을 구매하시겠습니까?"; //informtype shop일때
        itemCost = itemMoney;

        if (UserMoney >= itemCost)
        {
            canBuy = true;
        }
        else
            canBuy = false;
    }

    public void BuyItem()
    {
        StartCoroutine(UIManager.instance.SimpleInform());
        if (canBuy)
        {
            UIManager.instance.informText_simple.text = "구매를 완료했습니다.";
            UserMoney -= itemCost;
            UserMoneyText.text = GetThousandComma(UserMoney).ToString();
        }
        else
        {
            UIManager.instance.informText_simple.text = "다이아가 부족합니다.";
        }
    }

}
