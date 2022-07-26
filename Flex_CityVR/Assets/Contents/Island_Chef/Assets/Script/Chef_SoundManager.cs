using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef_SoundManager : MonoBehaviour
{
    public static Chef_SoundManager instance = null;
    public AudioSource bgSound;
    public AudioSource SFX;
    public AudioSource Button;

    private void Awake()  //싱글톤 생성
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    /*public void ADSplay(string ADSName, AudioClip clip)
    {
        GameObject go = new GameObject(ADSName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }*/
    void Start()
    {
        bgSound.volume = PlayerPrefs.GetFloat("bgSound");   //저장해놓은 배경 볼륨값 설정
        //SFX.volume = PlayerPrefs.GetFloat("effect");        //저장해놓은 효과음 볼륨값 설정 
        Button.volume = PlayerPrefs.GetFloat("effect");     //저장해놓은 효과음 볼륨값 설정 
    }
    public void BgmSlider(float volume) // Slider로 움직인 값을 bgSound의 volume 값으로 지정
    {
        bgSound.volume = volume;
        PlayerPrefs.SetFloat("bgSound", bgSound.volume); //마지막 배경 볼륨값을 프리팹에 저장
    }

    public void effectSlider(float volume)
    {
        Button.volume = volume;
        //SFX.volume = volume;
        PlayerPrefs.SetFloat("effect", SFX.volume); //마지막 효과음 볼륨값을 프리팹에 저장
    }
    public void PlaySFX() // 점프 효과음 출력
    {
        SFX.Play();
    }

    public void PlayButton()    //버튼 클릭음 출력
    {
        Button.Play();
    }
}
