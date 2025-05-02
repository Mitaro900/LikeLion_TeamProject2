using System;
using System.Collections;
using System.Collections.Generic;
using Singleton.Component;
using UnityEngine;
using UnityEngine.UI;

public class BindingManager : SingletonComponent<BindingManager>
{
    #region Singleton

    protected override void AwakeInstance()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (isInitialized) Destroy(gameObject);
    }

    protected override bool InitInstance()
    {
        LoadDefaultBindings();
        LoadBindings();
        return true;
    }

    protected override void ReleaseInstance()
    {
        Destroy(gameObject);
    }

    #endregion

    public enum Action
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Hook,
    }

    private Dictionary<Action, KeyCode> bindings = new Dictionary<Action, KeyCode>();
    private Dictionary<Action, KeyCode> defaultBindings = new Dictionary<Action, KeyCode>();

    private void LoadDefaultBindings()
    {
        defaultBindings[Action.Up] = KeyCode.UpArrow;
        defaultBindings[Action.Down] = KeyCode.DownArrow;
        defaultBindings[Action.Left] = KeyCode.LeftArrow;
        defaultBindings[Action.Right] = KeyCode.RightArrow;
        defaultBindings[Action.Jump] = KeyCode.Z;
        defaultBindings[Action.Hook] = KeyCode.X;

        foreach (var kv in defaultBindings)
            bindings[kv.Key] = kv.Value;
    }

    public KeyCode GetKey(Action action)
    {
        return bindings[action];
    }

    public void Rebind(Action action, KeyCode newKey)
    {
        bindings[action] = newKey;
        SaveBindings();
    }

    public void ResetBinding(Action action)
    {
        bindings[action] = defaultBindings[action];
        SaveBindings();
    }

    private void SaveBindings()
    {
        foreach (var kv in bindings)
            PlayerPrefs.SetString("KB_" + kv.Key, kv.Value.ToString());
        PlayerPrefs.Save();
    }

    private void LoadBindings()
    {
        foreach (Action act in Enum.GetValues(typeof(Action)))
        {
            string key = PlayerPrefs.GetString("KB_" + act, "");
            if (!string.IsNullOrEmpty(key) && Enum.TryParse(key, out KeyCode kc))
                bindings[act] = kc;
        }
    }
}


//사용예제.
namespace TEST
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;

        void Update()
        {
            Vector2 dir = Vector2.zero;
            var km = BindingManager.Instance;

            if (Input.GetKey(km.GetKey(BindingManager.Action.Up)))
                dir += Vector2.up;
            if (Input.GetKey(km.GetKey(BindingManager.Action.Down)))
                dir += Vector2.down;
            if (Input.GetKey(km.GetKey(BindingManager.Action.Left)))
                dir += Vector2.left;
            if (Input.GetKey(km.GetKey(BindingManager.Action.Right)))
                dir += Vector2.right;

            transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
        }
    }
}