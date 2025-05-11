using UnityEngine;
using System.Collections;

public class Bug2AttackState : State
{
    private Enemy_Bug2 enemy;
    private float summonDelay = 0.8f;

    public Bug2AttackState(Entity entity, StateMachine stateMachine, string animBoolName, Enemy_Bug2 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }


    public override void Enter()
    {
        base.Enter();
        enemy.StartCoroutine(SummonDelayCoroutine());
        enemy.moveSpeed += 1f;
        enemy.PlayerCheckRadius += 6f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.moveSpeed -= 1f;
        enemy.PlayerCheckRadius -= 6f;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
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
