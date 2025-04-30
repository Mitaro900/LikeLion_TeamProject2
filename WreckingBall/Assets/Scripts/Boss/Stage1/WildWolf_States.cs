using UnityEngine;

/// <summary> 달리기+공격 애니메이션 </summary>
public class WildWolf_RunAttackState : Boss_AttackState
{
    private WildWolf wolf;
    private bool isPlayerAttacked = false;
    public WildWolf_RunAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        animBoolName = "Run";
        base.Enter(true);
        //boss.anim.SetBool("Attack", true);
        rb.AddForce(Vector2.right * boss.GetFacingDir() * boss.GetAbility().moveSpeed);
        isPlayerAttacked = false;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
        wolf.SetVelocity(wolf.GetFacingDir() * wolf.GetAbility().moveSpeed, 0);

        //Debug.Log(nameof(WildWolf_RunAttackState) + " " + nameof(Update) + $" now : {nowAnimName} / default : {animBoolName}");
        
        if(IsPlayerInRange())
        {
            if(nowAnimName.Contains("Attack"))
            {
                wolf.player.Damage();
                Rigidbody2D _rb = wolf.player.GetComponent<Rigidbody2D>();
                _rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);


                boss.anim.SetBool("Attack", false);
                boss.anim.SetTrigger("Skip");
                boss.anim.SetBool("Run", true);
                isPlayerAttacked = true;
            }
            else if(nowAnimName.Contains("Run"))
            {
                boss.anim.SetBool("Run", false);
                boss.anim.SetTrigger("Skip");
                boss.anim.SetBool("Attack", true);
            }

        }

        Debug.Log(nameof(WildWolf_RunAttackState)+" "+nameof(Update)+" wolf.rb.linearVelocity : "+ wolf.rb.linearVelocity.ToString());
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (animBoolName.Contains("Run"))
        {
            if (!isPlayerAttacked)
            {
                if(IsPlayerInRange())
                {
                    boss.anim.SetBool("Attack", true);
                }
            }
            else
            {
                Exit(false);
            }
        }
        else if(animBoolName.Contains("Attack"))
        {
            if(isPlayerAttacked)
                boss.anim.SetBool("Run", true);
            else
                rb.AddForce(Vector2.right * boss.GetFacingDir() * boss.GetAbility().moveSpeed);
        }

        if(boss.IsWallDetected() == false)
        {
            boss.Flip();
            wolf.controller.NextAction(true);
        }
    }

    private bool IsPlayerInRange()
    {
        Collider2D[] colliders = boss.AttackCheck();
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject == wolf.player.gameObject)
            {
                return true;
            }
        }

        return false;
    }
}


/// <summary> 달리기 애니메이션 </summary>
public class WildWolf_RunState : Boss_MoveState
{
    private WildWolf wolf;
    public WildWolf_RunState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter(false);
        boss.anim.SetBool("Run", true);
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

/// <summary> 바닥 쓸기 공격 </summary>
public class WildWolf_FloorSlideState : Boss_AttackState
{
    private WildWolf wolf;
    public WildWolf_FloorSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 통통 튕기기 공격 </summary>
public class WildWolf_JumpAttackState : Boss_AttackState
{
    private WildWolf wolf;
    public WildWolf_JumpAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 트랩 던지기 공격 </summary>
public class WildWolf_ThrowTrapState : BossState
{
    private WildWolf wolf;
    public WildWolf_ThrowTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 공중 쓸기 공격 </summary>
public class WildWolf_AerialSlideState: Boss_AttackState
{
    private WildWolf wolf;
    public WildWolf_AerialSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 역V 찍기 공격 </summary>
public class WildWolf_TakeDown_VAttackState: Boss_AttackState
{
    private WildWolf wolf;
    public WildWolf_TakeDown_VAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 트랩 떨어뜨리기 공격 </summary>
public class WildWolf_DroppingTrapState : BossState
{
    private WildWolf wolf;
    public WildWolf_DroppingTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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

/// <summary> 벽에서 대각으로 내려찍기 공격 </summary>
public class WildWolf_TakeDown_DirectAttackState : Boss_AttackState
{
    private WildWolf wolf;
    public WildWolf_TakeDown_DirectAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
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