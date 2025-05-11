using UnityEngine;

public class BatAttackState : State
{
    private Enemy_Bat enemy;

    public BatAttackState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bat enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.moveSpeed += 2f;
        enemy.PlayerCheckRadius += 3f;
    }
    public override void Update()
    {
        base.Update();


        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);

    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.moveSpeed -= 2f;
        enemy.PlayerCheckRadius -= 3f;
    }
}
