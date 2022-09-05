using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShot_Bow_Arrow : MonoBehaviour
{
    public GameObject bow; // 활
    public GameObject check; // 화살생성위치
    public GameObject arrow_prefabs; // 화살 프리팹

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Arrow"))
        {
            var a = Instantiate(arrow_prefabs, check.transform);
            Invoke("Destroy", 30f);
        }
    }
    void Destroy()
    {
        Destroy(this);
    }
}
