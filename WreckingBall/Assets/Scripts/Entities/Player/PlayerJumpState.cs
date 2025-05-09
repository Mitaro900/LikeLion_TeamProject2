using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = player.JumpGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.JumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(km.GetKey(BindingManager.Action.Jump)))
        {
            rb.gravityScale = player.DefaultGravityScale;
        }

        if (rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = player.DefaultGravityScale;
    }
}
