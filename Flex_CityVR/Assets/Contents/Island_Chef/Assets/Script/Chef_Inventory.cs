using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef_Inventory : MonoBehaviour
{
    public List<Chef_SlotData> slots = new List<Chef_SlotData>();
    public List<GameObject> count = new List<GameObject>();
    //private int maxSlot = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length;
    private int maxSlot;
    public GameObject slotPrefab, countPrefab;
    public int idx;


    #region Singleton 
    private static Chef_Inventory instance = null;
    public static Chef_Inventory _Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
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


    private void Start()
    {
        maxSlot = Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length;
        GameObject slotPanel = GameObject.Find("ItemInventory");
        print(maxSlot);
        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform, false);
            go.name = "Slot_" + i;
            Chef_SlotData slot =  this.gameObject.AddComponent<Chef_SlotData>();
            //Chef_SlotData slot = new Chef_SlotData();
            slot.isEmpty = true;
            slot.slotObj = go;
            slots.Add(slot);
        }

        GameObject TryPannel = GameObject.Find("CountTry");

        for ( int i = 0; i<3; i++)
        {
            GameObject go = Instantiate(countPrefab, TryPannel.transform, false);
            count.Add(go);
        }
        idx = 0;
    }
    public void emptyslot()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (slots[i].slotObj.transform.childCount != 0)
            {
                print("1");
                Destroy(slots[i].slotObj.transform.GetChild(0).gameObject);
                print("2");
                slots[i].isEmpty = true;
                print("3");
            }
        }
    }

    public void discountTry()
    {
        Destroy(count[idx]);
        idx++;
    }
}