using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public bool spawn = false;
    [Range(0.2f,10)]
    public float interval = 1;
    float timeDelta = 0f;
    private void Update()
    {
        if (spawn)
        {
            timeDelta += Time.deltaTime;
            if (timeDelta > interval)
            {
                timeDelta = 0;
                GameObject.Instantiate(spawnObjects[Random.Range(0,spawnObjects.Length)], transform.position, Quaternion.identity);
            }
        }
    }
}
