using UnityEngine;

public class BatStunnedState : EnemyState
{
    private Enemy_Bat enemy;
    public BatStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.dieState);
            Debug.Log("아야");
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    
}
