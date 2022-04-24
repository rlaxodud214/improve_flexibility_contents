using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BNG;
using Button = UnityEngine.UI.Button;

public class Teleport : MonoBehaviour
{
    // teleport
    public GameObject XR_Rig; // 임시

    public GameObject teleportLocation;
    public Transform teleportBtn;

    private Dictionary<string, GameObject> telePos = new Dictionary<string, GameObject>();
    private Vector3 wantPos;
    public Transform contentsTitle;
    //

    //fadeout
    public Image backImg;
    //

    public static Teleport instance;   // 싱글톤 

    void Awake()
    {
        Teleport.instance = this;
        //timer = false;

        // 버튼 이름과 teleportLocation 매치
        // 눌린 버튼의 이름을 가져와 딕셔너리에서 값을 찾을거임

        for (int i=0; i < teleportBtn.childCount; i++)
        {
            telePos.Add(teleportBtn.GetChild(i).name, teleportLocation.transform.GetChild(i).gameObject);
        }

        backImg.gameObject.SetActive(false); 
    }

    private void Start()
    {
        XR_Rig = GameManager.instance.XR_Rig;
    }

    IEnumerator FadeInCamera(Image img, float fadeInTime) // 페이드 인 : 투명 > 불투명
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함

        // 투명하게 초기화
        img.gameObject.SetActive(true);
        Color temp = img.color;
        temp.a = 0;
        img.color = temp;
        //
        float t = 0f; // 0~1 일때 t=0; 0.5~1일때 t=0.5 와 같이 선형보간 값 구함
        temp.a = Mathf.Lerp(0f, 1f, t); // 투명~불투명

        while (temp.a < 1f)
        {
            t += Time.deltaTime / fadeInTime;
            temp.a = Mathf.Lerp(0f, 1f, t);
            img.color = temp;
            yield return null;
        }
    }

    IEnumerator FadeOutCamera(Image img, float fadeOutTime) // 페이드 아웃 : 불투명 > 투명
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함

        // 불투명하게 초기화
        if (img.gameObject.activeSelf == false)
            img.gameObject.SetActive(true);
        Color temp = img.color;
        temp.a = 255;
        img.color = temp;
        //

        float t = 0f;
        temp.a = Mathf.Lerp(1f, 0f, t); // 불투명~투명

        while (temp.a > 0f)
        {
            t += Time.deltaTime / fadeOutTime;
            temp.a = Mathf.Lerp(1f, 0f, t);
            img.color = temp;
            yield return null;
        }
        img.gameObject.SetActive(false);
    }


    public void Doteleport() // UI 버튼 함수에 연결
    {
        backImg.gameObject.SetActive(true);
        // CharacterController 꺼줘야 캐릭터가 이동함, 페이드아웃될 때 못 움직이도록 함
        Player.instance.controller_state = false;
        XR_Rig.transform.GetChild(0).GetComponent<CharacterController>().enabled = false;

        StartCoroutine(FadeInCamera(backImg, 3f));
        StartCoroutine(changePosition());
    }

    IEnumerator changePosition()
    {
        // 눌린 버튼의 name 가져오기
        string name = EventSystem.current.currentSelectedGameObject.name;
        wantPos = telePos[name].transform.position;

        yield return new WaitForSeconds(4f); // 캐릭터 이동이 fadeout 보다 먼저 발생하지 않도록

        XR_Rig.transform.position = wantPos;
        Player.instance.controller_state = true;
        XR_Rig.transform.GetChild(0).GetComponent<CharacterController>().enabled = true;

        yield return new WaitForSeconds(2f);
        backImg.gameObject.SetActive(false);
        StartCoroutine(showTitle(telePos[name]));
    }

    IEnumerator showTitle(GameObject obj) // 장소 이름 띄우기
    {
        yield return new WaitForSeconds(1f);
        string num = obj.name.Substring(0, 1); // 텔레포트 장소의 번호 가져오기
        //print("장소 이름 index : " + (int.Parse(num) - 1));
        Image nowTitle = contentsTitle.GetChild(int.Parse(num) - 1).gameObject.transform.GetComponent<Image>(); // 장소 이름

        // title Fade In/Out
        StartCoroutine(FadeInCamera(nowTitle, 2f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCamera(nowTitle, 2f));
    }
}
