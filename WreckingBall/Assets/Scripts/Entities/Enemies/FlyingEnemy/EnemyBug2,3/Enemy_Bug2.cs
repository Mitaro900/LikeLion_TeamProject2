using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bug2 : Enemy
{
    [Header("소환 관련")]
    public GameObject SpawnFlyPrefab;       // 소환할 미니언 프리팹
    public Transform summonPoint;

    #region States
    public Bug2MoveState moveState { get; private set; }
    public Bug2BattleState battleState { get; private set; }
    public Bug2AttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new Bug2IdleState(this, stateMachine, "Idle", this);
        moveState = new Bug2MoveState(this, stateMachine, "Move", this);
        battleState = new Bug2BattleState(this, stateMachine, "Move", this);
        attackState = new Bug2AttackState(this, stateMachine, "Attack", this);
        stunnedState = new EnemyStunnedState(this, stateMachine, "Stun", this);
        panicState = new EnemyPanicState(this, stateMachine, "Stun", this);
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
        stateMachine.ChangeState(deadState);
    }

    
}
