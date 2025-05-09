using UnityEngine;
using UnityEngine.Events;

public class BlockBase : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] protected UnityEvent onHit;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void AnimationFinished()
    {
        Destroy(gameObject);
    }
}
