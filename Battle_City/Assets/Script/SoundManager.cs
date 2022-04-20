using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BombSource;
    public AudioSource FireSource;
    public AudioSource MissSource;
    public AudioClip BombClip;
    public AudioClip FireClip;
    public AudioClip MissClip;
    public static SoundManager instance;
    

    void Awake(){
        if(SoundManager.instance == null)
        {
            SoundManager.instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }
    
    public void BombSound(){
        BombSource.PlayOneShot(BombClip);
    }

    public void FireSound(){
        FireSource.PlayOneShot(FireClip);
    }

    public void MissSound(){
        MissSource.PlayOneShot(MissClip);
    }
}
