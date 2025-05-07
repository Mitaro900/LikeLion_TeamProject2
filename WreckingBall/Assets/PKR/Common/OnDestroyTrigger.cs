using System;
using UnityEngine;

namespace PKR.Lobby
{
    public class OnDestroyTrigger : MonoBehaviour
    {
        public event Action onDestroy;

        void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}