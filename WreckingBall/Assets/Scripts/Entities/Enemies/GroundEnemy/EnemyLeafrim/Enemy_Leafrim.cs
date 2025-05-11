using UnityEngine;

public class Enemy_Leafrim : Enemy
{
    #region States

    public LeafrimIdleState idleState { get; private set; }
    public LeafrimMoveState moveState { get; private set; }
    public LeafrimBattleState battleState { get; private set; }
    public LeafrimAttackState attackState { get; private set; }
    public LeafrimStunnedState stunnedState { get; private set; }
    public LeafrimDieState dieState { get; private set; }

    #endregion

    [Header("감지 옵션")]
    [SerializeField] private float maxVerticalOffset = 1f;  // y차이가 이 값 이하일 때만 감지

    protected override void Awake()
    {
        base.Awake();

        idleState = new LeafrimIdleState(this, stateMachine, "Idle", this);
        moveState = new LeafrimMoveState(this, stateMachine, "Move", this);
        battleState = new LeafrimBattleState(this, stateMachine, "Move", this);
        attackState = new LeafrimAttackState(this, stateMachine, "Attack", this);
        stunnedState = new LeafrimStunnedState(this, stateMachine, "Stun", this);
        dieState = new LeafrimDieState(this, stateMachine, "Die", this);

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
        RaycastHit2D hit = Physics2D.CircleCast(playerCheck.position, Radius, Vector2.right * facingDir, Distance, whatIsPlayer);

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
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageImpact()
    {
        base.DamageImpact();
        stateMachine.ChangeState(dieState);
    }

}
