using UnityEngine;

public class GE_BattleState : GroundEnemyState
{
    private GroundEnemy MEnemy;
    private Transform player;

    public GE_BattleState(GroundEnemy _G_enemy, GroundEnemyStateMachine _G_stateMachine, string _G_animBoolName, GroundEnemy _MEnemy)
        : base(_G_enemy, _G_stateMachine, _G_animBoolName)
    {
        MEnemy = _MEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        // 대기 시간 초기화
        stateTimer = MEnemy.battleTime;
    }

    public override void Update()
    {
        base.Update();

        // Raycast 및 거리 기반 감지
        RaycastHit2D hit = MEnemy.IsPlayerDetected();

        if (hit.collider != null && hit.distance < MEnemy.battleRange)
        {
            // 감지 유지 시간 리셋
            stateTimer = MEnemy.battleTime;

            // Dash: 플레이어 방향으로 돌진
            float dir = player.position.x > MEnemy.transform.position.x ? 1f : -1f;
            MEnemy.SetVelocity(MEnemy.moveSpeed * MEnemy.chaseMultiplier * dir,
                               MEnemy.rb.linearVelocity.y);
            return;
        }

        // 감지 끊기면 대기 타이머 감소
        stateTimer -= Time.deltaTime;

        // 대기 시간이 만료되면 순찰 상태로 복귀
        if (stateTimer <= 0f)
        {
            G_stateMachine.ChangeState(MEnemy.idleState);
        }
        else
        {
            // 기다리는 동안 정지
            MEnemy.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}