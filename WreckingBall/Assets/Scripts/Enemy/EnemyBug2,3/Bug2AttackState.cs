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
        enemy.SetZeroVelocity();
        enemy.StartCoroutine(SummonDelayCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(enemy._battleState);
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
            GameObject clone = Object.Instantiate(enemy.SpawnFlyPrefab, enemy.summonPoint.position, Quaternion.identity);
            // 필요하면 clone으로 추가 설정 가능
        }
        else
        {
            Debug.LogWarning("SpawnFlyPrefab이나 summonPoint가 비어있습니다.");
        }
    }
}
