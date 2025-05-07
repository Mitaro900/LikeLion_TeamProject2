using UnityEngine;

public class Bug2IdleState : State
{
    private Enemy_Bug2 enemy;
    public Bug2IdleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bug2 enemy) : base(entity, stateMachine, animBoolName)
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
            stateMachine.ChangeState(enemy._moveState);
    }
}
