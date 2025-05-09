using UnityEngine;

public class MushAttackState : State
{
    private Enemy_Mush enemy;

    public MushAttackState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.moveSpeed += 2f;
        enemy.Radius += 3f;
        enemy.PlayerCheckRadius += 3f;

        stateTimer = enemy.AttackCooldown;
    }
    public override void Update()
    {
        base.Update();


        enemy.SetZeroVelocity();

        if (triggerCalled || stateTimer < 0f)
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }

    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.moveSpeed -= 2f;
        enemy.Radius -= 3f;
        enemy.PlayerCheckRadius -= 3f;
    }

    
}
