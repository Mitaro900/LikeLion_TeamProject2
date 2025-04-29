using System;
using System.Collections;
using Singleton.Component;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : SingletonComponent<SceneLoader>
{
    private Animator anim;
    private Canvas canvas;

    private string sceneName;

    private int sceneStart = Animator.StringToHash("sceneStart");
    private int sceneEnd = Animator.StringToHash("sceneEnd");

    protected override void AwakeInstance()
    {
        var loader = Resources.Load<GameObject>("SceneLoader");
        GameObject go = GameObject.Instantiate(loader);
        go.name = "SceneLoader";
        go.transform.SetParent(transform);

        anim = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        
        //로비씬에서는 Fade애님을 보여주지 않음
        anim.Play(sceneStart, 0, 1f);
    }

    protected override bool InitInstance()
    {
        return true;
    }

    protected override void ReleaseInstance()
    {
    }

    public void LoadScene(string loadSceneName)
    {
        if (isLoading)
        {
            Debug.Log("로딩중");
            return;
        }

        sceneName = loadSceneName;
        StartCoroutine(LoadSceneCo());
    }

    private bool isLoading = false;

    IEnumerator LoadSceneCo()
    {
        isLoading = true;
        canvas.gameObject.SetActive(true);
        
        anim.Play(sceneEnd, 0, 0f);
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        yield return null;
        
        var info = anim.GetCurrentAnimatorStateInfo(0);
        while (info.normalizedTime < 1f)
        {
            yield return null;
            info = anim.GetCurrentAnimatorStateInfo(0);
        }

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        anim.Play(sceneStart, 0, 0f);
        yield return null;
        
        info = anim.GetCurrentAnimatorStateInfo(0);
        while (info.normalizedTime < 1f)
        {
            yield return null;
            info = anim.GetCurrentAnimatorStateInfo(0);
        }
        
        canvas.gameObject.SetActive(false);
        isLoading = false;
    }
}