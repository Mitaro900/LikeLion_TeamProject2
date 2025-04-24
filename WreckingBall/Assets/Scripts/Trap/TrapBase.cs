using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TrapProperty
{
    public enum TrapState { None, Idle, Move, Destroyable, Invincibility, Destroy }
    [Tooltip("함정 상태")]
    public List<TrapState> state;
    [Tooltip("상대가 닿으면 피해를 입는 방향")]
    public Vector2[] trapDir;
    [Tooltip("움직이는 속도")]
    public float moveSpeed;
    [Tooltip("돌아오는 속도")]
    public float returnSpeed;
    [Tooltip("움직이는 범위(인식범위)")]
    public float moveDistance;
    [Tooltip("함정 크기")]
    public Vector2 thisSize;
    [Tooltip("함정 길이 조절")]
    public Vector2 sizeOffSet;

    public TrapProperty(List<TrapState> state, Vector2[] trapDir, float moveSpeed, float returnSpeed, float moveDistance, Vector2 trapSize, Vector2 offset)
    {
        this.state = state;
        this.trapDir = trapDir;
        this.moveSpeed = moveSpeed;
        this.returnSpeed = returnSpeed;
        this.moveDistance = moveDistance;
        this.thisSize = trapSize;
        this.sizeOffSet = offset;
    }
}

[System.Serializable]
public class TrapBase : MonoBehaviour
{
    [SerializeField] protected bool isDebug;
    [Header(nameof(TrapBase))]
    [Tooltip("속성")]
    [SerializeField] protected TrapProperty prop;
    [Tooltip("애니메이션 작동 여부")]
    [SerializeField] private bool isActive;
    [SerializeField] private Color gizmoColor = Color.white;
    protected Animator anim; protected Rigidbody2D rb;

    public TrapBase(TrapProperty property)
    {
        this.prop = property;
    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if(anim != null)
            anim.enabled = isActive;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            Rigidbody2D _rb = player.GetComponent<Rigidbody2D>();
            // 함정 오브젝트가 부숴지는 애니메이션 재생중일때
            if (prop.state.Contains(TrapProperty.TrapState.Destroy))
                return;
            //플레이어와 가시부분 충돌
            else if (prop.trapDir.Length > 0)
            {
                bool isCrushed = false;
                for(int i = 0; i < prop.trapDir.Length; i++)
                {
                    isCrushed = prop.trapDir[i].x == (_rb.linearVelocityX > 0 ? 1 : -1);
                    if (isCrushed) break;
                    isCrushed = prop.trapDir[i].y == (_rb.linearVelocityY > 0 ? 1 : -1);
                    if (isCrushed) break;
                }

                if(isCrushed)
                {
                    player.Damage();
                }
            }
            // 함정 오브젝트와 가시가 없는 부분과 충돌했을때 부숴지는 애니메이션 재생
            else if (prop.state.Contains(TrapProperty.TrapState.Destroyable))
            {
                anim.SetBool("Destroy", true);
                prop.state = new() { TrapProperty.TrapState.Destroy };
            }
            else
            {
                player.Damage();
            }
        }
    }

    protected virtual void AnimationFinished()
    {
        string animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animName == "Destroy")
            gameObject.SetActive(false);
    }

    protected virtual void OnDisable() => TrapPoolManager.instance?.Rerurn(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine((Vector3)prop.sizeOffSet + new Vector3(transform.position.x - prop.thisSize.x / 2f, transform.position.y),(Vector3)prop.sizeOffSet + new Vector3(prop.thisSize.x / 2f + transform.position.x, transform.position.y));
        Gizmos.DrawLine((Vector3)prop.sizeOffSet + new Vector3(transform.position.x, transform.position.y - prop.thisSize.y / 2f),(Vector3)prop.sizeOffSet + new Vector3(transform.position.x, prop.thisSize.y / 2f + transform.position.y));
    }
}
