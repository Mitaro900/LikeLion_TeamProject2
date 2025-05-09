using UnityEngine;

public class Enemy_Mush : Enemy
{
    #region States

    public MushIdleState idleState { get; private set; }
    public MushMoveState moveState { get; private set; }
    public MushBattleState battleState { get; private set; }
    public MushAttackState attackState { get; private set; }
    public MushStunnedState stunnedState { get; private set; }
    public MushDieState dieState { get; private set; }

    #endregion

    [Header("감지 옵션")]
    [SerializeField] private float maxVerticalOffset = 1f;  // y차이가 이 값 이하일 때만 감지

    protected override void Awake()
    {
        base.Awake();

        idleState = new MushIdleState(this, stateMachine, "Idle", this);
        moveState = new MushMoveState(this, stateMachine, "Move", this);
        battleState = new MushBattleState(this, stateMachine, "Move", this);
        attackState = new MushAttackState(this, stateMachine, "Attack", this);
        stunnedState = new MushStunnedState(this, stateMachine, "Stun", this);
        dieState =new MushDieState(this, stateMachine, "Die", this);

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (IsGrabbed && stateMachine.currentState != stunnedState)
        {
            stateMachine.ChangeState(stunnedState);
            return;
        }
    }

    public override RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D hit = Physics2D.CircleCast(playerCheck.position, Radius, Vector2.right * facingDir, Distance, whatIsPlayer);
        
        if (hit.collider != null)
        {
            float dy = Mathf.Abs(hit.transform.position.y - transform.position.y);
            if (dy <= maxVerticalOffset)  // 예: 1.0f
            {
                return hit;
            }
        }

        return default;
    }

    public override bool CanBeStunned()
    {
        if( base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Damage()
    {
        base.Damage();
         stateMachine.ChangeState(dieState);
    }

     // 플레이어의 Dash/Bodyslam 상태로 충돌 시 즉시 DieState 진입
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // (1) 이미 Stun 상태라면 Dash/Bslam 로직 건너뛰기
        if (stateMachine.currentState == stunnedState) return;

        // 1) 충돌 대상이 Player 태그인지 확인
        if (!collision.collider.CompareTag("Player")) return;

        // 2) Player 컴포넌트 가져오기
        Player player = collision.collider.GetComponent<Player>();
        if (player == null) return;

        var currState = player.stateMachine.currentState;
        bool isDashHit = currState == player.dashState;

        bool isBslamHit = currState == player.bodyslamState && player.rb.linearVelocity.y < 0f;

        if (isDashHit || isBslamHit)
        {
            // 5) 즉시 사망 상태로 전환
            stateMachine.ChangeState(dieState);
        }
    }

     // Stun 상태일 때 Trigger 충돌로 사망 판정
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Stun 상태가 아니면 무시
        if (stateMachine.currentState != stunnedState) return;

        // 2) 다른 몬스터와 부딪힌 경우 → 둘 다 Die
        if (other.CompareTag("Enemy"))
        {
            var otherEnemy = other.GetComponent<Enemy_Mush>();
            stateMachine.ChangeState(dieState);
            if (otherEnemy != null)
                otherEnemy.stateMachine.ChangeState(otherEnemy.dieState);
            return;
        }

        // 3) 땅 또는 벽에 부딪힌 경우 → 자기만 Die
        if (other.CompareTag("Ground"))
            {
                stateMachine.ChangeState(dieState);
            }
    }

}
