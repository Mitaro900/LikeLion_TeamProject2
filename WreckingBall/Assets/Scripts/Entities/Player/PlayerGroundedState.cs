using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
