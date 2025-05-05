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
        if(nowAnimName != null && nowAnimName.Contains("Attack"))
            moveBoost = 2f;
        else if(moveBoost != 1f)
            moveBoost = 1f;

        base.Update();

        if (boss.AttackCheck() != null)
        {
            
            if (nowAnimName.Contains("Attack"))
            {

                if (boss.IsGiveDamagedAction() && !isPlayerAttacked)
                {
                    Debug.Log("Player.Damage");
                    wolf.player.Damage();
                    Rigidbody2D _rb = wolf.player.GetComponent<Rigidbody2D>();
                    _rb.AddForce(Vector2.up, ForceMode2D.Impulse);


                    //SkipAnimation("Attack", "Run");
                    isPlayerAttacked = true;
                }
                //임시 무적 체크
                else if(boss.IsGiveDamagedAction() == false && isPlayerAttacked)
                    isPlayerAttacked = false;

            }
            else if(nowAnimName.Contains("Run"))
                SkipAnimation("Run", "Attack");
            
            else
                SkipAnimation("Idle", "Attack");
            
        }
        else if(nowAnimName.Contains("Attack"))
            SkipAnimation("Attack", "Run");
        
        else
            SkipAnimation("Idle", "Run");
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        //Debug.Log(nameof(AnimationFinishTrigger) + (animBoolName != null ? $" name : {animBoolName}" : " name : null") + (nowAnimName != null ? $" now : {nowAnimName}" : " now : null"));

        //Debug.Log(nameof(isWallDetected)+" : "+ isWallDetected +" / "+nameof(isPlayerInRange)+" : "+isPlayerInRange);

        if (isWallDetected)
            stateMachine.ChangeState(wolf.idleState);
        
        else if (boss.AttackCheck() != null && nowAnimName.Contains("Run"))
            SkipAnimation("Run", "Attack");
        
        else if(boss.AttackCheck() == null && nowAnimName.Contains("Attack"))
            SkipAnimation("Attack", "Run");
        
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
        animBoolName = "Run";
        base.Enter(true);
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
public class WildWolf_FloorSlideState : Boss_MoveState
{
    public int attackCount { get; protected set; } = 0;
    public int maxAttackCount { get; protected set; } = 1;
    private bool isReturnning = false;
    private bool isPlayerAttacked = false;

    private Collider2D collider;
    private WildWolf wolf;

    public WildWolf_FloorSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        collider = boss.GetComponent<Collider2D>();
        collider.isTrigger = true;
        animBoolName = "Attack";
        moveBoost = 2f;

        base.Enter(true);

        rb.gravityScale = 0;
        isReturnning = false;
        isPlayerAttacked = false;

        boss.AddVisibleEvent(this.SlideTurn);
    }

    public override void Exit(bool isAnimPlay = true)
    {
        collider.isTrigger = false;
        rb.gravityScale = 1;
        boss.RemoveVisibleEvent(this.SlideTurn);
        base.Exit(true);
    }

    public override void Update()
    {
        if(isReturnning && canMove)
        {
            boss.SetZeroVelocity();
            canMove = false;
        }
        //Debug.Log(nameof(WildWolf_FloorSlideState) + " " + nameof(Update) + " : " + $"canMove : {canMove} / isReturnning : {isReturnning}");
        

        Vector3 pos = boss.transform.position;

        //공격하는 부분
        if(boss.AttackCheck() != null && boss.IsGiveDamagedAction())
        {
            //임시 무적
            if(!isPlayerAttacked)
            {
                Debug.Log("Player.Damage");
                wolf.player.Damage();
                Rigidbody2D _rb = wolf.player.GetComponent<Rigidbody2D>();
                _rb.AddForce(Vector2.up, ForceMode2D.Impulse);

                isPlayerAttacked = true;
            }
            
        }
        else if (boss.IsGiveDamagedAction() == false && isPlayerAttacked)
            isPlayerAttacked = false;

        //이동부분
        if (isReturnning)
        {
            //방향 찾기
            float _distance = pos.x - boss.oriPos.x;
            if (_distance > 0 && boss.GetFacingDir() > 0 || _distance < 0 && boss.GetFacingDir() < 0)
            {
                //Debug.Log("방향찾기");
                boss.Flip();
            }

            pos += Vector3.right * boss.GetFacingDir() * boss.GetAbility().moveSpeed * Time.deltaTime;

            float _max = boss.GetAbility().moveSpeed * moveBoost * Time.deltaTime;
            //Debug.Log("distance : " + _distance +" / max : "+_max);

            if (Mathf.Abs(_distance) <= _max)
            {
                //Debug.Log("이동완료");
                pos = boss.oriPos;
            }
            boss.transform.position = pos;

            if(pos == boss.oriPos)
            {
                boss.Flip();
                stateMachine.ChangeState(wolf.idleState);
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void AnimationFinishTrigger()
    {
        //base.AnimationFinishTrigger();
    }

    public void SlideTurn(bool isVisible)
    {
        //Debug.Log(nameof(WildWolf_FloorSlideState) + " " + nameof(SlideTurn) + $" : isVisible - {isVisible} / attackCount : {attackCount} / maxAttackCount : {maxAttackCount}");
        if (isVisible)
            return;
        
        wolf.Flip();
        attackCount++;
        if (attackCount >= maxAttackCount)
            isReturnning = true;
    }

    public void SetAttackCount(int count)
    {
        maxAttackCount = count;
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