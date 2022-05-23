using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDatabase : MonoBehaviour
{
    // 구매한 
    //public List<ItemInfo> items;

    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
}
