using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 대포알에 들어가있는 컴포넌트
public class Bullet : MonoBehaviour
{

    #region Public Fields

    public static Bullet instance;

    public float speed;
    public bool start;

    #endregion

    #region Private Fields

    private Transform tr;


    #endregion 

    #region MonoBehaviour Callbacks

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update() {
        speed = Battle_Fire.instance.speed;
        
        if(speed >= 20)
            speed = 20f;

        tr.Translate(Vector3.forward * speed * Time.deltaTime);
        if (gameObject.activeSelf)
            Battle_Fire.instance.LineRenderer_position.Add(gameObject.transform.position);
    }   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("목표 격추");
            gameObject.SetActive(false);
            float criticalRate = Random.RandomRange(1f, 3f);
            Battle_UIManager.instance.DecreaseLife("enemy", 35f * criticalRate);
            
            // 적 체력이 0 이하
            if (Battle_UIManager.instance.enemyLife <= 0)
            {
                Battle_Effect.instance.Explosion();
                Battle_UIManager.instance.IsVictory("Success");
            }
            else
            {
                Battle_Effect.instance.Shot();
                Battle_UIManager.instance.IsVictory("Hit");
            }
            
            Battle_Fire.instance.Line_Renderer.enabled = false;    // 목표 격추 시 이전의 라인렌더러는 사라짐
        }
        else if (collision.collider.CompareTag("Collider"))
        {
            Debug.Log("안맞음, 맞은 물체: " + collision.gameObject.name);
            gameObject.SetActive(false);
            Battle_UIManager.instance.DecreaseLife("player", 10);
            Battle_UIManager.instance.IsVictory("Fail");
            Battle_Fire.instance.Line_Renderer.enabled = true;
            Battle_Fire.instance.removeBullet();
        }
/*        else
        {
            Debug.Log("안맞음, 맞은 물체: " + collision.gameObject.name);
        }*/
    }

    #endregion

}


