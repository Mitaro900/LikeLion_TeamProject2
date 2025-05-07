using UnityEngine;

public class BatStunnedState : State
{
    private Enemy_Bat enemy;

    public BatStunnedState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bat enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
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
