using PKR;
using UnityEngine;

public class EntityCollision : MonoBehaviour
{

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
    [SerializeField] protected LayerMask whatIsPlayer;

    [SerializeField] protected int facingDir = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        Debug.Log(nameof(EntityCollision) + " " + nameof(Awake));
    }

    protected virtual void Start()
    {
        Debug.Log(nameof(EntityCollision) + " " + nameof(Start));
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

    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsPlayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    public virtual int GetFacingDir() => facingDir;

    public virtual Collider2D AttackCheck()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, whatIsPlayer);
        foreach(Collider2D c in hitEnemies)
        {
            if (gameObject.tag != "Player" && c.gameObject.tag == "Player" 
                || gameObject.tag == "Player" && c.gameObject.tag != "Ground")
                return c;
            
        }
        return null;
    }


    protected virtual void OnDrawGizmos()
    {
        if(groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        if(attackCheck != null)
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
    public void SetVelocity(float _xVelocity = 0, float _yVelocity = 0)
    {
        if (knockbackState.isChecked) return;

        if(_xVelocity != 0 && _yVelocity != 0)
            rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        else if(_xVelocity != 0)
            rb.linearVelocityX = _xVelocity;
        else if (_yVelocity != 0)
            rb.linearVelocityY = _yVelocity;
        else
            SetZeroVelocity();
        FlipController(_xVelocity);
    }
    #endregion
}
