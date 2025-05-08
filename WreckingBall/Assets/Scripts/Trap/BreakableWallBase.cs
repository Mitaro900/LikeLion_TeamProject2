using UnityEngine;

public class BreakableWallBase : MonoBehaviour
{
    [Header(nameof(BreakableWallBase)+ ".Settings")]
    [SerializeField] Collider2D col;
    [SerializeField] protected Animator anim;
    [SerializeField] protected float breakableSpeed;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            float speed = rb.linearVelocity.magnitude;
            if (breakableSpeed <= speed && anim.enabled == false)
            {
                col.enabled = false;
                anim.enabled = true;
            }
            else if(col.enabled == false)
            {
                col.enabled = true;
            }
        }
    }

    protected virtual void AnimationFinished()
    {
        gameObject.SetActive(false);
    }
}
