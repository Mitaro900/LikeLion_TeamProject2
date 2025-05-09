using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
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
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckRadius;
    public float PlayerCheckRadius { get => playerCheckRadius; set => playerCheckRadius = value; }

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float distance;
    public float Distance { get => distance; set => distance = value; }
    
    [SerializeField] protected float radius;
    public float Radius { get => radius; set => radius = value; }

    [Header("공격 정보")]
    [SerializeField] protected Transform attackCheck;
    public Transform AttackCheck { get => attackCheck; }

    [SerializeField] protected float attackCheckRadius;
    public float AttackCheckRadius { get => attackCheckRadius; }

    [SerializeField] protected float attackDistance;
    public float AttackDistance { get => attackDistance; }

    [SerializeField] protected float attackCooldown;
    public float AttackCooldown { get => attackCooldown; }

    [HideInInspector] public float lastTimeAttacked;

    protected bool isGrabbed = false;
    public bool IsGrabbed { get => isGrabbed; set => isGrabbed = value; }

    public StateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if(IsOutOfView() && isGrabbed)
        {
            Destroy(gameObject);
        }

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

    protected virtual bool IsOutOfView()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        return transform.position.x < min.x || transform.position.x > max.x ||
               transform.position.y < min.y || transform.position.y > max.y;
    }


    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected()
    => Physics2D.CircleCast(playerCheck.position, Radius, Vector2.down * facingDir, Distance, whatIsPlayer);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + AttackDistance * facingDir, transform.position.y));
    }
}