using UnityEngine;

public class LeafrimStunnedState : State
{
    private Enemy_Leafrim enemy;

    public LeafrimStunnedState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Leafrim enemy) : base(entity, stateMachine, animBoolName)
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
