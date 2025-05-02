using UnityEngine;

public class GE_MoveState : GE_GroundState
{
    public GE_MoveState(GroundEnemy _G_enemy, GroundEnemyStateMachine _G_stateMachine, string _G_animBoolName, GroundEnemy _MEnemy)
        : base(_G_enemy, _G_stateMachine, _G_animBoolName, _MEnemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Move 지속 시간 초기화
        stateTimer = MEnemy.moveTime;
        // 순찰 방향 랜덤 설정: 50% 확률로 좌/우 반전
        int dir = Random.value < 0.5f ? -1 : 1;
        if (dir != MEnemy.facingDir)
            MEnemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // (1) 플레이어 감지 시 전투 상태로
        if (MEnemy.IsPlayerDetected())
        {
            G_stateMachine.ChangeState(MEnemy.battleState);
            return;
        }

        // (2) 순찰 이동
        MEnemy.SetVelocity(MEnemy.moveSpeed * MEnemy.facingDir, MEnemy.rb.linearVelocity.y);

        // (3) 벽·낭떠러지 닿거나, 타이머 만료 시 랜덤 복귀
        bool obstacle = MEnemy.IsWallDetected() || !MEnemy.IsGroundDetected();
        if (obstacle || stateTimer <= 0f)
        {
            // 벽/끝 감지 시에는 먼저 방향 반전
            if (obstacle)
                MEnemy.Flip();

            // 다음 상태는 Idle 또는 Move 중 랜덤
            if (Random.value < 0.5f)
                G_stateMachine.ChangeState(MEnemy.idleState);
            else
                G_stateMachine.ChangeState(MEnemy.moveState);
        }

    }
}
