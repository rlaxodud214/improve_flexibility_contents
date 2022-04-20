using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;
    public float speed;
    public int life;
    //public bool shoot = Fire.instance.getShoot();
    private Transform tr;
    public bool start;

    public static Bullet instance;

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
        life = 10;
    }

    void Update() {
        speed = Fire.instance.speed;
        
        if(speed >= 20)
            speed = 20f;

        tr.Translate(Vector3.forward * speed * Time.deltaTime);
        Fire.instance.LineRenderer_position.Add(gameObject.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("목표 격추");
            UIManager.instance.EnemyLife = UIManager.instance.EnemyLife - 10;
            gameObject.SetActive(false);
            ChangeSceneBreak();
            Fire.instance.Line_Renderer.enabled = false;
        }

        if (collision.collider.CompareTag("Collider"))
        {
            gameObject.SetActive(false);
            Debug.Log("안맞음");
            UIManager.instance.life = UIManager.instance.life - 10;
            Invoke("FailPanel", 1f);
            Fire.instance.Line_Renderer.enabled = true;
        }
        Fire.instance.removeBullet();
    }

    void ChangeSceneBreak()
    {
        // SceneManager.LoadScene("BreakEnemy");
        Fire.instance.fireActive = false;
        Effect.instance.Explosion();
        Invoke("VictoryPanel", 2f);
        Enemy.instance.turretDeactive();
        Invoke("turretActive", 5.5f);
    }

    // void ChangeSceneMiss()
    // {
    //     Fire.instance.fireActive = true;
    //     Fire.instance.changeCamera = false;
    //     //Effect.instance.Notshot();
    // }

    void turretActive()
    {
        Enemy.instance.turretActive();
    }

    void VictoryPanel()
    {
        UIManager.instance.Victory.SetActive(true);
    }

    void FailPanel()
    {
        UIManager.instance.Fail.SetActive(true);
    }

}


