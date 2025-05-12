using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy")]
    [Header("스턴 정보")]
    [SerializeField] private float stunDuration = 3f;
    public float StunDuration { get => stunDuration; }
    [SerializeField] private Vector2 stunDirection;
    public Vector2 StunDirection { get => stunDirection; }
    protected bool canBeStunned;

    [Header("이동 정보")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;
    [SerializeField] public float fallSpeed = 2f;
    
    private Transform playerTransform;
    [Header("플레이어 탐지 정보")]
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckRadius;
    public float PlayerCheckRadius { get => playerCheckRadius; set => playerCheckRadius = value; }

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float playerCheckDistance;
    public float PlayerCheckDistance { get => playerCheckDistance; set => playerCheckDistance = value; }

    [Header("공격 정보")]
    [SerializeField] protected Transform attackCheck;
    public Transform AttackCheck { get => attackCheck; }

    [SerializeField] protected float attackCheckRadius;
    public float AttackCheckRadius { get => attackCheckRadius; }

    [SerializeField] protected float attackDistance;
    public float AttackDistance { get => attackDistance; }

    [SerializeField] protected float attackCooldown;
    public float AttackCooldown { get => attackCooldown; }

    [HideInInspector] public float lastTimeAttacked;

    protected bool isGrabbed = false;
    public bool IsGrabbed { get => isGrabbed; set => isGrabbed = value; }
    protected bool isThrowing = false;
    public bool IsThrowing { get => isThrowing; set => isThrowing = value; }
    protected bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }

    public StateMachine stateMachine { get; protected set; }
    public State idleState { get; protected set; }
    public EnemyStunnedState stunnedState { get; protected set; }
    public EnemyPanicState panicState { get; protected set; }
    public EnemyDeadState deadState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if(IsOutOfView() && (isThrowing || isDead))
        {
            Destroy(gameObject);
        }

        if (isGrabbed && !isDead)
        {
            stateMachine.ChangeState(stunnedState);
        }

        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckRadius);
        if (hit.collider != null)
        {
            playerTransform = hit.transform;
            var pl = hit.collider.GetComponent<Player>();
            if (pl != null && pl.IsOverSpeedThreshold && !isDead)
            {
                stateMachine.ChangeState(panicState);
                return;
            }
        }

        stateMachine.currentState.Update();
    }

    public override void DamageImpact()
    {
        base.DamageImpact();

        UIManager.Instance.GetUI<NormalStageUI>()?.AddCombo();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    protected virtual bool IsOutOfView()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        return transform.position.x < min.x || transform.position.x > max.x ||
               transform.position.y < min.y || transform.position.y > max.y;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected()
    => Physics2D.CircleCast(playerCheck.position, playerCheckRadius, Vector2.down * facingDir, PlayerCheckDistance, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + AttackDistance * facingDir, transform.position.y));
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            var currState = player.stateMachine.currentState;
            bool isDashHit = currState == player.dashState && player.IsOverSpeedThreshold;
            bool isBslamHit = currState == player.bodyslamState && player.rb.linearVelocity.y < 0f;

            if (isDashHit || isBslamHit)
            {
                DamageImpact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.IsThrowing)
            {
                DamageImpact();
            }
        }
    }
}
