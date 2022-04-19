using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar_Control : MonoBehaviour
{
    public List<Transform> obj;
    public List<GameObject> hp_bar;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("XR Rig").GetComponent<Camera>();
        for (int i = 0; i < obj.Count; i++)
        {
            hp_bar[i].transform.position = obj[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            hp_bar[i].transform.position = camera.WorldToScreenPoint(obj[i].position + new Vector3(0, 1f, 0));
        }
    }
}