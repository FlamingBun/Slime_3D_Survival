using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioType
{
    BGM,
    Environment,
    SFX
}
public class SettingSound : MonoBehaviour
{
    SoundManager soundManager;
    AudioSource bgmSource;
    AudioSource environmentSource;
    AudioSource sfxSource;

    public AudioClip bgm;
    public AudioClip environment;
    public AudioClip sfx;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // bgm 변경
        bgmSource = soundManager.bgmSource;
        bgmSource.Stop();
        bgmSource.clip = bgm;
        soundManager.bgmSorces[currentSceneIndex] = bgm;
        bgmSource.Play();

        // 환경음 변경
        ChangeSound(AudioType.Environment, soundManager.environmentSource, environment,false);
       
        // 소리 이펙트 변경
        ChangeSound(AudioType.SFX, soundManager.sfxSource, sfx,false);
    } 
 
    // 음악 변경
    public void ChangeSound(AudioType audioType, AudioSource audioSource, AudioClip audioClip, bool isPlay)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                bgmSource = audioSource;
                break;
            case AudioType.Environment:
                environmentSource = audioSource;
                break;
            case AudioType.SFX:
                sfxSource = audioSource;
                break;
        }

        audioSource.clip = audioClip;

        if (isPlay && audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }
    // 소리 이펙트 재생
    public void SfxPlay()
    {
        ChangeSound(AudioType.SFX, soundManager.sfxSource, sfx, true);
    }
    // 환경 소리 재생
    public void EnvironmentPlay()
    {
        ChangeSound(AudioType.Environment, soundManager.environmentSource, environment, true);
    }
    // 환경 소리 재생 멈춤
    public void EnvironmentStop()
    {
        if(environmentSource == null) { return; }
        if (environmentSource.isPlaying)
        {
            environmentSource.Stop();
        }
    }
}
