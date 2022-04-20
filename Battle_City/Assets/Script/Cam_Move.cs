using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Move : MonoBehaviour
{
    #region Public Fields

    public Transform target;        // 따라다닐 타겟 오브젝트의 Transform
    public GameObject Cannon;
    public Camera TurretCamera;     //플레이어 시점 카메라
    public Camera CannonCamera;     //포격시 하늘에서 보여주는 상공 카메라
    public Camera SubCamera;        //적 시점 카메라
    public Vector3 offset;          //하늘에서 보여주는 상공카메라 위치
    public Vector3 offsetMainCam;

    #endregion

    #region Private Fields

    Transform tr;

    bool changeCamera = false;

    #endregion



    public static Cam_Move instance;


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
        tr = GetComponent<Transform>();
        TurretCamera.enabled = true;
        CannonCamera.enabled = false;
        SubCamera.transform.LookAt(target);
    }

    void Update() {
        // changeCamera : 발사시 카메라 변환 변수
        changeCamera = Fire.instance.changeCamera;
    }

    Vector3 cameraPosition;

    void LateUpdate()
    {
        // print(changeCamera);
        if(changeCamera == true)
        {
            TurretCamera.enabled = false;       //플레이어 시점 카메라 꺼지고
            CannonCamera.enabled = true;        //상공 카메라 보여주고
            transform.position = target.position + offset;  //상공카메라는 타겟위치에서 오프셋 설정만큼 움직여줌
            //tr.position = Vector3.Lerp(tr.position, cameraPosition, speed * Time.deltaTime);
            Invoke("CameraReset", 5.8f);                    //약 5.8초 후에 카메라 원위치
            changeCamera = false;      //카메라바꿔주는 불변수 변경
            Fire.instance.changeCamera = false;     
            Fire.instance.fireActive = false;       //카메라 변경되는 동안 발사 못하게
            UIManager.instance.Active.SetActive(false);  
            Enemy.instance.mCamera = false;
        } 
    }

    #endregion

    #region Public Methods

    // 변경된 카메라 각도 리셋
    void CameraReset()
    {
        Debug.Log("<color=red><a>Cam_Move.cs - CameraReset 실행</a></color>");
        //위에것들 다 다시 원상태로
        Cannon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));      //캐논 회전 위치 원상태로
        Debug.LogFormat("PlayerTank의 rotation: {0}", Cannon.transform.rotation);
        TurretCamera.enabled = true;             
        CannonCamera.enabled = false;
        Fire.instance.fireActive = true;
        changeCamera = false;
        TankScript.instance.deg = -13;
        Fire.instance.speed = 0f;
        UIManager.instance.Active.SetActive(true);
        UIManager.instance.Fail.SetActive(false);
        UIManager.instance.Victory.SetActive(false);
        TankScript.instance.CannonCam.rotation = Quaternion.Euler(new Vector3(-13, 0, 0)); // x:-13 해야지 탱크가 정면을 보는구만
        Enemy.instance.mCamera = true;
    }

    #endregion
}