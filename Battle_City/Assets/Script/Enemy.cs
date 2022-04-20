using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour                                

{
    public int level;
    public Transform target;
    public GameObject Enemy2;
    public GameObject m_goHpBar;
    public bool mCamera;

    public static Enemy instance;

    void Start()
    {
        transform.position = new Vector3(164.63f, -5.57f, 817.66f);
        transform.eulerAngles = new Vector3(0, 120f, 0);
        m_goHpBar = GameObject.FindWithTag("EnemySlider");
        mCamera = true;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {

        level = Effect.instance.levelChange;        
        if(level == 1)
        {
            transform.position = new Vector3(165.45f, -5.57f, 810.98f);
            transform.eulerAngles = new Vector3(0, 120f, 0);
        }
        if(level == 2)
        {
            transform.position = new Vector3(3.6f , -0.05f , 6.6f);
        }
        if(level == 3)
        {
            transform.position = new Vector3(-5.3f , -0.05f , 0.15f);
        }
        if(level == 4)
        {
            transform.position = new Vector3(4.35f , 0.3f , 9.2f);
        }
        if(level == 5)
        {
            transform.position = new Vector3( 6.3f , -0.05f , -3f);
        }
        if(level == 6)
        {
            transform.position = new Vector3(4.85f , -0.05f , -8.75f);
        }
        if(level == 7)
        {
            transform.position = new Vector3(-10f , -0.05f , -12.5f);
        }
        Cam_Move.instance.SubCamera.transform.position = new Vector3(transform.position.x , 2.5f , transform.position.z );
        Cam_Move.instance.SubCamera.transform.LookAt(target);
        if(mCamera == true)
        {
            m_goHpBar.transform.position = Camera.main.WorldToScreenPoint(Enemy2.transform.position + new Vector3(0, 0.8f, 0));
        }
        
        
    }


    public void turretDeactive()
    {
        gameObject.SetActive(false);
    }

    public void turretActive()
    {
        gameObject.SetActive(true);
    }
}
