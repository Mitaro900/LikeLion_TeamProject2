using UnityEngine;

public class BatDieState : State
{
    private Enemy_Bat enemy;

    public BatDieState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bat enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
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
