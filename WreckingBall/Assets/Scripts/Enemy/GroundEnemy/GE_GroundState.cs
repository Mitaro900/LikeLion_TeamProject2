using UnityEngine;

public class GE_GroundState : GroundEnemyState
{
    protected GroundEnemy MEnemy;
    protected Transform player;

    public GE_GroundState(GroundEnemy _G_enemy, GroundEnemyStateMachine _G_stateMachine, string _G_animBoolName, GroundEnemy _MEnemy)
        : base(_G_enemy, _G_stateMachine, _G_animBoolName)
    {
        MEnemy = _MEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        // 수평 Raycast 감지 (Y축 무시) 및 x축 범위 내 감지 여부 판정
        RaycastHit2D hit = MEnemy.IsPlayerDetected();
        float xDistFromEnemy = Mathf.Abs(player.position.x - MEnemy.transform.position.x);
        // Raycast에 걸렸고, x축 거리(battleRange) 이내일 때만 상태 전환
        if (hit.collider != null && xDistFromEnemy <= MEnemy.battleRange)
        {
            G_stateMachine.ChangeState(MEnemy.battleState);
        }
    }
}