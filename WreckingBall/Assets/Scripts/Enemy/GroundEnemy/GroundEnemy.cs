using UnityEngine;

public class GroundEnemy : GroundEntity
{

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("이동 정보")]
    public float moveSpeed;
    public float idleTime;
    public float moveTime;

    [Header("전투 정보")]
    public float battleTime;      // 대기 시간
    public float battleRange;     // 추적 최대 거리
    public float chaseMultiplier; // 추격 시 속도 배수

    public GE_IdleState idleState { get; protected set; }
    public GE_MoveState moveState { get; protected set; }
    public GE_BattleState battleState { get; protected set; }

    public GroundEnemyStateMachine G_stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        G_stateMachine = new GroundEnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        G_stateMachine.G_currentState.Update();
    }

    public virtual void AnimationFinishTrigger() => G_stateMachine.G_currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, battleRange, whatIsPlayer);


}