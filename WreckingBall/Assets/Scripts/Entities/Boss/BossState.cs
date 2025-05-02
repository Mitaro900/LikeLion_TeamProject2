using UnityEngine;

public class BossState
{
    protected BossStateMachine stateMachine;
    protected Boss boss;

    protected Rigidbody2D rb;

    protected string animBoolName;

    protected float stateTime;
    protected bool triggerCalled;

    public BossState(BossStateMachine stateMachine, Boss boss, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        boss.anim.SetBool(animBoolName, true);
        rb = boss.GetComponent<Rigidbody2D>();
        triggerCalled = false;
    }

    public virtual void Exit()
    {
        boss.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        
    }
}
