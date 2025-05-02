using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WildWolf : Boss
{
    #region States

    [Header(nameof(WildWolf) + ".공동상태")]
    Boss_IdleState idleState;
    WildWolf_MoveState moveState;
    WildWolf_RunState runState;
    WildWolf_AttackState attackState;
    WildWolf_DamageState damageState;
    WildWolf_DeathState deathState;

    [Header(nameof(WildWolf) + ".1페이즈상태")]
    WildWolf_ThrowTrapState throwTrapState;
    WildWolf_FloorSlideState floorSlideState;
    WildWolf_JumpAttackState jumpAttackState; //통통 튕기기 공격 / 1.2페이즈 같은 공격

    [Header(nameof(WildWolf) + ".2페이즈상태")]
    WildWolf_AerialSlideState aerialSlideState;
    WildWolf_TakeDown_VAttackState vattackState;
    WildWolf_TakeDown_DirectAttackState directAttackState;
    WildWolf_DroppingTrapState droppingTrapState;

    #endregion

    #region Traps
    List<TrapBase> throwTraps = new();
    List<TrapBase> droppingTraps = new();
    #endregion


    #region 상속 메서드

    public WildWolf(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent, EntityAbnormalState knockback, EntityAbnormalState invincibility, int bossPage, int bossMaxPage, UnityAction<EntityAbility> pageChageEvent) : base(ability, damageEvent, deathEvent, knockback, invincibility, bossPage, bossMaxPage, pageChageEvent)
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        idleState = new Boss_IdleState(stateMachine, this, "Idle");
        moveState = new WildWolf_MoveState(stateMachine, this, "Move");
        runState = new WildWolf_RunState(stateMachine, this, "Run");
        attackState = new WildWolf_AttackState(stateMachine, this, "Attack");
        damageState = new WildWolf_DamageState(stateMachine, this, "Damage");
        deathState = new WildWolf_DeathState(stateMachine, this, "Death");

        throwTrapState = new WildWolf_ThrowTrapState(stateMachine, this, "ThrowTrap");
        floorSlideState = new WildWolf_FloorSlideState(stateMachine, this, "FloorSlide");
        jumpAttackState = new WildWolf_JumpAttackState(stateMachine, this, "JumpAttack");

        aerialSlideState = new WildWolf_AerialSlideState(stateMachine, this, "AerialSlide");
        vattackState = new WildWolf_TakeDown_VAttackState(stateMachine, this, "TakeDown_VAttack");
        directAttackState = new WildWolf_TakeDown_DirectAttackState(stateMachine, this, "TakeDown_DirectAttack");
        droppingTrapState = new WildWolf_DroppingTrapState(stateMachine, this, "DroppingTrap");
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Damage()
    {
        base.Damage();
    }

    public override void Flip()
    {
        base.Flip();
    }

    public override void FlipController(float _x)
    {
        base.FlipController(_x);
    }

    public override bool IsGroundDetected()
    {
        return base.IsGroundDetected();
    }

    public override bool IsWallDetected()
    {
        return base.IsWallDetected();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    #endregion

    #region 특수 메서드

    #endregion
}
