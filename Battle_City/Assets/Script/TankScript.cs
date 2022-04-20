using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 탱크 조작 스크립트
/// </summary>
public class TankScript : MonoBehaviour
{
    #region Public Static Fields

    public static TankScript instance;

    #endregion

    #region Public Fields

    public Transform CannonCam;    // Tank의 Cannon 카메라 부분
    public Transform CannonRoot;   // Tank의 Cannon 부분

    public float deg = -13; // 위,아래 회전
    public float deg2 = 0;  // 왼쪽 회전
    public float deg3 = 0;  // 오른쪽 회전


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
        CannonCam = transform.GetChild(0);
        CannonRoot = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0);
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
                if(deg <= 20)
                    deg = deg+Time.deltaTime*turretSpeed;

                CannonCam.eulerAngles = new Vector3(deg, CannonCam.eulerAngles.y, CannonCam.eulerAngles.z);
                CannonRoot.eulerAngles = new Vector3(deg, CannonRoot.eulerAngles.y, CannonRoot.eulerAngles.z);
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                if(deg >= -45)
                    deg = deg-Time.deltaTime*turretSpeed;

                CannonCam.eulerAngles = new Vector3(deg, CannonCam.eulerAngles.y, CannonCam.eulerAngles.z);
                CannonRoot.eulerAngles = new Vector3(deg, CannonRoot.eulerAngles.y, CannonRoot.eulerAngles.z);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            { 
                print("RightArrow");
                deg3 = 0;
                deg2 = deg2+Time.deltaTime* rotspeed;
                if(deg2<0.09f)
                {
                    //transform.Rotate(0, deg2, 0);
                    //CannonCam.Rotate(0, deg2, 0);
                    CannonRoot.Rotate(0, deg2, 0);
                    //CannonCam.eulerAngles = new Vector3(CannonCam.eulerAngles.x, deg2, CannonCam.eulerAngles.z);
                    //CannonRoot.eulerAngles = new Vector3(CannonCam.eulerAngles.x, deg2, CannonRoot.eulerAngles.z);
                }
                else
                {
                    //transform.Rotate(0,0.09f, 0);
                    //CannonCam.Rotate(0, 0.09f, 0);
                    CannonRoot.Rotate(0, 0.09f, 0);
                    //CannonCam.eulerAngles = new Vector3(CannonCam.transform.eulerAngles.x, 0.09f, CannonCam.eulerAngles.z);
                    //CannonRoot.eulerAngles = new Vector3(CannonCam.eulerAngles.x, 0.09f, CannonRoot.eulerAngles.z);
                }

            }
            else if(Input.GetKey(KeyCode.LeftArrow))
            {
                // print("LeftArrow");
                deg2 = 0;
                deg3 = deg3-Time.deltaTime*rotspeed;
                if(deg3>-0.09f)
                {
                    // transform.Rotate(0,deg3, 0);
                    //CannonCam.Rotate(0, deg3, 0);
                    CannonRoot.Rotate(0, deg3, 0);
                    //CannonCam.eulerAngles = new Vector3(CannonCam.eulerAngles.x, deg3, CannonCam.eulerAngles.z);
                    //CannonRoot.eulerAngles = new Vector3(CannonCam.eulerAngles.x, deg3, CannonRoot.eulerAngles.z);
                }
                else
                {
                    // transform.Rotate(0,-0.09f, 0);
                    //CannonCam.Rotate(0, -0.09f, 0);
                    CannonRoot.Rotate(0, -0.09f, 0);
                    //CannonCam.eulerAngles = new Vector3(CannonCam.eulerAngles.x, -0.09f, CannonCam.eulerAngles.z);
                    //CannonRoot.eulerAngles = new Vector3(CannonCam.eulerAngles.x, -0.09f, CannonRoot.eulerAngles.z);
                }
            }
            if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                deg2 = 0;
                deg3 = 0;
            }
        }
        
    }

    #endregion
}