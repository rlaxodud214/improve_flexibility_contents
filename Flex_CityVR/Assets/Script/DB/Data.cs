using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ## : Primary-KEY
[Serializable]
public class User
{
    public string email;  // ## 가입 이메일
    public string password; // 패스워드
    public string name;   // 사용자 이름
    public int age;   // 나이
    public bool gender;   // 성별 true: male, false: female
    public string registrationDate;   // 가입일자

    public string displayName;    // 캐릭터 닉네임
    public int money;  // 보유 재화
    public List<string> pets;    // 보유 펫
}

[Serializable]
public class GameResult
{
    public string date;
    public string gameID;
    public string playtime;
    public int reward;
}

[Serializable]
public class Measurement
{
    public string date;
    public int flexion;
    public int extension;
    public int leftFlexion;
    public int rightFlexion;
    public int leftRotation;
    public int rightRotation;
    public int totalFlexibility;
}

[Serializable]
public class InventorySlot
{
    public int petFood;
    public int normalBox;
    public int premiumBox;
}

