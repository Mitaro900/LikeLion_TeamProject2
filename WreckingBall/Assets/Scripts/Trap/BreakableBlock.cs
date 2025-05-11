using UnityEngine;
using UnityEngine.Events;

public class BreakableBlock : BlockBase
{
    [SerializeField] private bool isHardened;

    private void Update()
    {
        if ((PlayerManager.Instance.player.IsAccelerating && !isHardened) || (PlayerManager.Instance.player.IsOverSpeedThreshold && isHardened))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            if (((player.IsAccelerating && !isHardened) || (player.IsOverSpeedThreshold && isHardened))
                && ((collision.contacts[0].normal.x != 0 && player.stateMachine.currentState == player.dashState) ||
                (collision.contacts[0].normal.y < 0) && player.stateMachine.currentState == player.bodyslamState) && anim.enabled == false)
            {
                onHit?.Invoke();
                SoundManager.Instance.PlaySFX(SfxTrack.BlockDestroy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy.IsThrowing)
            {
                onHit?.Invoke();
                SoundManager.Instance.PlaySFX(SfxTrack.BlockDestroy);
            }
        }
    }
}
