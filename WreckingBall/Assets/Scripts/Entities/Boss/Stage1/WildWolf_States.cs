using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    public WildWolf_FloorSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, int count) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        maxAttackCount = count;
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
                Debug.Log("끝");
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
}

/// <summary> 통통 튕기기 공격 </summary>
public class WildWolf_JumpAttackState : Boss_AttackState
{
    public int attackCount { get; protected set; } = 0;
    public int maxAttackCount { get; protected set; } = 1;
    private bool isPlayerAttacked = false;

    private WildWolf wolf;
    public WildWolf_JumpAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, int count) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        maxAttackCount = count;
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

        //공격하는 부분
        if (boss.AttackCheck() != null && boss.IsGiveDamagedAction())
        {
            //임시 무적
            if (!isPlayerAttacked)
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
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

/// <summary> 트랩 던지기 공격 </summary>
public class WildWolf_ThrowTrapState : BossState
{
    protected TrapPoolManager pool;

    protected float throwCooldown = 1f;
    protected float throwDelay = 0f;
    protected int throwCount = 0;
    protected int maxThrowCount = 1;
    protected string throwName;

    protected Coroutine throwCoroutine = null;

    private WildWolf wolf;
    public WildWolf_ThrowTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, string throwName, int throwCount) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        this.throwName = throwName;
        this.maxThrowCount = throwCount;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
        if(pool == null)
            pool = GameObject.FindFirstObjectByType<TrapPoolManager>();

        throwDelay = 0;
        boss.anim.SetBool("NextThrow", false);
        throwCoroutine = null;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if(nowAnimName.Contains("Ready") && throwCoroutine == null)
            throwCoroutine = wolf.StartCoroutine(this.ThrowTrapCoroutine());
    }

    public IEnumerator ThrowTrapCoroutine()
    {
        WaitForSeconds wait = new(throwCooldown);
        if (pool != null)
        {
            List<GameObject> _traps = pool.Call(throwName, wolf.throwPos.position, maxThrowCount);

            for (int i = 0; i < _traps.Count; i++)
            {
                if(stateMachine.currentState != this)
                    yield break;
                

                GameObject _trap = _traps[i];
                _trap.SetActive(true);

                boss.anim.SetBool("NextThrow", i + 1 < _traps.Count);
                boss.anim.SetTrigger("Throw");

                //Debug.Log(nameof(WildWolf_ThrowTrapState) + " " + nameof(ThrowTrapCoroutine) + $" Throw Call : {_trap.name}", _trap);

                _trap.transform.DOJump(wolf.player.transform.position, 10f, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //TrapBase trap = _trap.GetComponent<TrapBase>();
                    _trap.SetActive(false);
                });
                
                yield return wait;
            }
        }

        stateMachine.ChangeState(wolf.idleState);
    }
}

/// <summary> 공중 쓸기 공격 </summary>
public class WildWolf_AerialSlideState: Boss_AttackState
{
    private int animIndex = 0;
    private bool isPlayerAttacked = false;

    private int attackCount = 0;
    private int maxAttackCount = 1;
    private Coroutine climbCoroutine = null;

    private float boost = 2.5f;
    private Collider2D col;

