using UnityEngine;

public class TargetBlock : BlockBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy.IsThrowing)
            {
                onHit?.Invoke();
                SoundManager.Instance.PlaySFX(SfxTrack.Hit);
            }
        }
    }
}
