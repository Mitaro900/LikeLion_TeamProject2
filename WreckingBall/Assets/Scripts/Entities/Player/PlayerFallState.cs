using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
