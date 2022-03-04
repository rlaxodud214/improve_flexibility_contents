using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class PhoneControl : MonoBehaviour
{
    public GameObject mPlayer; // 임시

    // phone
    private bool phone_on;
    private Animator animator;
    public GameObject Phone;
    public List<GameObject> toTeleport = new List<GameObject>(); // teleport 리스트
    public List<GameObject> teleportBtn = new List<GameObject>(); // teleport 와 연결할 버튼 리스트
    public Dictionary<string, GameObject> telePos = new Dictionary<string, GameObject>();
    private Vector3 wantPos;
    //

    //fadeout
    public Image balckImg;
    //

    // Start is called before the first frame update

    void Awake()
    {
        // 버튼 이름과 toTeleport 매치
        // 눌린 버튼의 이름을 가져와 딕셔너리에서 값을 찾을거임
        for (int i = 0; i < toTeleport.Count; i++)
        {
            telePos.Add(teleportBtn[i].name, toTeleport[i]);
        }
        mPlayer = GameObject.Find("Player");
        animator = Phone.transform.GetComponent<Animator>();
        balckImg.gameObject.SetActive(false);

        #region 폰 초기화
        phone_on = false;
        Phone.SetActive(false);
        Phone.transform.GetChild(1).gameObject.SetActive(true);
        Phone.transform.GetChild(2).gameObject.SetActive(false);
        #endregion
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // 핸드폰 켜기/끄기
        {
            if (phone_on) // 켜져 있으면
            {
                animator.SetBool("phone_on", false);
                //animator.SetFloat("Reverse", -1.0f);
                phone_on = false;
                Phone.SetActive(false);
                Phone.transform.GetChild(1).gameObject.SetActive(true);
                Phone.transform.GetChild(2).gameObject.SetActive(false);

            }
            else // 꺼져 있으면
            {
                animator.SetBool("phone_on", true);
                //animator.SetFloat("Reverse", 1.0f);
                phone_on = true;
                Phone.SetActive(true);
                Phone.transform.GetChild(1).gameObject.SetActive(true);
                Phone.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public void teleport()
    {
        StartCoroutine(FadeCoroutine());
        StartCoroutine(changePosition());
    }

    IEnumerator FadeCoroutine()
    {
        // 알파값 (투명도) 는 인스펙터에서 0 ~ 255  -->  투명 ~ 불투명
        // 코드 상으로 0 ~ 1로 지정해야함
        balckImg.gameObject.SetActive(true);

        // CharacterController 꺼줘야 캐릭터가 이동함, 페이드아웃될 때 못 움직이도록 함
        mPlayer.transform.GetComponent<CharacterController>().enabled = false; 
        
        float fadeCount = 0; // 처음 알파값 (투명도)
        while (fadeCount < 1.0f) // 알파 최댓값 1.0까지 반복
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01s마다 실행
            balckImg.color = new Color(0, 0, 0, fadeCount); // 해당 변수값으로 알파값 지정
        }
    }

    IEnumerator changePosition()
    {
        // 눌린 버튼의 name 가져오기
        string name = EventSystem.current.currentSelectedGameObject.name;
        wantPos = telePos[name].transform.position;

        yield return new WaitForSeconds(5f); // 캐릭터 이동이 fadeout 보다 먼저 발생하지 않도록

        mPlayer.transform.position = wantPos;
        mPlayer.transform.GetComponent<CharacterController>().enabled = true;

        yield return new WaitForSeconds(3f);
        balckImg.gameObject.SetActive(false);
    }
}
