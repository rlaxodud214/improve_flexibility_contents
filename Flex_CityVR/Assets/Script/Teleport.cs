using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    // timer
    private bool timer; // timer
    private float maxTime = 3; // 유지해야하는 시간
    private int minTime = 0; // 플레이어가 유지 중인 시간
    //

    // teleport
    public GameObject mPlayer; // 임시
    public List<GameObject> teleportLocation = new List<GameObject>(); // teleport 리스트
    public List<GameObject> teleportBtn = new List<GameObject>(); // teleport 와 연결할 버튼 리스트 //위와 순서 일치하게
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
        for (int i = 0; i < teleportLocation.Count; i++)
        {
            telePos.Add(teleportBtn[i].name, teleportLocation[i]);
        }
        mPlayer = GameObject.Find("Player");
        backImg.gameObject.SetActive(false);
    }

    public void teleport() // UI 버튼 함수에 연결
    {
        backImg.gameObject.SetActive(true);
        // CharacterController 꺼줘야 캐릭터가 이동함, 페이드아웃될 때 못 움직이도록 함
        Player.instance.controller_state = false;
        mPlayer.transform.GetComponent<CharacterController>().enabled = false;

        StartCoroutine(FadeOutCamera(backImg));
        StartCoroutine(changePosition());
    }

    IEnumerator FadeOutCamera(Image img) // 페이드 아웃
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함

        float fadeCount = 0; // 처음 알파값 (투명도)
        while (fadeCount < 1.0f) // 알파 최댓값 1.0까지 반복
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01s마다 실행
            img.color = new Color(0, 0, 0, fadeCount); // 해당 변수값으로 알파값 지정
        }
    }

    IEnumerator changePosition()
    {
        // 눌린 버튼의 name 가져오기
        string name = EventSystem.current.currentSelectedGameObject.name;
        wantPos = telePos[name].transform.position;

        yield return new WaitForSeconds(5f); // 캐릭터 이동이 fadeout 보다 먼저 발생하지 않도록

        mPlayer.transform.position = wantPos;
        Player.instance.controller_state = true;
        mPlayer.transform.GetComponent<CharacterController>().enabled = true;

        yield return new WaitForSeconds(3f);
        backImg.gameObject.SetActive(false);
        StartCoroutine(showTitle(telePos[name]));
    }

    IEnumerator showTitle(GameObject obj) // 장소 이름 띄우기
    {
        string num = obj.name.Substring(0, 1);
        Image nowTitle = contentsTitle.GetChild(int.Parse(num) - 1).gameObject.transform.GetComponent<Image>(); // 장소 이름

        nowTitle.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        // 끌 때 페이드 필요
        nowTitle.gameObject.SetActive(false);
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer = true;
            Timer_();
        }
    }

    public void Timer_()
    {
        if (timer == true)
        {
            maxTime += Time.deltaTime;
            if (maxTime >= 1)
            {
                minTime++;
                maxTime -= 1;
                Debug.Log("min : " + minTime);
                if (minTime == 3)
                {
                    //이벤트
                    print("버텼다!");
                    timer = false;
                }
            }
        }
        else
        {
            maxTime = 0;
            minTime = 0;
        }
    }

}
