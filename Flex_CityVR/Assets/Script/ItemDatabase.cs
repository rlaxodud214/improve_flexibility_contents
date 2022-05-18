using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDatabase : MonoBehaviour
{
    [HideInInspector]
    public int PetFood, NormalBox, PremiumBox;

    public static ItemDatabase instance;

    private void Awake()
    {
        // DB 연동으로 아이템 개수 받아오기
        instance = this;
        PetFood = NormalBox = PremiumBox = 0;
    }
}
