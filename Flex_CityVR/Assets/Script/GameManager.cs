using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject XR_Rig;

    public static GameManager instance;   // 싱글톤 

    private void Awake()
    {
        GameManager.instance = this;
        XR_Rig = GameObject.Find("XR Rig");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region 카메라 Rig 조정
        if (Input.GetKey(KeyCode.UpArrow))
            XR_Rig.transform.position += new Vector3(0, 0, 0.01f);
        else if (Input.GetKey(KeyCode.DownArrow))
            XR_Rig.transform.position -= new Vector3(0, 0, 0.01f);
        else if (Input.GetKey(KeyCode.LeftArrow))
            XR_Rig.transform.position -= new Vector3(0.01f, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow))
            XR_Rig.transform.position += new Vector3(0.01f, 0, 0);
        else if (Input.GetKey(KeyCode.Alpha1))
            XR_Rig.transform.position += new Vector3(0, 0.01f, 0);
        else if (Input.GetKey(KeyCode.Alpha2))
            XR_Rig.transform.position -= new Vector3(0, 0.01f, 0);
        #endregion
    }
}
