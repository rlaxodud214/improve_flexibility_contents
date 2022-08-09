using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using BNG;


public class Chef_PlayerMove : MonoBehaviour
{
    //public Slider YSlider;
    //public Slider XSlider;
    public float speed = 15f;
    public bool isLadder;
    public Vector3 IMU_data;
    // Start is called before the first frame update
    float XmaxValue = 34.6f;
    float XminValue = -34.4f;
    float YmaxValue = 95f;
    float YminValue = -36.6f;


    void Awake()
    {
        /*YSlider = GameObject.FindGameObjectWithTag("YSlider").GetComponent<Slider>();
        XSlider = GameObject.FindGameObjectWithTag("XSlider").GetComponent<Slider>();
        XSlider.value = 0;
        YSlider.value = 0;
        XSlider.maxValue = 34.6f;
        XSlider.minValue = -34.4f;
        YSlider.maxValue = 95f;
        YSlider.minValue = -36.6f;*/
        //OpenZenMoveObject.Instance.runstart();
        //OpenZenMoveObject.Instance.Calibration();
    }

    // Update is called once per frame
     void FixedUpdate()
     {
        IMU_data = OpenZenMoveObject.Instance.sensorEulerData;
        //YSlider.value = OpenZenMoveObject.Instance.sensorEulerData.y * -1.3f;
        //XSlider.value = OpenZenMoveObject.Instance.sensorEulerData.x * 1.3f;
        //print("XSlider.value" + XSlider.value + ", YSlider.value : " + YSlider.value);
        //print("XSlider.value" + IMU_data.x + ", YSlider.value : " + IMU_data.y);
         if (IMU_data.y > 9.5)
         {
            //transform.rotation = Quaternion.Euler(new Vector3(0, (XSlider.value / XSlider.maxValue) * 90, 0));
            //transform.Translate(Vector3.forward * (YSlider.value / YSlider.maxValue) * 1f );
            // transform.Translate(Vector3.forward * YSlider.value );
            transform.Translate(Vector3.forward * (IMU_data.y / YmaxValue) * 0.7f);
        }
/*         else if ((IMU_data.y < -3.6) && isLadder)
         {
                //transform.Translate(Vector3.up * (YSlider.value / YSlider.maxValue) * -5f );
                // transform.Translate(Vector3.up * YSlider.value);
                transform.Translate(Vector3.up * (IMU_data.y / YmaxValue) * -1f);
        }*/
         else if (IMU_data.x > -5)
         {
            //transform.eulerAngles += new Vector3(0,(XSlider.value / XSlider.maxValue) * 1.5f , 0);
            // transform.eulerAngles += new Vector3(0, XSlider.value, 0);
            transform.eulerAngles += new Vector3(0, (IMU_data.x / XmaxValue) * 1.2f, 0);

        }
         else if (IMU_data.x < 5)
         {
            //transform.rotation = Quaternion.Euler(new Vector3(0,(XSlider.value / XSlider.maxValue) * 90, 0));
            //transform.eulerAngles += new Vector3(0, (XSlider.value / XSlider.maxValue) * 1.5f, 0);
            // transform.eulerAngles += new Vector3(0, XSlider.value, 0);
            transform.eulerAngles += new Vector3(0, (IMU_data.x / XmaxValue) * 1.2f, 0);
        }

         if(isLadder && InputBridge.Instance.AButton)
        {
            transform.Translate(Vector3.up * -1f);
        }
     }

    /*void Update()
    {
         if (Input.GetKey(KeyCode.W))
         {
             //transform.rotation = Quaternion.Euler(new Vector3(0, (XSlider.value / XSlider.maxValue) * 90, 0));
             transform.Translate(Vector3.forward  * 15f * Time.deltaTime);
         }
         if (Input.GetKey(KeyCode.S))
         {
             var jump = Vector3.up * 10f * Time.deltaTime + Vector3.forward * 5f * Time.deltaTime;
             transform.Translate(jump);
         }
         if (Input.GetKey(KeyCode.A))

         {
             transform.eulerAngles += new Vector3(0,  -2, 0);

         }
         if (Input.GetKey(KeyCode.D))
         {
             //transform.rotation = Quaternion.Euler(new Vector3(0,(XSlider.value / XSlider.maxValue) * 90, 0));
             transform.eulerAngles += new Vector3(0, 2, 0);
         }*/



        // 키를 이용해서 움직이게 하는 코드
        /*if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, 3 * speed * Time.deltaTime );
        }
        if (Input.GetKey(KeyCode.S) && isLadder)
        {
            //var jump = Vector3.up * 15f * Time.deltaTime + Vector3.forward * 10f * Time.deltaTime;
            var jump = Vector3.up * 15f * Time.deltaTime;
            transform.Translate(jump);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles += new Vector3(0, -2, 0)* Time.deltaTime * 30;

        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.Euler(new Vector3(0,(XSlider.value / XSlider.maxValue) * 90, 0));
            transform.eulerAngles += new Vector3(0, 2, 0) * Time.deltaTime * 30;
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "River")
        {
            Chef_Inventory._Instance.emptyslot();
            gameObject.transform.position = new Vector3(-78, -8, 33);
            Chef_UIManager._Instance.activewarning();
            Invoke("inactivewarn", 2); //2초 뒤에 Panel이 종료될 수 있도록 함
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("ladder"))
        {
            print("OnTriggerEnter: " + other.gameObject.name);
            isLadder = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.transform.Translate(0, 0.1f, 0);
            // Debug.Log("ladderin");
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("ladder"))
        {
            print("OnTriggerExit : " + other.gameObject.name);
            this.GetComponent<Rigidbody>().useGravity = true;
            this.transform.Translate(0, 0.3f, 1.5f);
            isLadder = false;
            // print("ladderout");
        }
    }

    void inactivewarn()
    {
        Chef_UIManager._Instance.inactivewarning();
    }
}