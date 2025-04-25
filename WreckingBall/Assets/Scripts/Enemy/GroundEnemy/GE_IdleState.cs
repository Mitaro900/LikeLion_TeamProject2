using UnityEngine;

public class GE_IdleState : GE_GroundState
{
    public GE_IdleState(GroundEnemy _G_enemy, GroundEnemyStateMachine _G_stateMachine, string _G_animBoolName, GroundEnemy _MEnemy)
        : base(_G_enemy, _G_stateMachine, _G_animBoolName, _MEnemy)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = MEnemy.idleTime;

        MEnemy.SetZeroVelocity();
    }
    public override void Update()
    {
        base.Update();

        // 플레이어가 감지되면 즉시 전투 상태로
        if (MEnemy.IsPlayerDetected())
        {
            G_stateMachine.ChangeState(MEnemy.battleState);
            return;
        }

        // 타이머가 다 떨어지면 랜덤하게 Idle 또는 Move로 전환
        if (stateTimer <= 0f)
        {
            if (Random.value < 0.5f)
                G_stateMachine.ChangeState(MEnemy.idleState);
            else
                G_stateMachine.ChangeState(MEnemy.moveState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
