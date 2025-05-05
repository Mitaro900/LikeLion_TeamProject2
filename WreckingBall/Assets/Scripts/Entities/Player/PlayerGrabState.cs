using UnityEngine;

public class PlayerGrabState : PlayerState
{
    public PlayerGrabState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.IsBusy = true;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z) && (player.IsGroundDetected() || player.IsAchored))
        {
            rb.gravityScale = player.JumpGravityScale;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, player.JumpForce);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.gravityScale = player.DefaultGravityScale;
        }

        player.SetVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();

        player.IsBusy = false;
        rb.gravityScale = player.DefaultGravityScale;
    }
}
