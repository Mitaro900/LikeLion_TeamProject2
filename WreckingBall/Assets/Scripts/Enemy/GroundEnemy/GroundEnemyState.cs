using UnityEngine;

public class GroundEnemyState
{
    protected GroundEnemyStateMachine G_stateMachine;
    protected GroundEnemy G_enemy;
    protected Rigidbody2D G_rb;

    protected bool triggerCalled;
    private string G_animBoolName;

    protected float stateTimer;

    public GroundEnemyState(GroundEnemy _G_enemy, GroundEnemyStateMachine _G_stateMachine, string _G_animBoolName)
    {
        this.G_enemy = _G_enemy;
        this.G_stateMachine = _G_stateMachine;
        this.G_animBoolName = _G_animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        G_rb = G_enemy.rb;
        G_enemy.anim.SetBool(G_animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        G_enemy.anim.SetBool(G_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }



}