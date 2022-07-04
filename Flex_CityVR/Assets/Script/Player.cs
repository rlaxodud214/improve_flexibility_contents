using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /*    //public GameObject mPlayer;

        #region Public Fields

        public static Player instance;   // 싱글톤

        public Vector3 lastPosition; // 플레이어 마지막 위치

        public bool controller_state;

        #endregion


        #region Private Fields

        private Animator animator = null;
        private CharacterController _controller = null;

        private Vector3 moveDir = Vector3.zero;

        private float mvspeed = 6f;
        private float rotspeed = 100f;
        private float gravity = 20f;
        [SerializeField]
        private bool isStandingJump;

        #endregion



        #region MonoBehaviour Callbacks

        void Awake()
        {
            Player.instance = this;
            controller_state = true;
            if (!PlayerPrefs.HasKey("pos_X")) // 게임 첫 플레이시 시작 지점
            {
                PlayerPrefs.SetFloat("pos_X", -44f);
                PlayerPrefs.SetFloat("pos_Y", 2f);
                PlayerPrefs.SetFloat("pos_Z", 22f);
            }
            animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
        }
        void Start()
        {
            if (!PlayerPrefs.HasKey("pos_X")) // 게임 첫 플레이시 시작 지점
            {
                PlayerPrefs.SetFloat("pos_X", -44f);
                PlayerPrefs.SetFloat("pos_Y", 2f);
                PlayerPrefs.SetFloat("pos_Z", 22f);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (controller_state == true)
            {
                if (_controller.isGrounded == true)
                {
                    if (!isStandingJump)
                    {
                        float Rot_speed = rotspeed * Time.deltaTime; // 회전 속도
                        float horizontalInput = Input.GetAxisRaw("Horizontal");
                        float VerticalInput = Input.GetAxisRaw("Vertical");
                        transform.Rotate(Vector3.up * horizontalInput * Rot_speed);

                        moveDir = new Vector3(0, 0, VerticalInput * mvspeed);
                        moveDir = transform.TransformDirection(moveDir);


                        if (Input.GetKey(KeyCode.W)) // 달리기
                        {
                            animator.SetBool("IsRun", true);
                        }
                        else
                            animator.SetBool("IsRun", false);
                    }

                    if (Input.GetKey(KeyCode.Space))    // 점프
                    {
                        animator.SetTrigger("IsJump");
                        isStandingJump = true;  // 제자리 점프 할 때에는 캐릭터 이동하지 못하도록
                    }
                    else
                        animator.ResetTrigger("IsJump");

                    // 달리는 중에 점프를 했다면 -> isStandingJump를 false로 만듦
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running-Jump"))
                        isStandingJump = false;

                    // 제자리 점프를 했지만 점프 애니메이션이 모두 끝났다면 -> isStandingJump를 false로 만듦
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standing-Jump") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        isStandingJump = false;
                    //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Standing-Jump"));
                }
                moveDir.y -= gravity * Time.deltaTime;
                _controller.Move(moveDir * Time.deltaTime);
            }
            else
                Debug.Log("텔레포트 중에는 움직일 수 없습니다.");
        }

        #endregion

        #region Public Methods

        // 플레이어의 마지막 위치 저장
        public void SavePosition()
        {
            //lastPosition = this.transform.position;
            PlayerPrefs.SetFloat("pos_X", this.transform.position.x);
            PlayerPrefs.SetFloat("pos_Y", this.transform.position.y);
            PlayerPrefs.SetFloat("pos_Z", this.transform.position.z);
        }

        #endregion*/ // 까지 2022.04.14 주석 - XR Rig에 사용 불가

    // timer
    public bool timer; // timer
    private float maxTime = 2; // 유지해야하는 시간
    private int minTime = 0; // 플레이어가 유지 중인 시간
    //

    private string objectName;
    //public Vector3 Offset;  //캐릭터 사용시
    public GameObject CenterEyeAnchor;

    // 캐릭터 컨트롤러
    public bool controller_state;

    // Cntents Trigger 입장 여부
    public Dictionary<string, bool> dic_contents = new Dictionary<string, bool>()
    {
        {"T_hospital", false},{"T_soccer", false},{"T_limbo", false},{"T_kayak", false},
        {"T_fly", false},{"T_window", false},{"T_battle", false},{"T_chef", false},{"T_arrow", false},
    };

    public static Player instance;   //싱글톤 

    void Awake()
    {
        Player.instance = this;

        #region 초기화
        controller_state = true;
        objectName = "";
        #endregion
    }

    private void Update()
    {
        //캐릭터 사용시
        /*transform.position = CenterEyeAnchor.transform.position + Offset;
        transform.eulerAngles = new Vector3(0f, GameManager.instance.XR_Rig.transform.GetChild(0).rotation.eulerAngles.y, 0f);*/
        Timer_();
        //print("nowPortal : "+nowPortal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11) // 모든 콘텐츠의 Trigger는 Contents Layer 로 할 것
        {
            objectName = other.name.Substring(0, 5);
            timer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11) // 모든 콘텐츠의 Trigger는 Contents Layer 로 할 것
        {
            timer = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11) // 모든 콘텐츠의 Trigger는 Contents Layer 로 할 것
        {
            objectName = other.transform.name.Substring(0, 5);
            timer = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 11) // 모든 콘텐츠의 Trigger는 Contents Layer 로 할 것
        {
            timer = false;
        }
    }

    public void PortalInform()  // 포탈 이름 확인해서 해당 변수 true로 변경
    {
        UIManager.instance.setInformType(1); //알림창 type = Contents로 변경

        if (objectName == "T_kay") dic_contents["T_kayak"] = true;              //카약
        else if (objectName == "T_hos") dic_contents["T_hospital"] = true;      //병원
        else if (objectName == "T_soc") dic_contents["T_soccer"] = true;        //골키퍼
        else if (objectName == "T_lim") dic_contents["T_limbo"] = true;         //림보
        else if (objectName == "T_fly") dic_contents["T_fly"] = true;           //건물 피하기
        //else if (objectName == "T_win") dic_contents["T_window"] = true;        //창문 닦기
        else if (objectName == "T_bat") dic_contents["T_battle"] = true;     //포트리스
        else if (objectName == "T_che") dic_contents["T_chef"] = true;         //음식만들기
        else if (objectName == "T_arr") dic_contents["T_arrow"] = true;       //활쏘기
        UIManager.instance.informPanel.SetActive(true);
        UIManager.instance.informText.text = "콘텐츠 수행 장소로 이동하시겠습니까?"; //informtype contetns일때
        print("이용 중인 포탈은 : " + KeySearch());
    }


    public void Timer_()
    {
        if (timer == true){
            maxTime += Time.deltaTime;
            if (maxTime >= 1){
                minTime++;
                maxTime -= 1;
                //Debug.Log("min : " + minTime);
                if (minTime == 2){
                    //이벤트
                    Debug.Log("<color=cyan>포탈에 2초 머물렀습니다.</color>");
                    PortalInform();
                    timer = false;
                }
            }
        }
        else {
            maxTime = 0;
            minTime = 0;
        }
    }

    public string KeySearch()
    {
        // 해당 value 값을 찾는 key 반환
        string key;
        key = dic_contents.FirstOrDefault(x => x.Value == true).Key;
        return key;
    }
}