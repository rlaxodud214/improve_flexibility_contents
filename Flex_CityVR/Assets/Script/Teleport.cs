using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public bool fadeOut; // timer
    private float maxTime = 3; // 유지해야하는 시간
    private int minTime = 0; // 플레이어가 유지 중인 시간
    public static Teleport instance;   // 싱글톤 

    void Awake()
    {
        Teleport.instance = this;
        fadeOut = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeOut = true;
            Timer_();
        }
    }

    public void Timer_()
    {
        if (fadeOut == true)
        {
            maxTime += Time.deltaTime;
            if (maxTime >= 1)
            {
                minTime++;
                maxTime -= 1;
                Debug.Log("min : " + minTime);
                if (minTime == 3)
                {
                    //이벤트
                    print("버텼다!");
                    fadeOut = false;
                }
            }
        }
        else
        {
            maxTime = 0;
            minTime = 0;
        }
    }

    public void teleport()
    {

    }

}
