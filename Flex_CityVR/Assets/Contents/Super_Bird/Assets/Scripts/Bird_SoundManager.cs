using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird_SoundManager : MonoBehaviour
{
    public static Bird_SoundManager instance;
    public AudioSource audio;
    public AudioClip Moveclip;

    public AudioSource BackVolume; 
    public AudioSource effectVolume;

    public Slider BackSlider;
    public Slider EffectSlider;

/*    public string[] source_name;
    public AudioClip[] source_clip;
    public Dictionary<string, AudioClip> source;*/

    Image thisImg;

    private void Awake() //인스턴스 생성
    {
        if (instance == null)
        {
            instance = GetComponent<Bird_SoundManager>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
/*        for(int i=0; i<source_clip.Length; i++)
        {
            source.Add(source_name[i], source_clip[i]);
        }*/
    }

    void Start()
    {     
        audio = gameObject.GetComponent<AudioSource>();

        BackSlider.value = PlayerPrefs.GetFloat("BackVolume");
        EffectSlider.value = PlayerPrefs.GetFloat("EffectVolume");

        BackVolume.volume = BackSlider.value;
        effectVolume.volume = EffectSlider.value;  
    }

    // Update is called once per frame
    void Update(){ }

    public void BackChangeVolume(float volume) //배경음 슬라이더 볼륨 조절
    {
        BackVolume.volume = volume;
        PlayerPrefs.SetFloat("BackVolume", volume);
    }
    public void EffectChangeVolume(float volume) //효과음 슬라이더 볼륨 조절
    {
        effectVolume.volume = volume;
        PlayerPrefs.SetFloat("EffectVolume", volume);
    }

    public void EffectOn() 
    {
        effectVolume.Play();
    }

    public void playoneshot()
    {
        audio.volume = effectVolume.volume;
        //audio.clip = Moveclip;
        audio.PlayOneShot(Moveclip);
    }

    /*public void playoneshot(string name)
    {
        audio.volume = effectVolume.volume;
        audio.PlayOneShot(source["name"]);
    }*/
}
