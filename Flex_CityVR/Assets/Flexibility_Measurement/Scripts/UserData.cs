using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public float[] angleValues;

    public static UserData instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        angleValues = new float[6] { 0, 0, 0, 0, 0, 0 };   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
