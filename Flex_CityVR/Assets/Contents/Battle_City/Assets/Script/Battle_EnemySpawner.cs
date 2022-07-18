using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Battle_EnemySpawner : MonoBehaviour
{
    public static Battle_EnemySpawner instance;   // 싱글톤

    public Transform player;
    public GameObject enemyPrefab;
    public BoxCollider range;

    Vector3 randomPos;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = GameObject.Find("PlayerTank").transform;
        RandomPoint(range, out randomPos);
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        enemy.transform.LookAt(player);
    }

    public void InstantiateEnemy()
    {
        RandomPoint(range, out randomPos);
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        enemy.transform.LookAt(player);
    }

    bool RandomPoint(BoxCollider rangeCollider, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = ReturnRandomPos(rangeCollider);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;

        return false;
    }

    Vector3 ReturnRandomPos(BoxCollider rangeCollider)
    {
        Vector3 originPos = rangeCollider.transform.position;

        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 randomPos = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPos = originPos + randomPos;

        return respawnPos;
    }
}
