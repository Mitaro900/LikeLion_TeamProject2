using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float finalXSpeed;

    public PlayerDashState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player.stateMachine.previousState == player.turnState)
        {
            finalXSpeed = player.DashSpeedThereshold;
        }
        else
        {
            finalXSpeed = player.MoveSpeed;
        }
    }

    public override void Update()
    {
        base.Update();

        if(!Input.GetKey(KeyCode.LeftShift) || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Z) && (player.IsGroundDetected() || player.IsAchored))
        {
            rb.gravityScale = player.JumpGravityScale;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, player.JumpForce);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.gravityScale = player.DefaultGravityScale;
        }
        
        finalXSpeed += player.Acceleration * Time.deltaTime;

        if (finalXSpeed > player.MaxSpeed)
        {
            finalXSpeed = player.MaxSpeed;
        }

        if (player.IsGroundDetected() && finalXSpeed >= player.DashSpeedThereshold)
        {
            if((xInput > 0 && !player.facingRight) || (xInput < 0 && player.facingRight))
            {
                player.stateMachine.ChangeState(player.turnState);
            }
        }

        if (player.IsAchored)
        {
            player.RopeAction(finalXSpeed);
        }
        else
        {
            player.SetVelocity(finalXSpeed * player.facingDir, rb.linearVelocityY);
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = player.DefaultGravityScale;
    }
}
