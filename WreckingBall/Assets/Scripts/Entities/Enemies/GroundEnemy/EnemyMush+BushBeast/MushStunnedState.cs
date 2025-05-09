using UnityEngine;

public class MushStunnedState : State
{
    private Enemy_Mush enemy;

    public MushStunnedState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = float.MaxValue;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
    }

    
}
