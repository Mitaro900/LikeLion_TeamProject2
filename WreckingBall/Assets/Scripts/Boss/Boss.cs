using UnityEngine;
using UnityEngine.Events;

public class Boss : EntityCollision
{
    public int bossPage {get; private set; }
    public int bossMaxPage {get; private set; }
    protected UnityAction<EntityAbility> pageChageEvent;

    protected BossStateMachine stateMachine;

    public Boss(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent,
        EntityAbnormalState knockback, EntityAbnormalState invincibility, int bossPage, int bossMaxPage, UnityAction<EntityAbility> pageChageEvent)
        : base(ability, damageEvent, deathEvent)
    {
        base.knockbackState = knockback;
        base.invincibilityState = invincibility;
        this.bossPage = bossPage;
        this.bossMaxPage = bossMaxPage;
        this.pageChageEvent = pageChageEvent;
    }

    public override void Damage()
    {
        base.Damage();
    }

    public override void FlipController(float _x)
    {
        base.FlipController(_x);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine = new BossStateMachine();
    }

    protected override void Update()
    {
        base.Update();
    }
}
