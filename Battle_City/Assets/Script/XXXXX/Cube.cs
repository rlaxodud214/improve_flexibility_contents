using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cube : MonoBehaviour
{
    [SerializeField] GameObject m_goPrefab = null;
    [SerializeField] float m_force = 0f;
    [SerializeField] Vector3 m_offset = Vector3.zero;
    private float startTime = Time.time;

    public static Cube instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start() 
    {
        Explosion();
        SoundManager.instance.BombSound();
        Invoke("Restart", 6f);
    }
    public void Explosion()
    {
        GameObject t_clone = Instantiate(m_goPrefab, transform.position, Quaternion.identity);
        Rigidbody[] t_rigids = t_clone.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i<t_rigids.Length;i++)
        {
            t_rigids[i].AddExplosionForce(m_force, transform.position + m_offset, 10f);
        }
        gameObject.SetActive(false);
    }

    void Restart()
    {   
        SceneManager.LoadScene("Main");
    }

}
