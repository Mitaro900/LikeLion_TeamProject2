using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TrapState
{
    public enum MoveState { None, Idle, Move, Destroyable, Invincibility, Destroy }
    public List<MoveState> state;
    public int[] trapDir;
    public float moveSpeed;

    public TrapState(List<MoveState> state, int[] trapDir, float moveSpeed)
    {
        this.state = state;
        this.trapDir = trapDir;
        this.moveSpeed = moveSpeed;
    }
}

[System.Serializable]
public class TrapBase : MonoBehaviour
{
    Animator anim; Rigidbody2D rb;
    TrapState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<Player>())
        {
            Player player = collision.GetComponent<Player>();
            if (state.state.Contains(TrapState.MoveState.Destroy))
                return;
            else if (state.trapDir.ToList().Contains(player.facingDir * -1))
            {
                player.Damage();
            }
            else if (state.state.Contains(TrapState.MoveState.Destroyable))
            {
                anim.SetBool("Destroy", true);
                state.state = new() { TrapState.MoveState.Destroy };
            }
        }
    }

    public virtual void AnimationFinished()
    {
        string animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animName == "Destroy")
            gameObject.SetActive(false);
    }

    private void OnDisable() => TrapPoolManager.instance?.Rerurn(gameObject);
}
