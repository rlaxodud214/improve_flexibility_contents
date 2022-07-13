using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour                                
{
    #region Public Fields

    public static Enemy instance;   // 싱글톤

    public BoxCollider RangeCollider;
    public Transform Cannon;
    public Transform player;

    //public float life;
    public bool refreshPos;

    #endregion

    #region Private Fields

    NavMeshAgent nav;   // NavMeshAgent 인스턴스
    Vector3 point;  // NavMesh 안의 랜덤위치

    #endregion

    #region MonoBehavior Callbacks

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        player = GameObject.Find("PlayerTank").transform;
        RangeCollider = GameObject.Find("RangeCollider").GetComponent<BoxCollider>();

        transform.eulerAngles = Vector3.zero;
        refreshPos = false;
        nav = GetComponent<NavMeshAgent>();
        point = transform.position; // 초기값은 지정된 위치
    }

    void Update()
    {
        if (refreshPos)
        {
            if (!RandomPoint(RangeCollider, out point))
            {
                Debug.LogError("Enemy.cs: 랜덤 위치 받아오지 못함");
            }
            refreshPos = false;
        }
        
        nav.SetDestination(point);

        Cannon.LookAt(player);
    }

    #endregion

    #region Public Methods


    #endregion

    #region Private Methods

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

    void FollowTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 dir = target.position = this.transform.position;
            this.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    #endregion
}
