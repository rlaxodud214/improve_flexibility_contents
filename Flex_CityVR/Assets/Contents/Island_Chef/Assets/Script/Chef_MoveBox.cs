using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chef_MoveBox : MonoBehaviour
{
    public GameObject[] BoxPrefab;
    public GameObject[] SpawnBox;
    public int[] randArray;
    public Vector3 a;
    public int randNum;


    static public double eucladianDist(GameObject A, GameObject B)
    {
        // var dists = Math.Sqrt((Math.Pow(A.transform.position.x - B.transform.position.x, 2) + (Math.Pow(A.transform.position.y - B.transform.position.y, 2))));
        var dists = Vector3.Distance(A.transform.position, B.transform.position);
        return dists;
    }

    void Start()
    {
        a = new Vector3(0, 6.2f, 0);
        SpawnBox = GameObject.FindGameObjectsWithTag("RespawnBox");
        randArray = new int[BoxPrefab.Length]; // SpawnBox의 index

        randArray[0] = UnityEngine.Random.Range(0, SpawnBox.Length); // randNum = 0~30;
        GameObject spawnPos = SpawnBox[randArray[0]];
        Instantiate(BoxPrefab[0], spawnPos.transform.position + a, BoxPrefab[0].transform.rotation);
        bool flag;
        //Instantiate(BoxPrefab[0], SpawnBox[0].transform.position + a, BoxPrefab[0].transform.rotation);
        for (int i = 1; i < BoxPrefab.Length; ++i) // BoxPrefab.Length = 10
        {
            flag = true;
            while (flag)
            {
                var j = 0;
                bool check = true;
                var dist = 0.0;
                randNum = UnityEngine.Random.Range(0, SpawnBox.Length); // randNum = 0~30
                do
                {
                    dist = eucladianDist(SpawnBox[randNum], SpawnBox[randArray[j]]);
                    if (dist < 9)
                    {
                        check = false;
                    }
                    j++;
                } while (j < i);

                if (check)
                {
                    randArray[i] = randNum; // SpawnBox의 인덱스
                    spawnPos = SpawnBox[randArray[i]];
                    Instantiate(BoxPrefab[i], spawnPos.transform.position + a, BoxPrefab[i].transform.rotation);
                    flag = false;
                }
            }
        }
    }
}



        /*if (Array.IndexOf(randArray, randNum) == -1) // randNum이 randArray 안에 있는 경우 1을 반환하는 함수
        {
            randArray[i] = randNum; 
            GameObject spawnPos = SpawnBox[randArray[i]];
            Instantiate(BoxPrefab[i], spawnPos.transform.position + a, BoxPrefab[i].transform.rotation);
            break;
        }*/

        /*for (int i = 0; i < randArray.Length; i++)
        {
            GameObject spawnPos = SpawnBox[randArray[i]];
            Instantiate(BoxPrefab[i], spawnPos.transform.position + a, BoxPrefab[i].transform.rotation);
        }*/
    

