using UnityEngine;
using System.Collections;

public class Bug2AttackState : EnemyState
{
    private Enemy_Bug2 enemy;
    private float summonDelay = 0.8f;
    public Bug2AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bug2 _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        enemy.StartCoroutine(SummonDelayCoroutine());
        enemy.moveSpeed += 1f;
        enemy.Radius += 6f;
        enemy.PlayerCheckRadius += 6f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.moveSpeed -= 1f;
        enemy.Radius -= 6f;
        enemy.PlayerCheckRadius -= 6f;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(enemy._battleState);
        enemy.SetZeroVelocity();
    }

    private IEnumerator SummonDelayCoroutine()
    {
        yield return new WaitForSeconds(summonDelay);
        Summon();
    }

    private void Summon()
    {
        if (enemy.SpawnFlyPrefab != null && enemy.summonPoint != null)
        {
            GameObject go = Object.Instantiate(enemy.SpawnFlyPrefab, enemy.summonPoint.position, Quaternion.identity);

        }
        else
        {
            Debug.LogWarning("소환몬스터 비어있음");
        }
    }
}
