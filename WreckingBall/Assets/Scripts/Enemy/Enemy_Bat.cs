using UnityEngine;

public class Enemy_Bat : Enemy
{
    public BatIdleState idleState { get; private set; }
    public BatMoveState moveState { get; private set; }
    public BatBattleState battleState { get; private set; }
    public BatAttackState attackState { get; private set; }
    public BatHitState hitState { get; private set; }
    public BatDieState dieState { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        idleState = new BatIdleState(this, stateMachine, "Idle", this);
        moveState = new BatMoveState(this, stateMachine, "Move", this);
        battleState = new BatBattleState(this, stateMachine, "Move", this);
        attackState = new BatAttackState(this, stateMachine, "Attack", this);
        hitState = new BatHitState(this, stateMachine, "Hit", this);
        dieState = new BatDieState(this, stateMachine, "Die", this);
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
}
