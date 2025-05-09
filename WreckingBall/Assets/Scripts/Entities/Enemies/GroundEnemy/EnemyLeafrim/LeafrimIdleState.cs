using UnityEngine;

public class LeafrimIdleState : State
{
    private Enemy_Leafrim enemy;

    public LeafrimIdleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Leafrim enemy)
        : base(entity, stateMachine, animBoolName)
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
