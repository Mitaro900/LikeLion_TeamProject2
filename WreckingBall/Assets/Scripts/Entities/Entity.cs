using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public EntityFx fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public BoxCollider2D cd { get; private set; }
    #endregion

    [Header("넉백 정보")]
    [SerializeField] protected Vector2 knockbackDirection = new Vector2(5f, 5f); // 넉백 방향
    [SerializeField] protected float knockbackDuration = 0.5f; // 넉백 지속 시간
    protected bool isKnocked; // 넉백 상태

    [Header("충돌 정보")]
    [SerializeField] protected Transform groundCheck; // 바닥 체크 위치
    [SerializeField] protected float groundCheckDistance = 0.1f; // 바닥 체크 거리
    [SerializeField] protected Transform wallCheck; // 벽 체크 위치
    [SerializeField] protected float wallCheckDistance = 0.1f; // 벽 체크 거리
    [SerializeField] protected LayerMask whatIsGround; // 바닥 레이어

    public float facingDir { get; private set; } = 1f;
    public bool facingRight { get; private set; } = true; // 기본값은 오른쪽

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFx>();
        cd = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFx");
        StartCoroutine(HitKnockback());
        SoundManager.Instance.PlaySFX(SfxTrack.Hit);
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

        SetZeroVelocity();
    }

    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    #region 플립
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void FlipController(float _xVelocity)
    {
        if((_xVelocity > 0 && !facingRight) || (_xVelocity < 0 && facingRight))
        {
            Flip();
        }
    }
    #endregion

    #region 속력
    public void SetZeroVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
}
