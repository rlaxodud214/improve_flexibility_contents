using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_SoundManager : MonoBehaviour
{
    //public AudioSource BombSource;
    //public AudioSource FireSource;
    //public AudioSource MissSource;
    public AudioSource Battle_EffectADS;
    public AudioSource backgroundADS;
    public AudioClip BombClip;
    public AudioClip FireClip;
    public AudioClip MissClip;
    public static Battle_SoundManager instance;
    

    void Awake(){
        if(Battle_SoundManager.instance == null)
        {
            Battle_SoundManager.instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }
    
    public void BombSound(){
        Battle_EffectADS.PlayOneShot(BombClip);
    }

    public void FireSound(){
        Battle_EffectADS.PlayOneShot(FireClip);
    }

    public void MissSound(){
        Battle_EffectADS.PlayOneShot(MissClip);
    }
}
