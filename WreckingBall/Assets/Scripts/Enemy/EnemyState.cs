using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animBoolName;
    protected Enemy_Bat enemy;
    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine , string _animBoolName,Enemy_Bat _enemy)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.enemy = _enemy;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }


    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }



    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
