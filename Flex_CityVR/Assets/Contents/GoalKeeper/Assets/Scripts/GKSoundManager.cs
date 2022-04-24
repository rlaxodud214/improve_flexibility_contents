using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GKSoundManager : MonoBehaviour
{
    public AudioSource effectADS;
    public AudioClip btnClick;
    
    public static GKSoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<GKSoundManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EffectSoundPlay()
    {
        effectADS.PlayOneShot(btnClick);
    }
    
    public void GameStart()
    {
        SceneManager.LoadScene("GoalKeeper");
    }
    
}
