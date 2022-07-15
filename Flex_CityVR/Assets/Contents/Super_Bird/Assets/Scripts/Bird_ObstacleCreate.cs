using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_ObstacleCreate : MonoBehaviour
{
    public static Bird_ObstacleCreate _instance_;

    public List<GameObject> ObstaclePool = new List<GameObject>();
    public GameObject[] obstacles;
    public int count = 1;
    public float create = 5.4f;

    private void Awake()
    {
        if (_instance_ == null) _instance_ = this;


        for (int i = 0;i < obstacles.Length; i++)
        {
            for(int q = 0; q < count; q++)
            {
                ObstaclePool.Add(CreateObstacle(obstacles[i], transform));
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Create());
    }

    int DeactiveObstacle()
    {
        List<int> num = new List<int>();
        for (int i = 0; i < ObstaclePool.Count; i++)
        {
            if (!ObstaclePool[i].activeSelf)
            {
                num.Add(i);
            }
        }
        return num[Random.Range(0, num.Count)];
    }

    GameObject CreateObstacle(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }

    IEnumerator Create()
    {
        while (true)
        {
            var UI = Bird_UIManagerGame._instance;
            if (UI.timerStart)
            {
                if (UI.playtime % 20f < create) // 0~24 < 6.5
                {
                    if(create > 3.0f)
                    {
                        create = create - 0.4f;
                    }
                    print("creat : " + create);
                }
            }

            ObstaclePool[DeactiveObstacle()].SetActive(true);
            //print("UIMangerGame._instance.playtime : " + UIMangerGame._instance.playtime.ToString("N1"));
            yield return new WaitForSeconds(create);
        }
    }
}
