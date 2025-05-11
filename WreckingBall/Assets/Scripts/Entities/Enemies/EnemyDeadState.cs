using UnityEngine;

public class EnemyDeadState : State
{
    Enemy enemy;

    public EnemyDeadState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.cd.enabled = false;
        enemy.IsDead = true;

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0f, 10f);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
