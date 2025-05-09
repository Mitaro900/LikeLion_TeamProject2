using UnityEngine;

public class MushIdleState : State
{
    private Enemy_Mush enemy;

    public MushIdleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy) 
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
