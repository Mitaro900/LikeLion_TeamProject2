using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingUI : UIBase
{
    private BindingManager.Action action;

    public void StartRebind(BindingManager.Action action)
    {
        this.action = action;
        StartCoroutine(DetectKey());
    }

    IEnumerator DetectKey()
    {
        yield return null; //엔터눌러서 들어올경우, 다음프레임으로 넘김
        while (!Input.anyKeyDown) yield return null;
        foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kc))
            {
                BindingManager.Instance.Rebind(action, kc);
                UIManager.Instance.CloseUI(this);
                break;
            }
        }
    }
}