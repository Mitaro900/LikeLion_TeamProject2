using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float finalSpeed;

    public PlayerDashState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player.stateMachine.previousState == player.turnState)
        {
            finalSpeed = player.DashSpeedThereshold;
        }
        else
        {
            finalSpeed = player.MoveSpeed;
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

        if(Input.GetKeyDown(KeyCode.Z) && (player.IsGroundDetected() || player.isAchored))
        {
            rb.gravityScale = 1.0f;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, player.JumpForce);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.gravityScale = 2.5f;
        }
        
        finalSpeed += player.Acceleration * Time.deltaTime;

        if (finalSpeed > player.MaxSpeed)
        {
            finalSpeed = player.MaxSpeed;
        }

        if (player.IsGroundDetected() && finalSpeed >= player.DashSpeedThereshold)
        {
            if((xInput > 0 && !player.facingRight) || (xInput < 0 && player.facingRight))
            {
                player.stateMachine.ChangeState(player.turnState);
            }
        }

        if (player.isAchored)
        {
            player.RopeAction(finalSpeed);
        }
        else
        {
            player.SetVelocity(finalSpeed * player.facingDir, rb.linearVelocityY);
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = 2.5f;
    }
}
