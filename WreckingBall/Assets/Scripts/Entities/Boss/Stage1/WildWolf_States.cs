using UnityEngine;

/// <summary> 가만히 서있는 애니메이션 </summary>
public class Boss_IdleState : BossState
{
    public Boss_IdleState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 이동 애니메이션 </summary>
public class WildWolf_MoveState : BossState
{
    public WildWolf_MoveState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 달리기 애니메이션 </summary>
public class WildWolf_RunState : BossState
{
    public WildWolf_RunState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 공격 애니메이션 </summary>
public class WildWolf_AttackState : BossState
{
    public WildWolf_AttackState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 피해 입음 애니메이션 </summary>
public class WildWolf_DamageState : BossState
{
    public WildWolf_DamageState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 죽음 애니메이션 </summary>
public class WildWolf_DeathState : BossState
{
    public WildWolf_DeathState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 바닥 쓸기 공격 </summary>
public class WildWolf_FloorSlideState : BossState
{
    public WildWolf_FloorSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 통통 튕기기 공격 </summary>
public class WildWolf_JumpAttackState : BossState
{
    public WildWolf_JumpAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 트랩 던지기 공격 </summary>
public class WildWolf_ThrowTrapState : BossState
{
    public WildWolf_ThrowTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 공중 쓸기 공격 </summary>
public class WildWolf_AerialSlideState: BossState
{
    public WildWolf_AerialSlideState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 역V 찍기 공격 </summary>
public class WildWolf_TakeDown_VAttackState: BossState
{
    public WildWolf_TakeDown_VAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 트랩 떨어뜨리기 공격 </summary>
public class WildWolf_DroppingTrapState : BossState
{
    public WildWolf_DroppingTrapState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

/// <summary> 벽에서 대각으로 내려찍기 공격 </summary>
public class WildWolf_TakeDown_DirectAttackState : BossState
{
    public WildWolf_TakeDown_DirectAttackState(BossStateMachine stateMachine, Boss boss, string animBoolName) : base(stateMachine, boss, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}