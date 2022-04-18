using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BNG;

[System.Serializable]
public class PhoneControl : MonoBehaviour
{
    // phone
    private bool phone_on; // 핸드폰 on/off 여부
    private Animator animator; //핸드폰 애니메이터 (추후 핸드폰 up/down 애니메이션)
    public GameObject Phone;
    private bool startPhone; // 컨트롤러 A 버튼 입력 bool 변수
    private bool canCall; // 연속된 입력으로 코루틴이 연달아 실행되지 않도록 제어하는 bool 변수
    // XR Rig
    private GameObject XR_Rig;

    // Start is called before the first frame update

    void Awake()
    { 
        animator = Phone.transform.GetComponent<Animator>();

        #region 폰 초기화
        phone_on = false;
        Phone.SetActive(false);
        Phone.transform.GetChild(1).gameObject.SetActive(true);
        Phone.transform.GetChild(2).gameObject.SetActive(false);
        startPhone = false;
        canCall = true;
        #endregion
    }
    void Start()
    {
        XR_Rig = GameManager.instance.XR_Rig.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputBridge.Instance.AButton) {
            startPhone = true;
        }
        if (Input.GetKeyDown(KeyCode.Tab) || InputBridge.Instance.AButton) // 핸드폰 켜기/끄기
        {
            if (startPhone && canCall) {
                canCall = false;
                StartCoroutine(phoneCon());
            }
        }
    }

    IEnumerator delay() {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator phoneCon()
    {
        if (phone_on) // 켜져 있으면
        {
            phone_on = false;
            //print("끔1.  phone_on : " + phone_on);
            //animator.SetBool("phone_on", false);
            //animator.SetFloat("Reverse", -1.0f);
            //animator.SetBool("phone_off", true);
            Phone.SetActive(false);
            Phone.transform.GetChild(1).gameObject.SetActive(true);
            Phone.transform.GetChild(2).gameObject.SetActive(false);
            //print("끔2.  phone_on : " + phone_on);
            startPhone = false;
            yield return new WaitForSeconds(2f);
        }
        else // 꺼져 있으면
        {
            phone_on = true;
            //print("켬1.  phone_on : " + phone_on);
            //animator.SetBool("phone_on", true);
            //animator.SetBool("phone_off", false);
            //animator.SetFloat("Reverse", 1.0f);
            Phone.SetActive(true);
            Phone.transform.GetChild(1).gameObject.SetActive(true);
            Phone.transform.GetChild(2).gameObject.SetActive(false);
            //print("켬2.  phone_on : " + phone_on);
            startPhone = false;
            yield return new WaitForSeconds(2f);
        }
        startPhone = false;
        canCall = true;
    }
}
