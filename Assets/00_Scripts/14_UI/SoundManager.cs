using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // 배경음악
    public AudioSource environmentSource; //환경음악
    public AudioSource sfxSource; // 소리 이펙트
    public AudioClip[] bgmSorces; // 배경음악 모음

    public static SoundManager Instance;

    private int currentSceneIndex; // 현재 씬 인덱스

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void EnterSoundSetting()
    {
        UIManager.Instance.OpenUI(UIKey.SoundUI);
    }

    public void ExitSoundSetting()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        UIManager.Instance.CloseUI();
        if(currentSceneIndex == 2)
        {
            CharacterManager.Instance.Player.controller.ToggleCursor(false);
        }
    }

    public void PlaySFXSource()
    {
        sfxSource.Play();
    }
}