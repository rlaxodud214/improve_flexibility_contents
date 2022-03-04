using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class USER_Info : MonoBehaviour
{
    public Text name, age;
    public Toggle ses_Toggle_Man, ses_Toggle_Woman;

    public string[] user_infomation; 

    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.
    private static USER_Info _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    public static USER_Info Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        user_infomation = new string[3];
    }

    public void button_click()
    {
        user_infomation[0] = name.text;
        user_infomation[1] = age.text;
        if (ses_Toggle_Man.isOn)
            user_infomation[2] = "남성";
        if (ses_Toggle_Woman.isOn)
            user_infomation[2] = "여성";
    }

    public class DontDestoryObject : MonoBehaviour { 
        private void Awake() {
            
        } 
    }
}
