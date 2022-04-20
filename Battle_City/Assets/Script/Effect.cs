using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    //터지는 이펙트 구현.
    

    public GameObject goPrefab = null;
    public float force = 0f;
    public Vector3 offset = Vector3.zero;
    public float startTime;
    public int levelChange = 1;
    
    

    private Rigidbody rigid;
    private Transform tr;


    public static Effect instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        startTime = Time.time;
    }

    void Update()
    {

    }

    // public void Notshot() { // 피해가는 모션 
    //     SoundManager.instance.MissSound();
    //     tr = GetComponent<Transform>();
    //     tr.position = new Vector3(1,4,-4);
    //     rigid = GetComponent<Rigidbody>();
    //     rigid.velocity = new Vector3(0, 0, 10f);
    // }

    public void Explosion() // 팡 터지는 모션
    {
        var Enemy = GameObject.Find("Enemy");
        SoundManager.instance.BombSound();
        GameObject t_clone = Instantiate(goPrefab, Enemy.transform.position, Quaternion.identity);
        Destroy(t_clone, 5.5f);
        Rigidbody[] t_rigids = t_clone.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i<t_rigids.Length;i++)
        {
            t_rigids[i].AddExplosionForce(force, transform.position + offset, 10f);
        }
        gameObject.SetActive(false);
        levelChange++;
    }
}
