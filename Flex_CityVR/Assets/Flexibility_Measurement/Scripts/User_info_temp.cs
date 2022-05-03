using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User_info_temp : MonoBehaviour
{
    [HideInInspector]
    public static readonly int user_age = 23;
    [HideInInspector]
    public static readonly string user_name = "김태영";

    #region Singleton                                   // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.
    private static User_info_temp _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    public static User_info_temp Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }
    #endregion

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
/*        user = GameObject.Find("USER_Info").gameObject.GetComponent<USER_Info>().user_infomation;
        info[0].text = "이름 : " + user[0];
        info[1].text = "나이 : " + user[1];
        info[2].text = "성별 : " + user[2];*/

    }
}
