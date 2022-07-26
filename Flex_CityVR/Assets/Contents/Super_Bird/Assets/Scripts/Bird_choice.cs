using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_choice : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject player;
    public GameObject XR_Rig, EyeAnchor;
    public static Bird_choice instance__;

    private void Awake()
    {
        if (instance__ == null) instance__ = this;
        else if (instance__ != null) return;
        //DontDestroyOnLoad(gameObject);
        player = Instantiate(charPrefabs[(int)Bird_DataManager.instance_.currentCharacter]);
        //player = Instantiate(charPrefabs[0]); //임시
        XR_Rig.transform.parent = player.transform;
        player.transform.position = new Vector3(0, 0, -17);
    }
    /* void Start()
     {
         GameObject.FindWithTag("Player").AddComponent<CameraWork>();
         player.AddComponent<CameraWork>();
     }
     void Update()
     {
         var p = camera.transform.position;
         p = new Vector3(p.x, player.transform.position.y, p.z);
     }*/
}
