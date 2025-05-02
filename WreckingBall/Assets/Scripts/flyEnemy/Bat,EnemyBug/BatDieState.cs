using UnityEngine;

public class BatDieState : EnemyState
{
    private Enemy_Bat enemy;
    public BatDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetTrigger("Die");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Object.Destroy(enemy.gameObject,0.8f);
    }
    
}
