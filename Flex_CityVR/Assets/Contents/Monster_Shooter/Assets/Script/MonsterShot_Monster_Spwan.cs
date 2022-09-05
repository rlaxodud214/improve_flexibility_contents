using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Monster_Info
{
    public string name;
    public float max_hp;
    public float current_hp;
}

public class MonsterShot_Monster_Spwan : MonoBehaviour
{
    public List<GameObject> Monster_Prefabs;
    public List<GameObject> Spawn_point;
    public GameObject Player;
    public float Spawn_Time;
    public float Position_Change_Delay;
    public int RandNum_type;
    public int RandNum_position;
    public GameObject Image_Arrow;
    public Vector3 Image_Arrow_Rotation;

    // public Monster_Info[] monster_info;

    // Start is called before the first frame update
    void Start()
    {
        Spawn_Time = 9.8f; // 실제는 시간 조정 해야함
        Position_Change_Delay = 30f;
        RandNum_type = 0;
        RandNum_position = 0;
        StartCoroutine(Scenario());
        StartCoroutine(Create());
        Image_Arrow_Rotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        switch(RandNum_position)
        {
            case 0:
                Image_Arrow_Rotation.z = 0;
                break;

            case 1:
                Image_Arrow_Rotation.z = -180;
                break;

            case 2:
                Image_Arrow_Rotation.z = 90; 
                break;

            case 3:
                Image_Arrow_Rotation.z = -90;
                break;
        }
        Image_Arrow.transform.localEulerAngles = Image_Arrow_Rotation;
    }

    IEnumerator Create()
    {
        while (true)
        {
            yield return new WaitForSeconds(Spawn_Time);
            var g = Instantiate(Monster_Prefabs[RandNum_type], Spawn_point[RandNum_position].transform);

            // 회전처리
            g.transform.LookAt(Player.transform);
            var angle = g.transform.rotation.eulerAngles;
            var angle_remove_X = new Vector3(0, angle.y, angle.z); // x축 회전값 제거
            g.transform.rotation = Quaternion.Euler(angle_remove_X); // x축 회전 값 제거한 거 적용

            // 몬스터 이동 스크립트 추가
            var M = g.AddComponent<MonsterShot_Monster>();
            var temp = M.GetComponent<MonsterShot_Monster>();
            print(Monster_Prefabs[RandNum_type].name + "몬스터를" + Spawn_point[RandNum_position].name + "에 생성");
        }
    }

    IEnumerator Scenario()
    {
        // RandNum_type = 6;
        yield return new WaitForSeconds(1.0f);
        var temp_time = 0f;
        while (Position_Change_Delay > temp_time)
        {
            // RandNum_type = Random.Range(0, 2);
            yield return new WaitForSeconds(Spawn_Time-0.2f);
            temp_time += Spawn_Time;
        }
        temp_time = 0f;
        RandNum_position = Random.Range(0, 3);

        RandNum_type = Random.Range(1, 3);
        // RandNum_type = 6;
        switch (RandNum_position)
        {
            case 0:
                RandNum_position = 1;
                break;
            case 1:
                RandNum_position = 0;
                break;
            case 2:
                RandNum_position = 3;
                break;
            case 3:
                RandNum_position = 2;
                break;
        }
        yield return new WaitForSeconds(Position_Change_Delay);

        RandNum_type = Random.Range(2, 4);
        // RandNum_type = 6;
        switch (RandNum_position)
        {
            case 0:
            case 1:
                RandNum_position = Random.Range(2, 3);
                break;
            
            case 2:
            case 3:
                RandNum_position = Random.Range(0, 1);
                break;
        }
        yield return new WaitForSeconds(Position_Change_Delay);

        RandNum_type = Random.Range(3, 5);
        // RandNum_type = 6;
        switch (RandNum_position)
        {
            case 0:
                RandNum_position = 1;
                break;
            case 1:
                RandNum_position = 0;
                break;
            case 2:
                RandNum_position = 3;
                break;
            case 3:
                RandNum_position = 2;
                break;
        }
        yield return new WaitForSeconds(Position_Change_Delay);

        for(int i=0; i<30; i++)
        {
            RandNum_type = Random.Range(4, 6);
            RandNum_position = Random.Range(0, 3);
            print("랜덤 위치에 랜덤 몬스터 생성");
            yield return new WaitForSeconds(Position_Change_Delay);
        }
    }
}
