using System;
using System.Collections;
using System.Collections.Generic;
using PKR.Lobby;
using Singleton.Component;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum FocusUIType
{
    Volume,
    Control,
    CloseButton,
    Binding
}

public class SettingUI : UIBase
{
    [SerializeField] private VolumePanel volumePanel;
    [SerializeField] private ControlPanel controlPanel;
    [SerializeField] private Image closeBtnHighlight;

    private FocusUIType _activeFocusUI = FocusUIType.Volume;
    private int activeRow = 0;
    private int activeCol = 0;


    void Awake()
    {
        for (int i = 0; i < volumePanel.items.Count; i++)
            volumePanel.items[i].SetRow(i);

        for (int i = 0; i < controlPanel.items.Count; i++)
            controlPanel.items[i].SetRow(i);

        volumePanel.Init();
        controlPanel.Init();
    }

    private void OnEnable()
    {
        _activeFocusUI = FocusUIType.Volume;
        activeRow = 0;
        activeCol = 0;
        UpdateAllHighlights();
    }

    private float leftArrowTime;
    private float rightArrowTime;
    void Update()
    {
        if (activeBindingUI) return;
        // 볼륨 포커스 상태일 때, 좌우 방향키를 눌러 지속적으로 볼륨을 조절
        if (_activeFocusUI == FocusUIType.Volume)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftArrowTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (Time.time - leftArrowTime >= 0.7f)
                {
                    volumePanel.AdjustVolume(activeRow, -1);
                    UpdateAllHighlights();
                }
            }
            else
            {
                leftArrowTime = 0f;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rightArrowTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (Time.time - rightArrowTime >= 0.7f)
                {
                    volumePanel.AdjustVolume(activeRow, 1);
                    UpdateAllHighlights();
                }
            }
            else
            {
                rightArrowTime = 0f;
            }
        }
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) MoveVertical(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveVertical(+1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveHorizontal(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveHorizontal(+1);

            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (_activeFocusUI == FocusUIType.Control)
                {
                    if (activeCol == 0)
                        controlPanel.items[activeRow].StartRebind();
                    else
                        controlPanel.items[activeRow].ResetKey();
                }
                else if (_activeFocusUI == FocusUIType.CloseButton)
                {
                    OnClickCloseBtn();
                }
            }
        }
    }

    // 마우스 오버 콜백
    public void OnVolumeItemHover(int row)
    {
        _activeFocusUI = FocusUIType.Volume;
        activeRow = row;
        activeCol = 0;
        UpdateAllHighlights();
    }

    public void OnControlItemHover(int row, int col)
    {
        _activeFocusUI = FocusUIType.Control;
        activeRow = row;
        activeCol = col;
        UpdateAllHighlights();
    }

    public void OnCloseBtnHover()
    {
        _activeFocusUI = FocusUIType.CloseButton;
        activeRow = 0;
        activeCol = 0;
        UpdateAllHighlights();
    }
    public void OnClickCloseBtn()
    {
        UIManager.Instance.CloseUI(this);
    }

    // 세로 이동: 경계에서 패널 간 이동
    void MoveVertical(int dir)
    {
        if (_activeFocusUI == FocusUIType.Volume)
        {
            if (volumePanel.CanMove(activeRow, dir))
            {
                activeRow += dir;
            }
            else
            {
                // Volume 끝 -> Control 첫
                if (dir > 0)
                {
                    _activeFocusUI = FocusUIType.Control;
                    activeRow = 0;
                    activeCol = 0;
                }
            }
        }
        else if (_activeFocusUI == FocusUIType.Control)
        {
            if (controlPanel.CanMoveRow(activeRow, dir))
            {
                activeRow += dir;
            }
            else
            {
                //윗방향이면, Volume
                if (dir < 0)
                {
                    _activeFocusUI = FocusUIType.Volume;
                    activeRow = volumePanel.items.Count - 1;
                    activeCol = 0;
                }
                //아래방향이면 closebtn
                else
                {
                    _activeFocusUI = FocusUIType.CloseButton;
                    activeRow = 0;
                    activeCol = 0;
                }
            }
        }
        else
        {
            //None -> Volume 끝
            if (dir < 0)
            {
                _activeFocusUI = FocusUIType.Control;
                activeRow = controlPanel.items.Count - 1;
                activeCol = 0;
            }
        }

        UpdateAllHighlights();
    }

    // 가로 이동: 패널 내부 이동 또는 볼륨 조절/키 설정
    void MoveHorizontal(int dir)
    {
        if (_activeFocusUI == FocusUIType.Volume)
        {
            volumePanel.AdjustVolume(activeRow, dir);
        }
        else if (_activeFocusUI == FocusUIType.Control)
        {
            activeCol = Mathf.Clamp(activeCol + dir, 0, 1);
        }

        UpdateAllHighlights();
    }


    void UpdateAllHighlights()
    {
        if (_activeFocusUI == FocusUIType.Volume)
        {
            volumePanel.UpdateHighlight(activeRow);
            controlPanel.UpdateHighlight(-1, -1);
            closeBtnHighlight.gameObject.SetActive(false);
        }
        else if (_activeFocusUI == FocusUIType.Control)
        {
            volumePanel.UpdateHighlight(-1);
            controlPanel.UpdateHighlight(activeRow, activeCol);
            closeBtnHighlight.gameObject.SetActive(false);
        }
        else
        {
            volumePanel.UpdateHighlight(-1);
            controlPanel.UpdateHighlight(-1, -1);
            closeBtnHighlight.gameObject.SetActive(true);
        }
    }

    private bool activeBindingUI = false;
    public void StartRebind(BindingManager.Action action)
    {
        var ui = UIManager.Instance.OpenUI<BindingUI>();
        var trigger = ui.AddComponent<OnDestroyTrigger>();
        trigger.onDestroy += () =>
        {
            controlPanel.Init();
            activeBindingUI = false;
        };
        
        ui.StartRebind(action);
        activeBindingUI = true;
    }

    public void ResetKey(BindingManager.Action action)
    {
        BindingManager.Instance.ResetBinding(action);
        controlPanel.Init();
    }
}