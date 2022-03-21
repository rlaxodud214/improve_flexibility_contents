using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public GameObject mPlayer;
    private Animator animator = null;
    public bool controller_state;
    private CharacterController _controller = null;
    private float mvspeed = 6f;
    private float rotspeed = 100f;
    private float gravity = 20f;
    private Vector3 moveDir = Vector3.zero;
    public Vector3 lastPosition; // 플레이어 마지막 위치
    public static Player instance;   // 싱글톤 

    // Start is called before the first frame update

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
                float Rot_speed = rotspeed * Time.deltaTime; // 회전 속도
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float VerticalInput = Input.GetAxisRaw("Vertical");
                transform.Rotate(Vector3.up * horizontalInput * Rot_speed);

                moveDir = new Vector3(0, 0, VerticalInput * mvspeed);
                moveDir = transform.TransformDirection(moveDir);
                #region 플레이어 애니메이션

                if (Input.GetKey(KeyCode.W)) // 달리기
                {
                    animator.SetBool("IsRun", true);
                }
                else
                    animator.SetBool("IsRun", false);

                // 옆으로 걷는 모션 추가?

                if (Input.GetKey(KeyCode.Space))
                    animator.SetBool("IsJump", true);
                else
                    animator.SetBool("IsJump", false);

                #endregion
            }
            moveDir.y -= gravity * Time.deltaTime;
            _controller.Move(moveDir * Time.deltaTime);
        }
        else
            Debug.Log("텔레포트 중에는 움직일 수 없습니다.");
    }

    // 플레이어의 마지막 위치 저장
    public void SavePosition()
    {
        //lastPosition = this.transform.position;
        PlayerPrefs.SetFloat("pos_X", this.transform.position.x);
        PlayerPrefs.SetFloat("pos_Y", this.transform.position.y);
        PlayerPrefs.SetFloat("pos_Z", this.transform.position.z);
    }
}
