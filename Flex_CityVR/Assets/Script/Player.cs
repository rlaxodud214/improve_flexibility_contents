using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    // Trigger 여부
    private bool T_hospital, T_soccer, T_limbo, T_kayak, T_fly, T_window, T_battle, T_chef, T_arrow;

    public static Player instance;   // 싱글톤 

    void Awake()
    {
        Player.instance = this;

        #region 초기화
        T_hospital = T_soccer = T_limbo = T_kayak = T_fly = T_window = T_battle = T_chef = T_arrow = false;
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        print("출력");
        Debug.Log("otherName : " + other.name.Substring(0,-1));

/*        if (other.gameObject.layer == 11) // Contents Layer
        {
            if (other.name.Substring(0, -1) == "T_kay") //카약
            {
                T_kayak = true;
            }
            UIManager.instance.informPanel.SetActive(true);
            UIManager.instance.informText.text = "콘텐츠 수행 장소로 이동하시겠습니까?";
        }

        UIManager.instance.checkTrigger();*/
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11) // Contents Layer
        {
            if (other.gameObject.name.Substring(0, -1) == "T_kay") //카약
            {
                T_kayak = true;
            }
            UIManager.instance.informPanel.SetActive(true);
            UIManager.instance.informText.text = "콘텐츠 수행 장소로 이동하시겠습니까?";
        }

        UIManager.instance.checkTrigger();
    }

=======
        if (other.name.Substring(0, 5) == "T_kay") //카약
        {
            T_kayak = true;
            UIManager.instance.informPanel.SetActive(true);
            UIManager.instance.informText.text = "콘텐츠 수행 장소로 이동하시겠습니까?";
        }
        UIManager.instance.checkTrigger();
    }



>>>>>>> a99a04dddc0dc358ee2236fd0b1f139f306d5cc4
}