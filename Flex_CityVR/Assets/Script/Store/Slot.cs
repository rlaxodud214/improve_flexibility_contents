using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isUse;
    public ItemInfo item;
    public UnityEngine.UI.Image image;

    // 상점에서 아이템 구매 시 인벤토리에 구매 아이템 설정
    public void SetItem(ItemInfo itemInfo)
    {
        item = itemInfo;
        //Debug.Log("SetItem :: name :: " + item.itemName);
        gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
        isUse = true;
        image.enabled = true;
        image.sprite = item.sprite;
    }

    public void ResetItem()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
        isUse = false;
        item = null;
        image.enabled = false;
        image.sprite = null;
    }
}
