using UnityEngine;

public class Enemy_Mush : GroundEnemy
{

    protected override void Awake()
    {
        base.Awake();


        idleState = new GE_IdleState(this, G_stateMachine, "Idle", this);
        moveState = new GE_MoveState(this, G_stateMachine, "Move", this);
        battleState = new GE_BattleState(this, G_stateMachine, "Battle", this);


    }

    protected override void Start()
    {
        base.Start();

        G_stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();


    }

}
