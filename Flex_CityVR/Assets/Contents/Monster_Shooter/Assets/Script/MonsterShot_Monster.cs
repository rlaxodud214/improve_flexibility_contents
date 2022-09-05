using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterShot_Monster : MonoBehaviour
{
    public GameObject Player;
    public GameObject slider;
    public float movespeed;
    public float monster_hp; // 고정값
    public float hp; // 수치 변경값
    public GameObject HP_Bar_White;
    public GameObject HP_Bar;
    public TextMeshPro[] Damage_text;
    public bool[] Damage_text_is_using;
    public Animator Anim;
    public float damage;
    public bool Avoiding_duplicate_runs; // HP_Down_Animation 코루틴 중복 실행 방지용 불 변수
    public bool isdead;

    // 싱글톤 패턴
    #region Singleton
    private static MonsterShot_Monster _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static MonsterShot_Monster Instance
    {
        get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
    }

    void Awake()
    {
        _Instance = this;
    }
    #endregion

        void Start()
    {
        isdead = false;
        Player = GameObject.Find("XR Rig");
        movespeed = 2.7f; // 실제?
        // movespeed = 20.0f; // 테스트

        var monster_name = transform.name;
        var value = 0;
        switch (monster_name.Substring(0, 3))
        {
            case "Rat":
                value = 100;
                break;

            case "Cra":
                value = 150;
                break;

            case "Liz":
                value = 200;
                break;

            case "Wer":
                value = 250;
                break;

            case "Spe":
                value = 300;
                break;

            case "Bla":
                value = 350;
                break;

            case "Fyl":
                value = 400;
                break;
        }

        monster_hp = value;
        hp = value;
        damage = 0;
        HP_Bar_White = transform.GetChild(transform.childCount - 1).gameObject;
        HP_Bar = HP_Bar_White.transform.GetChild(0).gameObject;
        Damage_text_is_using = new bool[10];
        Damage_text = new TextMeshPro[5];
        for (int i = 0; i < Damage_text.Length; i++)
        {
            Damage_text[i] = HP_Bar_White.transform.GetChild(1).GetChild(i).transform.GetComponent<TextMeshPro>();
            Damage_text_is_using[i] = false;
        }
            
        Anim = transform.GetComponent<Animator>();
        Avoiding_duplicate_runs = false;
    }

    // Update is called once per frame
    void Update()
    {
        // print(Time.timeScale);
        var position = Player.transform.position + Vector3.down * 0f;
        // transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * movespeed);
        transform.position = Vector3.MoveTowards(transform.position, position, movespeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("몬스터와 충돌한 오브젝트 이름 : " + collision.gameObject.name);
       /* if (collision.gameObject.name == "활")
        {
            MonsterShot_UIManager.Instance.HpDown(); // HP 감소
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }*/

        if (collision.gameObject.name == "XR Rig")
        {
            if(isdead == false)
            {
                MonsterShot_UIManager.Instance.HpDown(); // HP 감소
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.name == "Arrow2(Clone)")
        {
            // 04/27 : 화살의 스피드 == 데미지 적용
            var Arrow_Script = collision.transform.GetComponent<Arrow>();

            // 데미지가 너무 비슷해서 랜덤으로 조정되게 변경함
            var rand = Random.Range(0.90f, 1.15f);

            if (MonsterShot_Gamemanager.Instance.isLotation)
                damage = Arrow_Script.ZVel * rand;
            else
                damage = 110;

            // 데미지 출력 함수
            Damage_text_UI();
            print("명중~");
            hp -= damage;
            
            if (hp <= 0)
            {
                StartCoroutine(Monster_Die());
            }
            else
            {
                StartCoroutine(HP_Down_Animation());
                print(gameObject.name + " 몬스터 유지 HP : " + hp);
                print("localScale : " + HP_Bar.transform.localScale);
            }
            print("화살 삭제");
            Destroy(collision.gameObject);
        }
    }
    void Damage_text_UI()
    {
        var index = 0;
        for(int i = 0; i < Damage_text.Length; i++)
        {
            if (Damage_text_is_using[i] == false)
            {
                index = i;
                break;
            }
        }

        Color color = Damage_text[index].color;
        color.a = 1f;
        Damage_text[index].color = color;
        Damage_text[index].text = damage.ToString("N0");
        Damage_text_is_using[index] = true;
        StartCoroutine(down_Opacity(index));
    }

    // 데미지 텍스트 투명도 0.1씩 감소시켜서 사라지는 효과
    IEnumerator down_Opacity(int index)
    {
        print("down_Opacity()");
        Color color = Damage_text[index].color;
        var count = 20;
        for(int i = 1; i <= count; i++)
        {
            color.a = 1f - (i / count);
            Damage_text[index].color = color;
            yield return new WaitForSeconds(0.03f);
        }
        Damage_text_is_using[index] = false;
        Damage_text[index].text = "";
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    IEnumerator HP_Down_Animation()
    {
        print("맞음");
        Anim.SetTrigger("GetHit");
        yield return new WaitForSeconds(0.2f);
        Anim.SetTrigger("Walk");

        // 체력바 감소 lerp 선형보간 사용해서 부드럽게 줄어들게 변경하기
        // count != 200는 무한루프 방지를 위한 보험같은 것
        var count = 0;
        var scale_backup = HP_Bar.transform.localScale.x;
        while (count != 150)
        {
            var scale = HP_Bar.transform.localScale;
            var Target_value = hp / monster_hp;
            var lerp_value = Mathf.Lerp(scale.x, Target_value, Time.deltaTime * 5f);
            
            if (Target_value <= 0 || HP_Bar.transform.localScale.x <= 0)
            {
                HP_Bar.transform.localScale = new Vector3(0, scale.y, scale.z);
                break;
            }

            print(scale_backup.ToString("N3") + "에서" + Target_value.ToString("N3") + "로 lerp 시킴" + ", 현재 : " + lerp_value.ToString("N3"));
            HP_Bar.transform.localScale = new Vector3(lerp_value, scale.y, scale.z);

            // 선형 보간값으로는 정확히 원하는 수치와 같아질 수 없어서 93%이상 변경되면 100%로 변경시키고 넘어가게 함
            // scale.x : 현재 값
            // scale_backup - (damage / hp) : 감소하며 도달해야 하는 값
            if (scale.x <= Target_value + (scale_backup * 0.04f / (monster_hp / 100) ) ) 
            {
                print("if문 통과함 -> count : " + count);
                HP_Bar.transform.localScale = new Vector3(Target_value, scale.y, scale.z);
                break;
            } 
            yield return new WaitForSeconds(0.01f);
            count++;
        }
        yield return null;
    }

    IEnumerator Monster_Die()
    {
        isdead = true;
        Anim.SetTrigger("Die");
        print("몬스터 삭제");
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}