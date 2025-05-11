using UnityEngine;

public class BatBattleState : State
{
    private Transform player;
    private Enemy_Bat enemy;
    private int moveDir;

    public BatBattleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bat enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
        enemy.moveSpeed += 2f;
        enemy.PlayerCheckRadius += 3f;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            float distanceToPlayer = Vector2.Distance(player.position, enemy.transform.position);

            if (distanceToPlayer > 0.8f)
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

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed -= 2f;
        enemy.PlayerCheckRadius -= 3f;
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.AttackCooldown)
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
