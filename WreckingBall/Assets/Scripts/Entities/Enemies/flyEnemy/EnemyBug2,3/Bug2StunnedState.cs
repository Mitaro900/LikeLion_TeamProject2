using UnityEngine;

public class Bug2StunnedState : State
{
    private Enemy_Bug2 enemy;

    public Bug2StunnedState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bug2 enemy) : base(entity, stateMachine, animBoolName)
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
