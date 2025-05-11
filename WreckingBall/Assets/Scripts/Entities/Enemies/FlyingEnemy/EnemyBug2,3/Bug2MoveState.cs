using UnityEngine;

public class Bug2MoveState : State
{
    private Enemy_Bug2 enemy;

    public Bug2MoveState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bug2 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.battleState);
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
