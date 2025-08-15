
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : BaseUI
{
    protected override UIKey uiKey => UIKey.SoundUI;
    
    public Slider backGroundSoundBar; // 옵션 사운드 오브젝트
    public Slider enviornmentMusicBar; // 환경 사운드 오브젝트
    public Slider soundEffectBar; //  이펙트 사운드 바 오브젝트
    
    protected override void Initialize()
    {
        base.Initialize();
        
        SetBGMVolume(backGroundSoundBar, SoundManager.Instance.bgmSource.volume);
        SetBGMVolume(enviornmentMusicBar, SoundManager.Instance.environmentSource.volume);
        SetBGMVolume(soundEffectBar, SoundManager.Instance.sfxSource.volume);
        
        backGroundSoundBar.onValueChanged.AddListener(SetBGMVolume); //설정 붙여주기
        enviornmentMusicBar.onValueChanged.AddListener(SetEnvironmentVolume); //설정 붙여주기
        soundEffectBar.onValueChanged.AddListener(SetSFXVolume); //설정 붙여주기
    }

    private void Update()
    {
        backGroundSoundBar.value = SoundManager.Instance.bgmSource.volume;
        enviornmentMusicBar.value = SoundManager.Instance.environmentSource.volume;
        soundEffectBar.value = SoundManager.Instance.sfxSource.volume;
    }
    

    private void SetBGMVolume(float value)
    {
        SoundManager.Instance.bgmSource.volume = value;
    }
    
    private void SetBGMVolume(Slider slider,float value)
    {
        slider.minValue = 0.0001f;
        slider.maxValue = 1f;
        slider.value = value;
    }
    
    

    private void SetEnvironmentVolume(float value)
    {
        SoundManager.Instance.environmentSource.volume = value;
    }

    private void SetSFXVolume(float value)
    {
        SoundManager.Instance.sfxSource.volume = value;
    }
}