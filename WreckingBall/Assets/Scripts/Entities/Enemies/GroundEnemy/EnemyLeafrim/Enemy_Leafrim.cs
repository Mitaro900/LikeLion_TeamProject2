using UnityEngine;

public class Enemy_Leafrim : Enemy
{
    #region States
    public LeafrimMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new LeafrimIdleState(this, stateMachine, "Idle", this);
        moveState = new LeafrimMoveState(this, stateMachine, "Move", this);
        stunnedState = new EnemyStunnedState(this, stateMachine, "Dead", this);
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
