using PKR;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TrapProperty
{
    public enum TrapState { None, Idle, Move, Destroyable, Invincibility, Destroy }
    public List<TrapState> state;
    public Vector2[] trapDir;
    public float moveSpeed;
    public float returnSpeed;
    public float moveDistance;
    public Vector2 thisSize;

    public TrapProperty(List<TrapState> state, Vector2[] trapDir, float moveSpeed, float returnSpeed, float moveDistance, Vector2 trapSize)
    {
        this.state = state;
        this.trapDir = trapDir;
        this.moveSpeed = moveSpeed;
        this.returnSpeed = returnSpeed;
        this.moveDistance = moveDistance;
        this.thisSize = trapSize;
    }
}

[System.Serializable]
public class TrapBase : MonoBehaviour
{
    protected Animator anim; protected Rigidbody2D rb;
    [SerializeField] protected TrapProperty prop;
    [SerializeField] private bool isActive;
    

    public TrapBase(TrapProperty property)
    {
        this.prop = property;
    }

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if(anim != null)
            anim.enabled = isActive;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            Rigidbody2D _rb = player.GetComponent<Rigidbody2D>();
            // 함정 오브젝트가 부숴지는 애니메이션 재생중일때
            if (prop.state.Contains(TrapProperty.TrapState.Destroy))
                return;
            //플레이어와 가시부분 충돌
            else if (prop.trapDir != default)
            {
                bool isCrushed = false;
                for(int i = 0; i < prop.trapDir.Length; i++)
                {
                    isCrushed = prop.trapDir[i].x == (rb.linearVelocityX > 0 ? 1 : -1);
                    if (isCrushed) break;
                    isCrushed = prop.trapDir[i].y == (rb.linearVelocityY > 0 ? 1 : -1);
                    if (isCrushed) break;
                }

                if(isCrushed)
                {
                    //player.Damage();
                }
            }
            // 함정 오브젝트와 가시가 없는 부분과 충돌했을때 부숴지는 애니메이션 재생
            else if (prop.state.Contains(TrapProperty.TrapState.Destroyable))
            {
                anim.SetBool("Destroy", true);
                prop.state = new() { TrapProperty.TrapState.Destroy };
            }
        }
    }

    public virtual void AnimationFinished()
    {
        string animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animName == "Destroy")
            gameObject.SetActive(false);
    }

    public virtual void OnDisable() => TrapPoolManager.instance?.Rerurn(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(transform.position.x - prop.thisSize.x / 2f, transform.position.y), new Vector3(prop.thisSize.x / 2f + transform.position.x, transform.position.y));
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - prop.thisSize.y / 2f), new Vector3(transform.position.x, prop.thisSize.y / 2f + transform.position.y));
    }
}