    private WildWolf wolf;
    public WildWolf_AerialSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, int attackCount) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        this.maxAttackCount = attackCount;
        
    }

    public override void Enter(bool isAnimPlay = true)
    {
        animBoolName = "Attack";
        animIndex = 0;
        attackCount = 0;
        base.Enter(true);
        boss.AddVisibleEvent(this.SlideTurn);
        if (col == null)
            col = wolf.GetComponent<Collider2D>();
    }

    public override void Exit(bool isAnimPlay = true)
    {
        boss.RemoveVisibleEvent(this.SlideTurn);
        animIndex = 0;
        if (climbCoroutine != null)
            climbCoroutine = null;
        if (col.isTrigger)
            col.isTrigger = false;
        if (boss.anim.speed != 1f)
            boss.anim.speed = 1f;
        attackCount = 0;
        base.Exit(true);
    }

    public override void Update()
    {
        base.Update();

        //공격하는 부분
        if (boss.AttackCheck() != null && boss.IsGiveDamagedAction())
        {
            //임시 무적
            if (!isPlayerAttacked)
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

        //Debug.Log(nameof(WildWolf_AerialSlideState)+" "+nameof(Update)+" "+$"animIndex : {animIndex} / climbCoroutine : {(climbCoroutine == null? "null" : "not null")} / rb.linearVelocityY : {rb.linearVelocity}");


        //이동하는 부분
        switch (animIndex)
        {
            //벽으로 이동
            case 0:
                if (climbCoroutine != null)
                    climbCoroutine = null;
                int dir = boss.transform.position.x >= 0f ? 1 : -1;
                if (boss.GetFacingDir() != dir)
                    boss.Flip();
                boss.SetVelocity(boss.GetAbility().moveSpeed * boss.GetFacingDir());
                if (boss.IsWallDetected())
                {
                    boss.anim.speed = 5f;
                    animIndex = 1;
                    boss.SetZeroVelocity();
                    rb.gravityScale = 0;
                }
                
                break;
            //벽 오르기
            case 1:
                if (rb.gravityScale != 0)
                    rb.gravityScale = 0;

                if (climbCoroutine == null)
                    climbCoroutine = boss.StartCoroutine(ClimbWall());

                break;
            //공중 연속 슬라이드
            case 2:
                if(col)
                if (boss.anim.speed != 1f)
                    boss.anim.speed = 1f;
                if (climbCoroutine != null)
                    climbCoroutine = null;
                if (col.isTrigger == false)
                    col.isTrigger = true;
                if (rb.linearVelocityY != 0)
                    rb.linearVelocityY = 0;

                boss.SetVelocity(boss.GetAbility().moveSpeed * boss.GetFacingDir() * boost);

                if (attackCount >= maxAttackCount)
                    animIndex = 3;
                
                break;
            case 3:
                if(rb.linearVelocity.x != 0)
                    rb.linearVelocityX = 0;
                if (rb.linearVelocityY != 0)
                    rb.linearVelocityY = 0;
                if (col.isTrigger && boss.IsGroundDetected() && boss.IsWallDetected())
                    col.isTrigger = false;


                boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.oriPos, boss.GetAbility().moveSpeed * boost * Time.deltaTime);
                //Debug.Log("distance : " + _distance +" / max : "+_max);

                if (Vector3.Distance(boss.oriPos, boss.transform.position) <= 1f)
                {
                    //Debug.Log("이동완료");
                    boss.transform.position = boss.oriPos;
                    boss.SetZeroVelocity();
                    boss.Flip();
                    stateMachine.ChangeState(wolf.idleState);
                }

                break;
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public IEnumerator ClimbWall()
    {
        float climbTime = Random.Range(0.5f, 2f);
        WaitForFixedUpdate wait = new();
        while (climbTime > 0f)
        {
            if (stateMachine.currentState != this)
                yield break;
            climbTime -= Time.fixedDeltaTime;
            boss.SetVelocity(0, boss.GetAbility().moveSpeed);
            yield return wait;
        }

        boss.SetZeroVelocity();
        boss.Flip();

        animIndex = 2;
    }

    public void SlideTurn(bool isVisible)
    {
        //Debug.Log(nameof(WildWolf_FloorSlideState) + " " + nameof(SlideTurn) + $" : isVisible - {isVisible} / attackCount : {attackCount} / maxAttackCount : {maxAttackCount}");
        if (isVisible)
            return;
        if(animIndex !=2)
            return; 

        wolf.Flip();
        
        boss.transform.DOMove(new Vector3(boss.transform.position.x, wolf.player.transform.position.y), 0.5f).OnComplete(() =>
        {
            attackCount++;
        });
    }
}

/// <summary> 역V 찍기 공격 </summary>
public class WildWolf_TakeDown_VAttackState: Boss_AttackState
{
    private bool isAction = false;
    private bool isPlayerAttacked = false;
    private int attackCount = 0;
    private int maxAttackCount = 1;

    private WildWolf wolf;
    public WildWolf_TakeDown_VAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, int attackCount) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        base.Enter();
        isAction = false;
        attackCount = 0;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();

        //공격하는 부분
        if (boss.AttackCheck() != null && boss.IsGiveDamagedAction())
        {
            //임시 무적
            if (!isPlayerAttacked)
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

        if (boss.IsGroundDetected() && isAction == false)
        {
            isAction = true;

            if(boss.transform.position.x > wolf.player.transform.position.x && boss.GetFacingDir() == 1)
                boss.Flip();
            else if (boss.transform.position.x < wolf.player.transform.position.x && boss.GetFacingDir() == -1)
                boss.Flip();

            boss.transform.DOJump(wolf.player.transform.position, 10f, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isAction = false;
                boss.SetZeroVelocity();

                attackCount++;
                if (attackCount >= maxAttackCount)
                    stateMachine.ChangeState(wolf.idleState);
            });
        }
    }
}

