using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Effect : MonoBehaviour
{

    //터지는 이펙트 구현.
    public static Battle_Effect instance;

    public GameObject goPrefab = null;
    public GameObject explosion;
    public float force = 0f;
    public Vector3 offset = Vector3.zero;
    public float startTime;
    
    private Rigidbody rigid;
    private Transform tr;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        startTime = Time.time;
    }

    // public void Notshot() { // 피해가는 모션 
    //     Battle_SoundManager.instance.MissSound();
    //     tr = GetComponent<Transform>();
    //     tr.position = new Vector3(1,4,-4);
    //     rigid = GetComponent<Rigidbody>();
    //     rigid.velocity = new Vector3(0, 0, 10f);
    // }

    public void Explosion() // 팡 터지는 모션
    {
        var enemy = GameObject.Find("Enemy(Clone)");
        Battle_SoundManager.instance.BombSound();
        GameObject t_clone = Instantiate(goPrefab, enemy.transform.position, Quaternion.identity);
        Destroy(t_clone, 5.5f);
        Rigidbody[] t_rigids = t_clone.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i<t_rigids.Length;i++)
        {
            t_rigids[i].AddExplosionForce(force, transform.position + offset, 10f);
        }
        enemy.SetActive(false);
    }

    public void Shot()
    {
        var enemy = GameObject.Find("Enemy(Clone)");
        Battle_SoundManager.instance.BombSound();
        GameObject clonedBattle_Effect = Instantiate(explosion, enemy.transform.position, Quaternion.identity);
        Destroy(clonedBattle_Effect, 2f);
    }
}
