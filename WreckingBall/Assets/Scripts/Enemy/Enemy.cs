using System.Collections;
using UnityEngine;

public class Enemy : EnemyEntity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("스턴 정보")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;


    [Header("이동 정보")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;
    [SerializeField] protected float fallSpeed = 2f;
    private Transform playerTransform;

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
            playerTransform = hit.transform;
            MoveTowardPlayer();
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

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.CircleCast(playerCheck.position, 2.5f, Vector2.down * facingDir, 25, whatIsPlayer);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));

    }

    private void MoveTowardPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        }
    }

}