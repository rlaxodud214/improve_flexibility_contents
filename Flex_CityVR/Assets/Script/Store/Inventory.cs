using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public Transform slotRoot;
    public GameObject slotPrefab;
    public List<Slot> slots = new List<Slot>();
    public List<ItemInfo> items = new List<ItemInfo>();

    public static Inventory instance;

    private void Awake()
    {
        instance = this;

        // 슬롯 생성 및 초기화
        for(int i=0; i<15; i++)
        {
            Instantiate<GameObject>(this.slotPrefab, slotRoot);
            Transform obj = slotRoot.GetChild(i);
            obj.GetComponent<UnityEngine.UI.Button>().enabled = false;
            Slot slot = obj.GetComponent<Slot>();
            slot.isUse = false;
            slot.image = obj.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
            slots.Add(slot);
        }

        // 펫먹이 개수 초기화
        int petFood = UserDataManager.instance.inventory.petFood;

        for (int i = 0; i < petFood; i++)
            UpdateItem(items[0]);

        // 일반상자 개수 초기화
        int normalBox = UserDataManager.instance.inventory.normalBox;
        for (int i = 0; i < normalBox; i++)
            UpdateItem(items[1]);

        // 프리미엄상자 개수 초기화
        int premiumBox = UserDataManager.instance.inventory.premiumBox;
        for (int i = 0; i < premiumBox; i++)
            UpdateItem(items[2]);

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            slotRoot.GetChild(i).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UseItem);
        }
    }

    // 사용되지 않은 slot을 찾아 재설정
    public void UpdateItem(ItemInfo iteminfo)
    {
        //Debug.Log("UpdateItem :: name ::" + iteminfo.itemName);
        Slot emptySlot = slots.Find(t => t.isUse == false);
        emptySlot.SetItem(iteminfo);
    }

    public void UseItem()
    {
        // 눌린 버튼의 GameObject 가져오기
        GameObject obj = EventSystem.current.currentSelectedGameObject.gameObject;
        Slot slot = obj.transform.GetComponent<Slot>();
        ItemUse.instance.SlotClick(slot);
    }

}
