using System.Collections.Generic;
using Singleton.Component;
using UnityEngine;

public enum BgmTrack
{
    Test1,
    Test2
}

public enum SfxTrack
{
    Test1,
    Test2,
}

//PlayerPrefs에 "BGM",  "SFX"로, float 0~1f로 저장됨.
//SettingUI에서 Slider는 0~100 범위를 가짐.
public class SoundManager : SingletonComponent<SoundManager>
{
    public AudioSource BGM => bgm;
    public AudioSource SFX => sfx;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private List<AudioClip> bgmClips;
    [SerializeField] private List<AudioClip> sfxClips;

    protected override void AwakeInstance()
    {
        Initialize();
    }

    void OnEnable()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    protected override bool InitInstance()
    {
        bgm.loop = true;
        sfx.loop = false;
        float bgmVol = PlayerPrefs.GetFloat("BGM", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFX", 1f);
        bgm.volume = bgmVol;
        sfx.volume = sfxVol;
        return true;
    }

    protected override void ReleaseInstance()
    {
        Destroy(gameObject);
    }

    public void PlayBGM(BgmTrack track)
    {
        bgm.clip = bgmClips[(int)track];
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlaySFX(SfxTrack track)
    {
        sfx.PlayOneShot(sfxClips[(int)track]);
    }

    public void SetSFXVolume(float volume)
    {
        sfx.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void SetBGMVolume(float volume)
    {
        bgm.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGM", volume);
    }

    public float GetSFXVolume()
    {
        return sfx.volume;
    }

    public float GetBGMVolume()
    {
        return bgm.volume;
    }
}