/// <summary> 트랩 떨어뜨리기 공격 </summary>
public class WildWolf_DroppingTrapState : BossState
{
    protected TrapPoolManager pool;

    private int animIndex = 0;

    protected float throwCooldown = 1f;
    protected float throwDelay = 0f;
    protected int throwCount = 0;
    protected int maxThrowCount = 1;
    protected string throwName;

    protected Coroutine throwCoroutine = null;
    private Collider2D col;

    private WildWolf wolf;
    public WildWolf_DroppingTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, string throwName, int throwCount) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        this.throwName = throwName;
        this.maxThrowCount = throwCount;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        animBoolName = "Attack";
        base.Enter(true);
        if (pool == null)
            pool = GameObject.FindFirstObjectByType<TrapPoolManager>();
        if (col == null)
            col = boss.GetComponent<Collider2D>();

        animIndex = 0;

        throwCoroutine = null;
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();

        switch (animIndex)
        {
            //벽으로 이동
            case 0:
                int dir = boss.transform.position.x >= 0f ? 1 : -1;
                if (boss.GetFacingDir() != dir)
                    boss.Flip();
                boss.SetVelocity(boss.GetAbility().moveSpeed * boss.GetFacingDir());
                if (boss.IsWallDetected())
                {
                    boss.anim.speed = 5f;
                    animIndex = 1;
                    boss.SetZeroVelocity();
                    rb.gravityScale = 0;
                }

                break;
            //벽 오르기
            case 1:
                if (boss.anim.speed != 5f)
                    boss.anim.speed = 5f;
                if (throwCoroutine != null)
                    throwCoroutine = null;

                boss.SetVelocity(0, boss.GetAbility().moveSpeed * boss.GetFacingDir());
                if (boss.IsCeilingDetected())
                {
                    animIndex = 2;
                    boss.SetZeroVelocity();
                }

                break;

            //트랩 던지기
            case 2:
                if (boss.anim.speed != 1f)
                    boss.anim.speed = 1f;
                if (col.isTrigger == false)
                    col.isTrigger = true;
                if (throwCoroutine != null)
                    throwCoroutine = null;

                if (boss.anim.GetBool("ThrowTrap") == false)
                {
                    throwDelay = 0;
                    SkipAnimation("Attack", "ThrowTrap");
                    if (throwCoroutine == null)
                        throwCoroutine = wolf.StartCoroutine(this.ThrowTrapCoroutine());
                }

                break;

            //원위치
            case 3:
                if (boss.anim.GetBool("Attack") == false)
                {
                    boss.anim.SetBool("Attack", true);
                    boss.transform.DOMove(boss.oriPos, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        boss.SetZeroVelocity();
                        if (boss.GetFacingDir() == 1)
                            boss.Flip();
                        stateMachine.ChangeState(wolf.idleState);
                    });
                }

                if (col.isTrigger && boss.IsWallDetected() == false && boss.IsGroundDetected() == false)
                    col.isTrigger = false;
                break;
        }
    }

    public IEnumerator ThrowTrapCoroutine()
    {
        WaitForSeconds wait = new(throwCooldown);
        WaitForSeconds wait5 = new(0.5f);
        WaitForEndOfFrame frame = new();

        if (pool != null)
        {
            List<GameObject> _traps = pool.Call(throwName, wolf.throwPos.position, maxThrowCount);
            //Debug.Log(ThrowTrapCoroutine() + " " + nameof(ThrowTrapCoroutine) + $" : {_traps.Count} / {throwName}", _traps[0]);

            for (int i = 0; i < _traps.Count; i++)
            {
                if (stateMachine.currentState != this)
                    yield break;

                if (boss.transform.position.x > wolf.player.transform.position.x && boss.GetFacingDir() == 1)
                    boss.Flip();
                else if (boss.transform.position.x < wolf.player.transform.position.x && boss.GetFacingDir() == -1)
                    boss.Flip();

                boss.transform.DOMoveX(wolf.player.transform.position.x, 0.5f);
                yield return wait5;

                GameObject _trap = _traps[i];
                _trap.transform.position = wolf.throwPos.position;
                _trap.SetActive(true);

                boss.anim.SetBool("NextThrow", i + 1 < _traps.Count);
                boss.anim.SetTrigger("Throw");

                Debug.Log(nameof(WildWolf_ThrowTrapState) + " " + nameof(ThrowTrapCoroutine) + $" Throw Call [{i}] : {_trap.name}", _trap);

                _trap.transform.DOMoveY(0, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //TrapBase trap = _trap.GetComponent<TrapBase>();
                    _trap.SetActive(false);
                });

                yield return wait;
            }
        }

        boss.anim.SetBool("ThrowTrap", false);
        animIndex = 3;
    }
}

