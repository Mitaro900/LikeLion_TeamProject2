using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public List<ControlItem> items;

    public bool CanMoveRow(int row, int dir)
    {
        return row + dir >= 0 && row + dir < items.Count;
    }

    // SettingUI의 엔터 호출
    public void OnSubmit(int row, int col)
    {
        if (col == 0) items[row].StartRebind();
        else items[row].ResetKey();
    }

    // SettingUI에서 호출
    public void UpdateHighlight(int selectedRow, int selectedCol)
    {
        for (int i = 0; i < items.Count; i++)
            items[i].SetSelected(i == selectedRow, selectedCol);
    }

    private BindingManager km => BindingManager.Instance;
    private BindingManager.Action waitingFor = (BindingManager.Action)(-1);

    public void Init()
    {
        foreach (var ui in items)
        {
            // 1) 초기 UI 세팅
            ui.labelText.text = ui.action.ToString().ToUpper();
            ui.bindingButtonText.text = km.GetKey(ui.action).ToString();
        }
    }

    private void StartRebind(ControlItem ui)
    {
        // 이미 대기 중이면 무시
        if (waitingFor != (BindingManager.Action)(-1)) return;

        waitingFor = ui.action;
        ui.bindingButtonText.gameObject.SetActive(false);
        StartCoroutine(DetectKey(ui));
    }


    private IEnumerator DetectKey(ControlItem ui)
    {
        // 키 입력 대기
        while (!Input.anyKeyDown)
            yield return null;

        // 눌린 키 감지
        foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kc))
            {
                km.Rebind(ui.action, kc);
                ui.bindingButtonText.text = kc.ToString();
                break;
            }
        }

        // 완료 처리
        ui.bindingButtonText.gameObject.SetActive(true);
        waitingFor = (BindingManager.Action)(-1);
    }
}