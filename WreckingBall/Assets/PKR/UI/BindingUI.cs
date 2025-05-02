using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PKR.Lobby
{
    public class BindingUI : MonoBehaviour
    {
        [SerializeField]SettingUI settingUI;
        private BindingManager.Action action;

        public void StartRebind(BindingManager.Action action)
        {
            this.action = action;
            StartCoroutine(DetectKey());
        }

        IEnumerator DetectKey()
        {
            yield return null;//엔터눌러서 들어올경우, 다음프레임으로 넘김
            while (!Input.anyKeyDown) yield return null;
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    BindingManager.Instance.Rebind(action, kc);
                    settingUI.EndRebind();
                    break;
                }
            }
        }
    }
}