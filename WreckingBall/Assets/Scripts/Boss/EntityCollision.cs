using UnityEngine;
using UnityEngine.Events;

/// <summary> 상태이상 </summary>
[System.Serializable]
public struct EntityAbnormalState
{
    public Vector2 direction;
    public float duration;
    public bool isChecked;
    public EntityAbnormalState(Vector2 direction, float duration, bool isChecked)
    {
        this.direction = direction;
        this.duration = duration;
        this.isChecked = isChecked;
    }
}

/// <summary> 능력치 </summary>
[System.Serializable]
public struct EntityAbility
{
    public int hp;
    public float moveSpeed;
    public float jumpPower;

    public EntityAbility(int hp, float moveSpeed, float jumpPower)
    {
        this.hp = hp;
        this.moveSpeed = moveSpeed;
        this.jumpPower = jumpPower;
    }
}

public class EntityCollision : MonoBehaviour
{
    [Header(nameof(EntityCollision) + ".능력치")]
    public EntityAbility ability;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Collider2D col { get; private set; }
    #endregion

    [Header(nameof(EntityCollision) + ".상태이상")]
    [Tooltip(nameof(EntityCollision) + ".넉백")]
    [SerializeField] protected EntityAbnormalState knockbackState;
    [Tooltip(nameof(EntityCollision) + ".무적")]
    [SerializeField] protected EntityAbnormalState invincibilityState;



    [Header(nameof(EntityCollision) + ".충돌 정보")]
    public Transform attackCheck;
    public float attackCheckRadius;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    #region Event
    public UnityAction<EntityAbility> damageEvent;
    public UnityAction deathEvent;
    #endregion

    public EntityCollision(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent)
    {
        this.ability = ability;
        this.damageEvent = damageEvent;
        this.deathEvent = deathEvent;
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if(invincibilityState.duration > 0f)
        {
            invincibilityState.duration -= Time.deltaTime;
            invincibilityState.isChecked = invincibilityState.duration > 0f;
        }
    }

    public virtual void Damage()
    {
        ability.hp--;
        if(ability.hp <= 0)
        {
            deathEvent?.Invoke();
        }
        else
        {
            damageEvent?.Invoke(ability);
        }
    }

    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region 플립
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();

    }

    #endregion

    #region 속력
    public void SetZeroVelocity()
    {
        if (knockbackState.isChecked) return;

        rb.linearVelocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (knockbackState.isChecked) return;


        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
}
