using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public GameObject mPlayer;
    private Animator animator = null;
    private CharacterController _controller = null;
    private float mvspeed = 6f;
    private float rotspeed = 100f;
    private float gravity = 20f;
    private Vector3 moveDir = Vector3.zero;
    public static Player instance;   // 싱글톤 

    // Start is called before the first frame update

    void Awake()
    {
        Player.instance = this;
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
