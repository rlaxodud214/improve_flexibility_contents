using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // public LpmsTest Lpms;

    // Start is called before the first frame update
    void Start()
    {
        // Lpms.Excute();
        // Load 하기 전에  LPMS 연결을 끊고 해야함 중요!!!!!!!!!
        System.Diagnostics.Process.Start(Application.persistentDataPath+ "/Contents/골키퍼게임/Designteam_Game.exe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
