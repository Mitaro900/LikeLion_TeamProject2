using UnityEngine;

public class Bug2BattleState : EnemyState
{
    private Transform player;
    private Enemy_Bug2 enemy;
    private int moveDir;
    public Bug2BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bug2 _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = Player.instance.player.transform;
        
    }
    public override void Update()
    {
        base.Update();
        if (enemy.IsWallDetected())
        {
            enemy.Flip(); 
        }

        if (enemy.IsPlayerDetected(25f))
        {
            stateTimer = enemy.battleTime;

            // 플레이어가 감지되었으면
            enemy.SetZeroVelocity(); // 멈추고

            // X축 방향으로만 바라보기
            float direction = player.position.x - enemy.transform.position.x;
            if (direction > 0 && enemy.facingDir < 0)
                enemy.Flip();
            else if (direction < 0 && enemy.facingDir > 0)
                enemy.Flip();

            // 공격 거리 안이면 공격 상태로 전환
            if (enemy.IsPlayerDetected(25f).distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy._attackState);
            }
        }
        else
        {
            // 플레이어가 감지 안되면 이동 재개
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

            // 너무 멀어지면 idle 상태로 전환
            if (Vector2.Distance(player.position, enemy.transform.position) > 10f)
            {
                stateMachine.ChangeState(enemy._idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }


        return false;
    }


}
