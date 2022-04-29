using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XR_Rig_position_move_by_keyboard : MonoBehaviour
{
    public GameObject XR_Rig;
    public float value;

    private void Awake()
    {
        //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
    }
    void Start()
    {
        value = 0.05f;
    }

    void Update()
    {
        var offset = Vector3.zero;
        var position = XR_Rig.transform.position;
        // 위쪽 방향키
        if (Input.GetKey(KeyCode.UpArrow))
        {
            offset = new Vector3(0, 0, value);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            offset = new Vector3(0, 0, -value);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            offset = new Vector3(value, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            offset = new Vector3(-value, 0, 0);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            offset = new Vector3(0, value, 0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            offset = new Vector3(0, -value, 0);
        }
        XR_Rig.transform.position = position + offset;
    }
}
