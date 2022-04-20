using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPun, IPunObservable
{
    //public GameObject mPlayer;

    #region Public Fields

    public static Player instance;   // 싱글톤
    public static GameObject LocalPlayerInstance;
    public GameObject playerUIPrefab;

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
        /*if (!PlayerPrefs.HasKey("pos_X")) // 게임 첫 플레이시 시작 지점
        {
            PlayerPrefs.SetFloat("pos_X", -44f);
            PlayerPrefs.SetFloat("pos_Y", 2f);
            PlayerPrefs.SetFloat("pos_Z", 22f);
        }*/
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();
        if (cameraWork != null)
        {
            if (photonView.IsMine)
            {
                cameraWork.OnStartFollowing();
            }
        }
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component를 찾을 수 없습니다.");

        // 플레이어 인스턴스 파악
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(LocalPlayerInstance);
     
    }

    void Start()
    {
        /*if (!PlayerPrefs.HasKey("pos_X")) // 게임 첫 플레이시 시작 지점
        {
            PlayerPrefs.SetFloat("pos_X", -44f);
            PlayerPrefs.SetFloat("pos_Y", 2f);
            PlayerPrefs.SetFloat("pos_Z", 22f);
        }*/

        if (playerUIPrefab != null)
        {
            GameObject playerUI = Instantiate(playerUIPrefab);
            playerUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);    // 위의 인스턴스에게 메시지 전송
            DontDestroyOnLoad(playerUI);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUI 프리팹을 찾을 수 없습니다.", this);
        }

    }

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

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

                // 옆으로 걷는 모션 추가?

                
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
            }
            moveDir.y -= gravity * Time.deltaTime;
            _controller.Move(moveDir * Time.deltaTime);
        }
        else
            Debug.Log("텔레포트 중에는 움직일 수 없습니다.");
    }

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 클라이언트가 보내는 입장일 때
        if (stream.IsWriting)
        {

        }
        // 클라이언트가 받는 입장일 때
        else
        {

        }
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

    #endregion
}