using UnityEngine;

public class BatBattleState : EnemyState
{
    private Transform player;
    private Enemy_Bat enemy;
    private int moveDir;
    public BatBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy)
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

        if (enemy.IsPlayerDetected(10f))
        {
            stateTimer = enemy.battleTime;

            float distanceToPlayer = Vector2.Distance(player.position, enemy.transform.position);

            if (distanceToPlayer > 0.5f)
            {
                MoveTowardPlayer();
            }
            else
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        //if (enemy.IsPlayerDetected())
        //{



        //    stateTimer = enemy.battleTime;
        //    MoveTowardPlayer();
        //    float distanceToPlayer = Vector2.Distance(player.position, enemy.transform.position);

        //    if (distanceToPlayer <= enemy.attackDistance)
        //    {
        //        //공격상태
        //        if(CanAttack())
        //        stateMachine.ChangeState(enemy.attackState);

        //    }
        //}
        //else
        //{
        //    if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) >10)
        //        stateMachine.ChangeState(enemy.idleState);
        //}



        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);


    }

    public override void Exit()
    {
        base.Exit();
    }


    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.SetZeroVelocity();
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        
        return false;
    }

    private void MoveTowardPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, enemy.transform.position.z);
            enemy.transform.position = Vector3.Lerp(enemy.transform.position, targetPosition, enemy.fallSpeed * Time.deltaTime);
        }
    }


}
