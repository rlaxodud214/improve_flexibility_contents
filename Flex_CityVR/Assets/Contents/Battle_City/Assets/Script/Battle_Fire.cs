using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Battle_Fire : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePos;
    public GameObject clone;
    public LineRenderer Line_Renderer;
    public List<Vector3> LineRenderer_position;

    public Image gaugeImage;
    public Text progressText;

    public float speed;
    public bool reset; // 발사 후 원래의 상태로 리셋
    public bool fireActive; // 발사가 가능한 상태인지


    public static Battle_Fire instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        fireActive = false;
        reset = false;
        Line_Renderer = GetComponent<LineRenderer>();
        Line_Renderer.SetWidth(0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {   
        if(fireActive)
        {
            if(InputBridge.Instance.AButton || Input.GetKey(KeyCode.S))
            {   //누를수록 멀리 나가도록
                speed += Time.deltaTime*5f;
            }

            if(InputBridge.Instance.AButtonUp || Input.GetKeyUp(KeyCode.S))
            {   //포탄 복제
                LineRenderer_position.Clear();
                clone = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                SoundManager.instance.FireSound();
                reset = true;
                fireActive = false;
            }
        }

        gaugeImage.fillAmount = speed / 15;
        if (progressText)
        {
            progressText.text = (int)(gaugeImage.fillAmount * 100) + "%";
        }
        if (speed > 20f)
        {

            speed = 20f;
        }
    }

    void LateUpdate()
    {
        if (reset)
        {
            Invoke("ResetCamera", 5.8f);                    //약 5.8초 후에 카메라 원위치 
            reset = false;     //카메라바꿔주는 불변수 변경
            fireActive = false;       //카메라 변경되는 동안 발사 못하게
            //Battle_UIManager.instance.ActivePanel.SetActive(false);
        }
    }

    // 포탄 발사하고 라인렌더러 그려줌
    public void removeBullet()
    {
        var LineRenderer_position_remove = new List<Vector3>();
        for(int i=0; i< LineRenderer_position.Count; i+=10) {
            LineRenderer_position_remove.Add(LineRenderer_position[i]);
            // print(LineRenderer_position[i]);
        }
        Line_Renderer.positionCount = LineRenderer_position_remove.Count;
        Line_Renderer.SetPositions(LineRenderer_position_remove.ToArray());
        LineRenderer_position.Clear();
        LineRenderer_position_remove.Clear();
        
        Destroy(clone, 6f);
    }

    #region Private Methods

    void ResetCamera()
    {
        //위에것들 다 다시 원상태로
        Battle_Tank.instance.ResetTank();
        Battle_UIManager.instance.InitPanels();
        Battle_Fire.instance.fireActive = true;
        Battle_Fire.instance.speed = 0f;      
    }

    #endregion
}
