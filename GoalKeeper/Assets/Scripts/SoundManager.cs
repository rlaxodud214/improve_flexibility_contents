using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource effectADS;
    public AudioClip btnClick;
    
    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<SoundManager>();
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
