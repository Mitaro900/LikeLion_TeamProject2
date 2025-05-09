using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Simple Singleton

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private GameObject Root;
    [SerializeField] private NormalStageUI _normalStageUI;
    [SerializeField] private BossStageUI _bossStageUI;
    [SerializeField] private BindingUI _bindingUI;
    [SerializeField] private SettingUI _settingUI;

    private Dictionary<string, UIBase> dic = new Dictionary<string, UIBase>();
    private Dictionary<string, UIBase> openedDic = new Dictionary<string, UIBase>();
    private Stack<UIBase> stack = new Stack<UIBase>();

    private void Init()
    {
        dic.Add($"{typeof(NormalStageUI).Name}", _normalStageUI);
        dic.Add($"{typeof(BossStageUI).Name}", _bossStageUI);
        dic.Add($"{typeof(BindingUI).Name}", _bindingUI);
        dic.Add($"{typeof(SettingUI).Name}", _settingUI);
    }

    public UIBase GetUI<T>() where T : UIBase
    {
        string name = typeof(T).Name;
        if (openedDic.ContainsKey(name))
            return openedDic[name];

        return null;
    }

    public T OpenUI<T>() where T : UIBase
    {
        string name = typeof(T).Name;
        UIBase ui = GameObject.Instantiate(dic[name]);
        stack.Push(ui);
        openedDic.Add(name, ui);
        ui.transform.SetParent(Root.transform);
        var rt = ui.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        return ui as T;
    }

    public void CloseUI(UIBase ui)
    {
        if (stack.Count == 0) return;
        if (stack.Peek() != ui) return;
        CloseUI();
    }

    public void CloseUI()
    {
        if (stack.Count == 0) return;
        UIBase ui = stack.Pop();
        openedDic.Remove(ui.GetType().Name);
        GameObject.Destroy(ui.gameObject);
    }

}