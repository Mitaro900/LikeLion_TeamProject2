using UnityEngine;

public class Bug2MoveState : EnemyState
{
    private Enemy_Bug2 enemy;
    public Bug2MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bug2 _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy._idleState);
        }
    }
}
