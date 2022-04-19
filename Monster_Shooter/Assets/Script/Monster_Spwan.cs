using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Monster_Info
{
    public string name;
    public float max_hp;
    public float current_hp;
}

public class Monster_Spwan : MonoBehaviour
{
    public List<GameObject> Monster_Prefabs;
    public List<GameObject> Spawn_point;
    public GameObject Player;
    public float Spawn_Time;
    public float Position_Change_Delay;
    public int RandNum_type;
    public int RandNum_position;

    // public Monster_Info[] monster_info;

    // Start is called before the first frame update
    void Start()
    {
        /*monster_info[0].name = "WerewolfPBRDefault";
        monster_info[0].name = "WerewolfPBRDefault";
        monster_info[0].name = "WerewolfPBRDefault";*/
        Spawn_Time = 15f;
        Position_Change_Delay = 30f;
        RandNum_type = 0;
        RandNum_position = 0;
        StartCoroutine(Create());
        StartCoroutine(Sinario());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Create()
    {
        while (true)
        {
            yield return new WaitForSeconds(Spawn_Time);
            // 2022-03-11 : 랜덤 몬스터 & 랜덤 위치 였는데 시나리오 적용하면서 주석처리함
            // RandNum_type = Random.Range(0, Monster_Prefabs.Count);
            // RandNum_position = Random.Range(0, Spawn_point.Count);

            var g = Instantiate(Monster_Prefabs[RandNum_type], Spawn_point[RandNum_position].transform);

            // 회전처리
            g.transform.LookAt(Player.transform);
            var angle = g.transform.rotation.eulerAngles;
            var angle_remove_X = new Vector3(0, angle.y, angle.z); // x축 회전값 제거
            g.transform.rotation = Quaternion.Euler(angle_remove_X); // x축 회전 값 제거한 거 적용

            // 몬스터 이동 스크립트 추가
            var M = g.AddComponent<Monster>();
            M.hp = 100;

            // 콜라이더 크기가 몬스터 마다 달라서 몬스터 프리팹에 콜라이더 추가해버림
            /*            // 캡슐 콜라이더 추가
                        g.AddComponent<CapsuleCollider>();

                        // 캡슐콜라이더 설정
                        var C = g.GetComponent<CapsuleCollider>();
                        C.height = 2;
                        C.center = new Vector3(0, 0.8f, 0);*/

            print(Monster_Prefabs[RandNum_type].name + "몬스터를" + Spawn_point[RandNum_position].name + "에 생성");
        }
    }

    IEnumerator Sinario()
    {
        var temp_time = 0f;
        while(Position_Change_Delay > temp_time)
        {
            RandNum_type = Random.Range(0, 2);
            yield return new WaitForSeconds(Spawn_Time-0.2f);
            temp_time += Spawn_Time;
        }
        temp_time = 0f;
        RandNum_position = 0;
        print("위에 생성");

        RandNum_type = Random.Range(1, 3);
        RandNum_position = 1;
        print("아래에 생성");
        yield return new WaitForSeconds(Position_Change_Delay);

        RandNum_type = Random.Range(2, 4);
        RandNum_position = 2;
        print("좌측에 생성");
        yield return new WaitForSeconds(Position_Change_Delay);

        RandNum_type = Random.Range(3, 5);
        RandNum_position = 3;
        print("우측에 생성");
        yield return new WaitForSeconds(Position_Change_Delay);

        RandNum_type = Random.Range(0, 6);
        RandNum_position = Random.Range(0, 3);
        print("랜덤 위치에 랜덤 몬스터 생성");
    }
}
