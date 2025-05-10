using UnityEngine;

public class PlayerTurnState : PlayerState
{
    private float currentTime;
    private float finalXSpeed;

    public PlayerTurnState(Entity entity, StateMachine stateMachine, string animBoolName, Player player) : base(entity, stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.85f;
        currentTime = 0f;
        finalXSpeed = player.DashSpeedThereshold;

        SoundManager.Instance.PlaySFX(SfxTrack.Brake);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(finalXSpeed * player.facingDir, rb.linearVelocityY);

        currentTime += Time.deltaTime;
        finalXSpeed = Mathf.Lerp(finalXSpeed, 0f, currentTime / 2);

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
