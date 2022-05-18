using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType 
{
    PremiumBox,
    NormalBox,
    PetFood
}

[System.Serializable]
public class ItemInfo : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    public int itemCost;
    public Sprite image;

/*    public bool Use()
    {
        bool isUsed = false;
        isUsed = true;
        return isUsed;
    }*/
}
