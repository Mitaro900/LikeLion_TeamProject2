using UnityEngine;

public class Bug2BattleState : State
{
    private Transform player;
    private Enemy_Bug2 enemy;
    private int moveDir;

    public Bug2BattleState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bug2 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
        enemy.moveSpeed += 1f;
        enemy.Radius += 6f;
        enemy.PlayerCheckRadius += 6f;

    }
    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.AttackDistance)
            {
                //공격상태
                if (CanAttack())
                    stateMachine.ChangeState(enemy._attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy._idleState);
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
        enemy.moveSpeed -= 1f;
        enemy.Radius -= 6f;
        enemy.PlayerCheckRadius -= 6f;
    }


    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.AttackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }


        return false;
    }


}
