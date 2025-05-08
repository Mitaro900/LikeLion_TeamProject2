using System;
using UnityEngine;

public class OnDestroyTrigger : MonoBehaviour
{
    public event Action onDestroy;

    void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}