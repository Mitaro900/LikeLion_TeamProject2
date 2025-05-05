using UnityEngine;
using UnityEngine.Events;

public class BossState
{
    protected BossStateMachine stateMachine;
    protected Boss boss;

    protected Rigidbody2D rb;

    public string animBoolName { get; protected set; }
    public string nowAnimName { get; protected set; }

    protected float stateTime;
    public float cooldownTime { get; protected set; }
    protected bool isWallDetected;

    public BossState(BossStateMachine stateMachine, Boss boss, string animBoolName, float cooldown = 0f)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
        this.animBoolName = animBoolName;
        this.cooldownTime = cooldown;
    }

    public virtual void Enter(bool isAnimPlay = true)
    {
        if(isAnimPlay)
            boss.anim.SetBool(animBoolName, true);
        if(rb == null)
            rb = boss.GetComponent<Rigidbody2D>();
    }

    public virtual void Exit(bool isAnimPlay = true)
    {
        if (isAnimPlay)
            boss.anim.SetBool(animBoolName, false);
        
    }

    public virtual void Update()
    {
        nowAnimName = boss.anim.GetCurrentAnimatorClipInfo(0).Length > 0 ? boss.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name : "null";
    }

    public virtual void AnimationFinishTrigger()
    {
        //Debug.Log(nameof(BossState) + " " + nameof(AnimationFinishTrigger)+" "+nameof(stateMachine.currentState)+" "+stateMachine.currentState.ToString()+" : "+stateMachine.currentState.animBoolName+" / "+stateMachine.currentState.nowAnimName);
        isWallDetected = boss.IsWallDetected();
    }

    public virtual void SkipAnimation(string oldAnimName, string newAnimName = null)
    {
        if(newAnimName != null)
            boss.anim.SetBool(newAnimName, true);
        boss.anim.SetBool(oldAnimName, false);
        
    }

    public override string ToString()
    {
        //return base.ToString();
        return this.GetType().Name;
    }

}

/// <summary> 가만히 서있는 애니메이션 </summary>
public class Boss_IdleState : BossState
{
    public Boss_IdleState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(isAnimPlay);
        boss.SetZeroVelocity();
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

/// <summary> 이동 애니메이션 </summary>
public class Boss_MoveState : BossState
{
    protected bool canMove = true;
    protected float moveBoost = 1f;
    public Boss_MoveState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(isAnimPlay);
        canMove = true;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        canMove = false;
        boss.SetZeroVelocity();
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
        if (canMove)
            boss.SetVelocity(boss.GetFacingDir() * boss.GetAbility().moveSpeed * moveBoost, 0);
        Debug.Log(nameof(Boss_MoveState) + " " + nameof(Update) + $" moveSpeed : {boss.GetAbility().moveSpeed} / {nameof(moveBoost)} : {moveBoost}");
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isWallDetected)
            boss.Flip();
        
    }
}

/// <summary> 피해 입음 애니메이션 </summary>
public class Boss_DamageState : BossState
{
    public Boss_DamageState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(isAnimPlay);
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

/// <summary> 죽음 애니메이션 </summary>
public class Boss_DeathState : BossState
{
    public Boss_DeathState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(isAnimPlay);
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}


/// <summary> 공격 애니메이션 </summary>
public class Boss_AttackState : BossState
{
    public Boss_AttackState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(isAnimPlay);
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}