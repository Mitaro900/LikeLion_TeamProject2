using UnityEngine;

/// <summary> 달리기+공격 애니메이션 </summary>
public class WildWolf_RunAttackState : Boss_MoveState
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
        canMove = true;
        //boss.anim.SetBool(animBoolName, true);
        //rb.AddForce(Vector2.right * boss.GetFacingDir() * boss.GetAbility().moveSpeed);
        isPlayerAttacked = false;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();
        //wolf.SetVelocity(wolf.GetFacingDir() * wolf.GetAbility().moveSpeed, 0);

        //Debug.Log(nameof(WildWolf_RunAttackState) + " " + nameof(Update) + $" now : {nowAnimName} / default : {animBoolName}");

        if (isPlayerInRange)
        {
            if (nowAnimName.Contains("Attack"))
            {
                if (!isPlayerAttacked)
                {
                    Debug.Log("Player.Damage");
                    wolf.player.Damage();
                    Rigidbody2D _rb = wolf.player.GetComponent<Rigidbody2D>();
                    _rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);


                    //SkipAnimation("Attack", "Run");
                    isPlayerAttacked = true;

                }
                
            }
            else if(nowAnimName.Contains("Run"))
            {
                SkipAnimation("Run", "Attack");
            }

        }
        else if(nowAnimName.Contains("Attack"))
        {
            SkipAnimation("Attack", "Run");
        }

        //Debug.Log(nameof(WildWolf_RunAttackState)+" "+nameof(Update)+" wolf.rb.linearVelocity : "+ wolf.rb.linearVelocity.ToString());
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Debug.Log(nameof(isWallDetected)+" : "+ isWallDetected +" / "+nameof(isPlayerInRange)+" : "+isPlayerInRange);

        if (isWallDetected)
        {
            canMove = false;
            //stateMachine.ChangeState(wolf.idleState);
            //if(nowAnimName.Contains("Attack"))
            //    wolf.anim.SetTrigger("Skip");
            Exit(true);
            
            //wolf.controller.NextAction(true);
        }
        else if (isPlayerInRange && nowAnimName.Contains("Run"))
        {
            SkipAnimation("Run", "Attack");
        }
        else if(!isPlayerInRange && nowAnimName.Contains("Attack"))
        {
            SkipAnimation("Attack", "Run");
        }
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