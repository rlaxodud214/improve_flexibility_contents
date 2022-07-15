using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_choice : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject player;

    public static Bird_choice instance__;

    private void Awake()
    {
        if (instance__ == null) instance__ = this;
        else if (instance__ != null) return;
        //DontDestroyOnLoad(gameObject);
        player = Instantiate(charPrefabs[(int)Bird_DataManager.instance_.currentCharacter]);
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
