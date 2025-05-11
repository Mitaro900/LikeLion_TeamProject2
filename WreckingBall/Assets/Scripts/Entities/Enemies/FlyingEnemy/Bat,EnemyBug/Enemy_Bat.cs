using UnityEngine;

public class Enemy_Bat : Enemy
{
    #region States
    public BatMoveState moveState { get; private set; }
    public BatBattleState battleState { get; private set; }
    public BatAttackState attackState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState = new BatIdleState(this, stateMachine, "Idle", this);
        moveState = new BatMoveState(this, stateMachine, "Move", this);
        battleState = new BatBattleState(this, stateMachine, "Move", this);
        attackState = new BatAttackState(this, stateMachine, "Attack", this);
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
