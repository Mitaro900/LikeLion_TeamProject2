using UnityEngine;

public class PlayerBodyslamState : PlayerState
{
    private float currentTime;
    private float finalYSpeed;

    public PlayerBodyslamState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.IsBusy = true;
        rb.gravityScale = 0f;
        stateTimer = 0.5f;
        currentTime = 0f;
        finalYSpeed = 0f;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.MoveSpeed * 0.5f, finalYSpeed);

        currentTime += Time.deltaTime;

        finalYSpeed = Mathf.Lerp(finalYSpeed, -25f, currentTime / 2f);

        if (player.IsGroundDetected())
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.IsBusy = false;
        rb.gravityScale = player.DefaultGravityScale;
    }
}
