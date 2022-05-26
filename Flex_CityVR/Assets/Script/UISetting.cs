using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//알림창 타입 종류
public enum EinformType
{
    None,
    Contents,
    Shop,
    Inventory,
    PetSafe,
}
public class UISetting
{
    public static EinformType informType;
}
