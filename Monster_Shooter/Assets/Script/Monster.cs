using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    public GameObject slider;
    public float movespeed;
    public float hp;
    public GameObject HP_Bar;

    void Start()
    {
        Player = GameObject.Find("XR Rig");
        movespeed = 1.5f;
        hp = 100;
        HP_Bar = transform.GetChild(transform.childCount-1).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var position = Player.transform.position + Vector3.down * 0f;
        // transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime*movespeed);
        transform.position = Vector3.MoveTowards(transform.position, position, movespeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("몬스터와 충돌한 오브젝트 이름 : " + collision.gameObject.name);
        if (collision.gameObject.name == "활")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.name == "Arrow2(Clone)")
        {
            print("명중~");
            hp -= 30;
            var scale = HP_Bar.transform.localScale;
            HP_Bar.transform.localScale = new Vector3(scale.x - 0.3f, scale.y, scale.z);
            if (hp < 0)
            {
                print("몬스터 삭제");
                Destroy(gameObject);
            }
            else
            {
                print(gameObject.name + " 몬스터 유지 HP : " + hp);
                print("localScale : " + HP_Bar.transform.localScale);
            }
                

            print("화살 삭제");
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
}