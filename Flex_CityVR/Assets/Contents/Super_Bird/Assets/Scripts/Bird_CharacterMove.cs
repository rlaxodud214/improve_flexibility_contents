using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird_CharacterMove : MonoBehaviour
{
    public Slider MoveSlider;

    public float Flexion = 0 ; //굴곡
    public float Extension = 0; //신전

    public bool isUp = true;
    public bool isDown = true;

    public float downTemp = 0 ;
    public float upTemp = 0;

    public AudioClip clip;
    public bool istuto =false;

    void Start()
    {
        MoveSlider = GameObject.FindGameObjectWithTag("slider").GetComponent<Slider>();
        MoveSlider.value = 0;
        MoveSlider.maxValue = 95f;
        MoveSlider.minValue = -36.6f;
    }

    // Update is called once per frame
    void Update()
    {
        
            // 게임씬 
            if (!istuto)
            {
                // IMU 연동 시 사용
                // MoveSlider.value = OpenZenMoveObject.Instance.sensorEulerData.y * -1.3f;

                if (MoveSlider.value > 0)
                {
                    if (MoveSlider.value < Flexion)
                    {
                        Flexion = MoveSlider.value;
                    }

                    transform.position = new Vector3(0, (-5.5f / MoveSlider.maxValue) * MoveSlider.value, -17);
                }
                else if (MoveSlider.value < 0)
                {
                    if (MoveSlider.value > Extension)
                    {
                        Extension = MoveSlider.value;
                    }

                    transform.position = new Vector3(0, (6.7f / MoveSlider.minValue) * MoveSlider.value, -17);
                }

                if (!Bird_UIManagerGame._instance.pausePanel.activeSelf && !Bird_UIManagerGame._instance.endPanel.activeSelf)
                {
                    if (isDown && MoveSlider.value > upTemp)
                    {
                        Bird_SoundManager.instance.playoneshot();
                        isDown = false;
                        isUp = true;
                        downTemp = MoveSlider.value;
                    }

                    if (isUp && MoveSlider.value < downTemp)
                    {
                        Bird_SoundManager.instance.playoneshot();
                        isDown = true;
                        isUp = false;
                        upTemp = MoveSlider.value;
                    }
                }
            }
            // 튜토씬
            else
            {
                if (MoveSlider.value > 0)
                {
                    transform.position = new Vector3(0, (-1.5f / MoveSlider.maxValue) * MoveSlider.value, 0);
                }
                else if (MoveSlider.value < 0)
                {
                    transform.position = new Vector3(0, (2.7f / MoveSlider.minValue) * MoveSlider.value, 0);
                }
            }

        
    }
        private void OnCollisionEnter(Collision collision) //몹과 캐릭터가 충돌하였을 때
        {
            if (collision.collider.CompareTag("obstacle"))
            {
                Bird_UIManagerGame._instance.GameOver(); //게임 종료 
            }
        }
    
}