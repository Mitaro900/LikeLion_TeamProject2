using UnityEngine;

public class PlayerTurnState : PlayerState
{
    private float currentTime;
    private float finalSpeed;

    public PlayerTurnState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.85f;
        currentTime = 0f;
        finalSpeed = player.DashSpeedThereshold;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(finalSpeed * player.facingDir, rb.linearVelocityY);

        currentTime += Time.deltaTime;
        finalSpeed = Mathf.Lerp(finalSpeed, 0f, currentTime / 2);

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.Flip();
    }
}
