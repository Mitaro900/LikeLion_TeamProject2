using System.Collections;
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
    Jump,           //점프
    RopeStart,      //로프 발사
    RopeSuccess,    //로프 붙음
    Roll,           //구르는소리
    Brake,          //가속 제동
    Hit,            //피격음
    BlockDestroy,   //블록 부셔지는소리
    TakeDown,       //내려찍기
}

//PlayerPrefs에 "BGM",  "SFX"로, float 0~1f로 저장됨.
//SettingUI에서 Slider는 0~100 범위를 가짐.
public class SoundManager : SingletonComponent<SoundManager>
{
    [SerializeField] private AudioSource bgm;
    public AudioSource BGM => bgm;
    [SerializeField] private AudioSource sfx;
    public AudioSource SFX => sfx;
    [SerializeField] private List<AudioClip> bgmClips;
    [SerializeField] private List<AudioClip> sfxClips;

    private Coroutine repeatingSfxCo = null;

    #region Singleton
    protected override void AwakeInstance()
    {
        Initialize();
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
    #endregion

    private void OnEnable()
    {
        if (Instance != this)
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

    public void PlaySFXRepeating(SfxTrack track, float intervalTime)
    {
        repeatingSfxCo = StartCoroutine(RepeatingSFX(track, intervalTime));
    }

    public void StopSFX()
    {
        StopCoroutine(repeatingSfxCo);
        repeatingSfxCo = null;
    }

    private IEnumerator RepeatingSFX(SfxTrack track, float intervalTime)
    {
        while (true)
        {
            sfx.PlayOneShot(sfxClips[(int)track]);
            yield return new WaitForSeconds(intervalTime);
        }
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