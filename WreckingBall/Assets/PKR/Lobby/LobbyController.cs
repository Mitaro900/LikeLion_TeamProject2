using System;
using System.Collections.Generic;
using PKR;
using PKR.Lobby;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private ButtonSelector gameStartBtn;
    [SerializeField] private ButtonSelector settingsBtn;
    [SerializeField] private ButtonSelector gameExitBtn;

    private enum MenuType
    {
        GameStart,
        Settings,
        GameExit
    }

    private MenuType curMenu;
    private Dictionary<MenuType, ButtonSelector> menuDict = new Dictionary<MenuType, ButtonSelector>();

    private void Awake()
    {
        menuDict.Add(MenuType.GameStart, gameStartBtn);
        menuDict.Add(MenuType.Settings, settingsBtn);
        menuDict.Add(MenuType.GameExit, gameExitBtn);
    }

    private void Start()
    {
        curMenu = MenuType.GameStart;
        gameStartBtn.SetSelected(true);

        gameStartBtn.onSelected = () => SelectMenu(MenuType.GameStart);
        settingsBtn.onSelected = () => SelectMenu(MenuType.Settings);
        gameExitBtn.onSelected = () => SelectMenu(MenuType.GameExit);

        gameStartBtn.onClicked = () => ClickMenu(MenuType.GameStart);
        settingsBtn.onClicked = () => ClickMenu(MenuType.Settings);
        gameExitBtn.onClicked = () => ClickMenu(MenuType.GameExit);
    }

    private void Update()
    {
        if (activeSettingUI) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (curMenu == MenuType.GameStart) return;
            SelectMenu((MenuType)((int)curMenu - 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (curMenu == MenuType.GameExit) return;
            SelectMenu((MenuType)((int)curMenu + 1));
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            ClickMenu(curMenu);
        }
    }


    private void SelectMenu(MenuType type)
    {
        menuDict[curMenu].SetSelected(false);
        curMenu = type;
        menuDict[curMenu].SetSelected(true);
    }

    private void ClickMenu(MenuType type)
    {
        switch (type)
        {
            case MenuType.GameStart:
            {
                StartGame();
                break;
            }
            case MenuType.Settings:
            {
                OpenSettings();
                break;
            }
            case MenuType.GameExit:
            {
                ExitGame();
                break;
            }
        }
    }

    private bool isStarted = false;

    private void StartGame()
    {
        if (isStarted) return;
        isStarted = true;
        menuDict[curMenu].SetClickEffect();

        print("StartGame");
        SceneLoader.Instance.LoadScene("Tutorial");
    }

    private bool activeSettingUI = false;
    private void OpenSettings()
    {
        print("OpenSettings");
        activeSettingUI = true;
        var ui = UIManager.Instance.OpenUI<SettingUI>();
        var trigger = ui.AddComponent<OnDestroyTrigger>();
        trigger.onDestroy += () =>
        {
            activeSettingUI = false;
        };
    }


    private void ExitGame()
    {
        print("exitGame");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}