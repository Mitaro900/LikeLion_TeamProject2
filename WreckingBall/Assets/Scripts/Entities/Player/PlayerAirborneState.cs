using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
