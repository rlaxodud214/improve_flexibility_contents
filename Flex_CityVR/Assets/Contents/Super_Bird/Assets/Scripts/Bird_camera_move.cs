using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_camera_move : MonoBehaviour
{
    public Transform target;
    private Transform tr; //카메라 자신의 트렌지폼
    private float speed = 5f;

    private void Awake(){  }
    void Start()
    {
        tr = GetComponent<Transform>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update() { }

    //public float offsetX = -10f;
    public float offsetY = 20f;
    public float offsetZ = 0f;

    Vector3 cameraPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        cameraPosition.x = target.position.x;
        cameraPosition.y = target.position.y + offsetY;
        cameraPosition.z = target.position.z + offsetZ;

        tr.position = Vector3.Lerp(tr.position, cameraPosition, speed * Time.deltaTime); //너프로 갈 수록 이동이 느려지게 해서 부드러운 이동이 가능하게 함

    }
}
