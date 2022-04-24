using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kayak_CharacterMovement : MonoBehaviour
{
    public float data;
    public float speed = 0.02f;
    float time = 0f;

    public bool isDeath = false;

    public GameObject SpeedUpText;
    public GameObject Camera;
    public GameObject Paddle;
    public ParticleSystem BoatWater;
    // Start is called before the first frame update
    void Start()
    {
        /*transform.Find("").gameObject;
        transform.Find("FX_BoatWater_Large").gameObject;*/
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime*5f, Space.World);
        if (!kayak_GameManager.Game.PauseState)
        {
            if (!isDeath)
            {
                //transform.Translate(Vector3.forward * Time.deltaTime * 5f, Space.World);
                transform.position = Vector3.Lerp(transform.position, transform.Find("TargetMovement").position, speed);
            }

            data = OpenZenMoveObject.Instance.sensorEulerData.x;
            try
            {
                // print("data.x : " + data);
            } catch(Exception e)
            {
                print("error log : " + e.ToString());
                return ;
            }
            
            transform.Rotate(new Vector3(0, data / 100, 0), Space.Self);

            time += Time.deltaTime;
            if (time >= 0.5f) // 5초 마다 0.01 속도 증가
            {
                /* time -= 10f;
                 speed *= 1.13f;*/
/*                time -= 0.5f;
                speed += 0.001f;*/
                speed += 0.00001f;
                time -= 0.5f;
                /*SpeedUpText.SetActive(true);
                Invoke("TextOff", 1.5f);*/
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled)
        {
            isDeath = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Destroy(transform.Find("Main Camera").gameObject);
            Destroy(transform.Find("Character_Female_Pirate_01").gameObject);
            Destroy(transform.Find("FX_BoatWater_Large").gameObject);
            transform.Find("Character_Female_Pirate_01 (1)").gameObject.SetActive(true);
            //Camera.GetComponent<Animator>().SetBool("Death", true);
            //GameManager.Game.Revive();
            StartCoroutine(YouDie());
            //this.enabled = false;
        }
    }

    IEnumerator YouDie()
    {
        //Camera.GetComponent<Animator>().SetBool("Death", true); // VR 미사용시
        yield return new WaitForSeconds(3f);
        kayak_GameManager.Game.PlayerDeath();
    }

    void TextOff()
    {
        SpeedUpText.SetActive(false);
    }
}
