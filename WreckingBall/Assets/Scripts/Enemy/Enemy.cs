using System.Collections;
using UnityEngine;

public class Enemy : EnemyEntity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Enemy")]
    [Header("스턴 정보")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;


    [Header("이동 정보")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;
    [SerializeField] public float fallSpeed = 2f;
    
    private Transform playerTransform;
    [Header("플레이어 탐지 정보")]
    public float distance;
    public float radius;

    [Header("공격 정보")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;


    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }



    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        RaycastHit2D hit = IsPlayerDetected();
        if (hit.collider != null)
        {
            Debug.Log("플레이어발견");
            playerTransform = hit.transform;
        }
        
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }



    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
    }


    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    


    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected()
    => Physics2D.CircleCast(playerCheck.position, radius, Vector2.down * facingDir, distance, whatIsPlayer);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));

    }


}