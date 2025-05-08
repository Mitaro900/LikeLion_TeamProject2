using UnityEngine;

public class UnlockedWall : BreakableWallBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            float speed = rb.linearVelocity.magnitude;
            if (breakableSpeed <= speed && anim.enabled == false)
            {
                base.OnTriggerEnter2D(collision);
            }
            else if (col.enabled == false)
            {
                col.enabled = true;
            }
        }
        
    }

    protected override void AnimationFinished()
    {
        base.AnimationFinished();
    }
}
