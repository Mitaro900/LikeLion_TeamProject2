using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        player.SetVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
