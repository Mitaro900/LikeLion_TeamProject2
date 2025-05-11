using UnityEngine;

public class Enemy_Mush : Enemy
{
    #region States
    public MushMoveState moveState { get; private set; }
    public MushBattleState battleState { get; private set; }
    public MushAttackState attackState { get; private set; }
    #endregion

    [Header("감지 옵션")]
    [SerializeField] private float maxVerticalOffset = 1f;  // y차이가 이 값 이하일 때만 감지

    protected override void Awake()
    {
        base.Awake();

        idleState = new MushIdleState(this, stateMachine, "Idle", this);
        moveState = new MushMoveState(this, stateMachine, "Move", this);
        battleState = new MushBattleState(this, stateMachine, "Move", this);
        attackState = new MushAttackState(this, stateMachine, "Attack", this);
        stunnedState = new EnemyStunnedState(this, stateMachine, "Stun", this);
        panicState = new EnemyPanicState(this, stateMachine, "Panic", this);
        deadState = new EnemyDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D hit = Physics2D.CircleCast(playerCheck.position, playerCheckRadius, Vector2.right * facingDir, Distance, whatIsPlayer);
        
        if (hit.collider != null)
        {
            float dy = Mathf.Abs(hit.transform.position.y - transform.position.y);
            if (dy <= maxVerticalOffset)  // 예: 1.0f
            {
                return hit;
            }
        }

        return default;
    }

    public override bool CanBeStunned()
    {
        if( base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageImpact()
    {
        base.DamageImpact();

        stateMachine.ChangeState(deadState);
    }
}