/// <summary> 벽에서 대각으로 내려찍기 공격 </summary>
public class WildWolf_TakeDown_DirectAttackState : Boss_AttackState
{
    //인덱스0 : 벽찾기
    private Vector2 wallPos = Vector2.zero;
    private Vector2 directPos = Vector2.zero;

    private int animIndex = 0;
    private int attackCount = 0;
    private int maxAttackCount = 1;

    private bool isPlayerAttacked = false;
    private Coroutine climbCoroutine = null;

    private WildWolf wolf;
    public WildWolf_TakeDown_DirectAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName, WildWolf wolf, int attackCount) : base(stateMachine, boss, animBoolName)
    {
        this.wolf = wolf;
        this.maxAttackCount = attackCount;
    }

    public override void Enter(bool isAnimPlay = true)
    {
        animBoolName = "Attack";
        base.Enter(true);
        animIndex = 0;
        wallPos = new Vector2(FindWall() + boss.transform.position.x, boss.transform.position.y);
    }

    public override void Exit(bool isAnimPlay = true)
    {
        base.Exit(isAnimPlay);
    }

    public override void Update()
    {
        base.Update();

        //공격하는 부분
        if (boss.AttackCheck() != null && boss.IsGiveDamagedAction())
        {
            //임시 무적
            if (!isPlayerAttacked)
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



        //이동하는 부분
        switch (animIndex)
        {
            //벽으로 이동
            case 0:
                if (directPos != Vector2.zero)
                    directPos = Vector2.zero;

                int dir = wallPos.x > 0 ? 1 : -1;
                if (boss.GetFacingDir() != dir)
                    boss.Flip();
                boss.SetVelocity(boss.GetAbility().moveSpeed * dir);
                if (boss.IsWallDetected())
                {
                    boss.anim.speed = 5f;
                    animIndex++;
                    boss.SetZeroVelocity();
                    rb.gravityScale = 0;
                }
                if (climbCoroutine != null)
                    climbCoroutine = null;
                break;
            //벽 오르기
            case 1:
                if (directPos != Vector2.zero)
                    directPos = Vector2.zero;
                if (rb.gravityScale != 0)
                    rb.gravityScale = 0;

                if (climbCoroutine == null)
                    climbCoroutine = boss.StartCoroutine(ClimbWall());

                break;
            //벽에서 내려찍기
            case 2:
                if (rb.gravityScale != 1)
                    rb.gravityScale = 1;
                if(boss.anim.speed != 1f)
                    boss.anim.speed = 1f;
                if (climbCoroutine != null)
                    climbCoroutine = null;

                //출발
                if (directPos != Vector2.zero)
                {
                    boss.transform.DOMove(directPos, 1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        Debug.Log($"{boss.IsGroundDetected()} | {attackCount} : {maxAttackCount}");

                        animIndex = 0;
                        attackCount++;

                        if (attackCount >= maxAttackCount)
                            stateMachine.ChangeState(wolf.idleState);

                        else
                            wallPos = new Vector2(FindWall() + boss.transform.position.x, boss.transform.position.y);
                    });
                    directPos = Vector2.zero;
                }
                break;
        }

    }

    private float FindWall()
    {
        float dis = 12.5f - Mathf.Abs(boss.transform.position.x); 

        return boss.transform.position.x >= 0 ? dis : -dis;
    }

    public IEnumerator ClimbWall()
    {
        float climbTime = Random.Range(1f, 3f);
        WaitForFixedUpdate wait = new();
        while(climbTime > 0f)
        {
            if (stateMachine.currentState != this)
                yield break;

            climbTime -= Time.fixedDeltaTime;
            boss.SetVelocity(0, boss.GetAbility().moveSpeed);
            yield return wait;
        }

        boss.Flip();

        directPos = new Vector2(wolf.player.transform.position.x, boss.oriPos.y);
        animIndex++;
    }
}