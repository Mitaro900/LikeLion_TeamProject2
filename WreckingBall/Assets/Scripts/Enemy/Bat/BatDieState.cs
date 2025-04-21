using UnityEngine;

public class BatDieState : EnemyState
{
    private Enemy_Bat enemy;
    public BatDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
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
    }
}
