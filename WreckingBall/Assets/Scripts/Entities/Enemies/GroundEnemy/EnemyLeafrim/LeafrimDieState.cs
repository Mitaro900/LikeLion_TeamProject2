using UnityEngine;

public class LeafrimDieState : State
{
    private Enemy_Leafrim enemy;

    public LeafrimDieState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Leafrim enemy) : base(entity, stateMachine, animBoolName)
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
        Object.Destroy(enemy.gameObject, 0.8f);
    }

}
