using UnityEngine;

[RequireComponent(typeof(BlockBase))]
public class LinkedBlock : MonoBehaviour
{
    [SerializeField] private BlockBase linkedBlock;

    public void TriggerBlock()
    {
        linkedBlock?.OnHit?.Invoke();
    }
}
