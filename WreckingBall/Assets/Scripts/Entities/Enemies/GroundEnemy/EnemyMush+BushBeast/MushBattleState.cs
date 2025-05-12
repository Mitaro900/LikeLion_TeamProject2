// Assets/Scripts/Entities/Enemies/GroundEnemy/EnemyMush/MushBattleState.cs
using UnityEngine;

public class MushBattleState : State
{
    private Transform player;
    private Enemy_Mush enemy;
    private int moveDir;

    public MushBattleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Mush enemy)
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool("Idle", false);
        enemy.anim.SetBool("Move", true);

        // 1) 플레이어 감지 검사
        RaycastHit2D hit = enemy.IsPlayerDetected();
        if (hit.collider != null)
        {
            // 감지된 콜라이더에서 Player 컴포넌트 찾기
            Player p = hit.collider.GetComponent<Player>();
            if (p != null)
                player = p.transform;
        }

        // 2) 혹시 못 찾았으면 태그로 한 번 더
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
        }

        // 3) 초기 타이머 설정
        stateTimer = enemy.battleTime;

        // 4) 전투 버프 적용
        enemy.moveSpeed += 2f;
        enemy.PlayerCheckRadius += 3f;
    }

    public override void Update()
    {
        base.Update();

        if (player != null && enemy.IsPlayerDetected().collider != null)
        {
            // 플레이어가 여전히 감지되는 중
            stateTimer = enemy.battleTime;

            float dist = Vector2.Distance(player.position, enemy.transform.position);
            if (dist > enemy.AttackDistance)
            {
                //몬스터가 플레이어를 계속 밀어서 바꿈
                //MoveTowardPlayer();
                int moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;
                enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
            }
            else if (Time.time >= enemy.lastTimeAttacked + enemy.AttackCooldown)
            {
                enemy.lastTimeAttacked = Time.time;
                enemy.SetZeroVelocity();
                stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            // 감지 해제 혹은 시간/거리 조건
            bool tooFar = (player != null && Vector2.Distance(player.position, enemy.transform.position) > 10f);
            if (stateTimer < 0 || tooFar)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        // 방향 설정
        if (player != null)
            moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;

        // 이동 적용
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        // 전투 버프 원복
        enemy.moveSpeed -= 2f;
        enemy.PlayerCheckRadius -= 3f;
    }

    private void MoveTowardPlayer()
    {
        if (player == null) return;
        Vector3 current = enemy.transform.position;
        float targetX = player.position.x;

        //MoveSpeed 기반으로 수평 위치만 보정
        float newX = Mathf.MoveTowards(current.x, targetX, enemy.moveSpeed * Time.deltaTime);
        
        //y, z축은 기존 값 유지
        enemy.transform.position = new Vector3(newX, current.y, current.z);
    }
}
