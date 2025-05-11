using UnityEngine;

public class MushPanicState : State
{
    private Enemy_Mush enemy;

    // 1초 후 빠져나오기 위한 타이머
    private float exitTimer = -2f;
    private const float exitDelay = 2f;

    public MushPanicState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy)
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        // 타이머 리셋
        exitTimer = -2f;
    }

    public override void Update()
    {
        base.Update();

        // 1) 플레이어 과속 감지
        RaycastHit2D hit = enemy.IsPlayerDetected();
        bool playerStillFast = false;
        if (hit.collider != null)
        {
            var pl = hit.collider.GetComponent<Player>();
            if (pl != null && pl.IsOverSpeedThreshold)
                playerStillFast = true;
        }

        if (playerStillFast)
        {
            // 과속 지속 중 → 타이머 초기화하고 계속 Panic
            exitTimer = -2f;
            return;
        }

        // 2) 과속 상태 벗어남 → 타이머 시작
        if (exitTimer < 0f)
            exitTimer = exitDelay;
        else
            exitTimer -= Time.deltaTime;

        // 3) 타이머 다 돌면 Panic 해제
        if (exitTimer <= 0f)
        {
            // 플레이어 아직 감지 중이면 Battle, 아니면 Idle
            RaycastHit2D recheck = enemy.IsPlayerDetected();
            if (recheck.collider != null)
                stateMachine.ChangeState(enemy.battleState);
            else
                stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // (필요하다면) 추가 정리 로직
    }
}
