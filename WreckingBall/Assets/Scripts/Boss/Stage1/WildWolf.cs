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

    public WildWolf(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent, EntityAbnormalState knockback, EntityAbnormalState invincibility, int bossPage, int bossMaxPage, UnityAction<EntityAbility> pageChageEvent) : base(ability, damageEvent, deathEvent, knockback, invincibility, bossPage, bossMaxPage, pageChageEvent)
    {

    }

}
