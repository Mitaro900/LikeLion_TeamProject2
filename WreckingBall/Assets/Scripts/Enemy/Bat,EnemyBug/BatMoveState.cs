using UnityEngine;

public class BatMoveState : EnemyState
{
    private Transform player;
    private Enemy_Bat enemy;

    private float waitTime;
    private float waitCounter;
    private bool isWaiting;

    private float randomFlipChance = 0.4f;
    private float randomStopChance = 0.4f;

    public BatMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) 
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        isWaiting = false;
        waitTime = Random.Range(1f, 2f);
        waitCounter = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected(10f))
            stateMachine.ChangeState(enemy.battleState);

        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            enemy.SetVelocity(0f, rb.linearVelocity.y);

            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                waitTime = Random.Range(1f, 2f); 
                waitCounter = 0f;

                // 확률적으로 방향 전환
                if (Random.value < randomFlipChance)
                {
                    enemy.Flip();
                }
            }
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

            if (enemy.IsWallDetected())
            {
                enemy.Flip();
                stateMachine.ChangeState(enemy.idleState);
            }
            else if (Random.value < randomStopChance * Time.deltaTime)
            {
                isWaiting = true;
            }
        }



    }
}
