using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
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

    }
}
