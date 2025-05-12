using Singleton.Component;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonComponent<GameManager>
{
    #region Singleton
    protected override void AwakeInstance()
    {
        Initialize();
    }

    protected override bool InitInstance()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        return true;
    }

    protected override void ReleaseInstance()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(gameObject);
    }
    #endregion

    private void OnEnable()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.StopSFX();
        UIManager.Instance.CloseUI();

        if (scene.name == "Stage1-1")
        {
            UIManager.Instance.OpenUI<NormalStageUI>();
        }
        else if(scene.name == "Stage1_Boss")
        {
            UIManager.Instance.OpenUI<BossStageUI>();
        }
    }

    public void HitPenalty()
    {
        UIManager.Instance.GetUI<NormalStageUI>()?.AddScore(-50);
        UIManager.Instance.GetUI<NormalStageUI>()?.AdjustComboTimer(-2.5f);
        UIManager.Instance.GetUI<BossStageUI>()?.DamagedPlayerHp();
    }
}
