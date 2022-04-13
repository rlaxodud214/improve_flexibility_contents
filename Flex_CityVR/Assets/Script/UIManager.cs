using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Text UI
    public Text informText; // 알림창 Text

    // Panel
    public GameObject informPanel; // 알림창 패널

    public static UIManager instance;   // 싱글톤 
    void Awake()
    {
        UIManager.instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cancle()
    {
        // 취소 버튼을 눌렀을 때
    }

    public void check()
    {
        // 확인 버튼을 눌렀을 때
    }

    public void checkTrigger()
    {
        // 트리거 켜져있는 애 확인 if elseif else 로 확인해서 콘텐츠로 이동시키기
    }
}
