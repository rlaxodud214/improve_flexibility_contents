using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{
    PremiumBox,
    NormalBox,
    PetFood
}

public class Item
{
    public ItemType itemType;
    public string itemName;
    public int itemCost;

    public bool Use()
    {
        bool isUsed = false;
        isUsed = true;
        return isUsed;
    }
}
