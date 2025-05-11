using UnityEngine;

public class BatIdleState : State
{
    private Enemy_Bat enemy;

    public BatIdleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bat enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);



    }
}
