using UnityEngine;

public class EnemyPanicState : State
{
    private Enemy enemy;

    public EnemyPanicState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();
        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
