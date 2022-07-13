using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 탱크 조작 스크립트
/// </summary>
public class Battle_Tank : MonoBehaviour
{
    #region Public Fields

    public static Battle_Tank instance;  // Singleton

    public Transform CamY;  // Y축 회전(좌우)
    public Transform CamX;  // X축 회전(위아래)
    //public Transform player;
    public Transform CannonRoot;   // Tank의 Cannon 부분

    public float speed; 
    public float deg1;  // 위/아래 회전
    public float deg2;  // 왼쪽/오른쪽 회전


    #endregion

    #region Private Fields

    float rotspeed;      //움직이는 스피드 조정(좌우)
    float turretSpeed;   //(위아래)

    bool move;


    #endregion

    #region MonoBehaviour Callbacks

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {
        // 2022.07.04 VR카메라를 사용할 것이므로 기존의 카메라는 모두 주석처리 -> 카메라를 고정할 것이므로 다시 해제
        CamY = transform.GetChild(0);
        CamX = transform.GetChild(0).GetChild(0);
        CannonRoot = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0);

        speed = 0.3f;
        deg1 = 0;
        deg2 = 0;

        rotspeed = 5f;
        turretSpeed = 5f;

        move = true;
    }

    void Update()
    {
        // print("move : " + move);
        if(move == true)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            { 
                // 최대각도를 설정해준거구나!!
                if(-45 < deg1 && deg1 <= 20)
                {
                    CamX.Rotate(-speed, 0, 0);
                    CannonRoot.Rotate(-speed, 0, 0);
                    //deg1 = CannonRoot.localRotation.x;
                    deg1 -= speed;
                }
                else
                {
                    //CamY.localRotation = Quaternion.Euler(new Vector3(0, 20f, 0));
                    deg1 = -45f;
                }
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                if(-45 <= deg1 && deg1 < 20)
                {
                    CamX.Rotate(speed, 0, 0);
                    CannonRoot.Rotate(speed, 0, 0);
                    //deg1 = CannonRoot.localRotation.x;
                    deg1 += speed;
                }
                else
                {
                    //CamX.localRotation = Quaternion.Euler(new Vector3(0, -45f, 0));
                    deg1 = 20f;
                }
            }

            else if(Input.GetKey(KeyCode.LeftArrow))
            {
                if(-180f < deg2 && deg2 <= 180)
                {
                    CamY.Rotate(0, -speed, 0);
                    CannonRoot.Rotate(0, -speed, 0);
                    //deg2 = CannonRoot.localRotation.y;
                    deg2 -= speed;
                }
                else
                {
                    //CamY.localRotation = Quaternion.Euler(new Vector3s(0, -45f, 0));
                    deg2 = -180f;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if(-180 <= deg2 && deg2 < 180)
                {
                    CamY.Rotate(0, speed, 0);
                    CannonRoot.Rotate(0, speed, 0);
                    //deg2 = CannonRoot.localRotation.y;
                    deg2 += speed;
                }
                else
                {
                    //CamY.localRotation = Quaternion.Euler(new Vector3(0, 45f, 0));
                    deg2 = 180f;
                }
            }
        }      
    }


    #endregion

    #region Public Methods

    public void ResetTank()
    {
        CamY.rotation = Quaternion.Euler(new Vector3(0, 0, 0));      // 플레이어 탱크 카메라 원위치로
        CamX.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        CannonRoot.rotation = Quaternion.Euler(new Vector3(0, 0, 0));   // 대포 부분 원위치로
        deg1 = 0;
        deg2 = 0;
    }

    #endregion

}