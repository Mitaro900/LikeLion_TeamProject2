using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bug2 : Enemy
{
    [Header("소환 관련")]
    public GameObject SpawnFlyPrefab;       // 소환할 미니언 프리팹
    public Transform summonPoint;


    public Bug2IdleState _idleState { get; private set; }
    public Bug2MoveState _moveState { get; private set; }
    public Bug2BattleState _battleState { get; private set; }
    public Bug2AttackState _attackState { get; private set; }
    public Bug2StunnedState _stunnedState { get; private set; }
    public Bug2DieState _dieState { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        _idleState = new Bug2IdleState(this, stateMachine, "Idle", this);
        _moveState = new Bug2MoveState(this, stateMachine, "Move", this);
        _battleState = new Bug2BattleState(this, stateMachine, "Move", this);
        _attackState = new Bug2AttackState(this, stateMachine, "Attack", this);
        _stunnedState = new Bug2StunnedState(this, stateMachine, "Stun", this);
        _dieState = new Bug2DieState(this, stateMachine, "Die", this);
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(_idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(_stunnedState);
            return true;
        }
        return false;
    }

    public override void Damage()
    {
        base.Damage();
        stateMachine.ChangeState(_dieState);
    }

    
}
