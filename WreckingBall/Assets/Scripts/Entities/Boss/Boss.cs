using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary> 상태이상 </summary>
[System.Serializable]
public struct EntityAbnormalState
{
    public Vector2 direction;
    public float duration;
    public bool isChecked;
    public EntityAbnormalState(Vector2 direction, float duration, bool isChecked)
    {
        this.direction = direction;
        this.duration = duration;
        this.isChecked = isChecked;
    }
}

/// <summary> 능력치 </summary>
[System.Serializable]
public struct EntityAbility
{
    public int hp;
    public int maxHp;
    public float moveSpeed;
    public float jumpPower;

    public EntityAbility(int hp, int maxHp, float moveSpeed, float jumpPower)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.moveSpeed = moveSpeed;
        this.jumpPower = jumpPower;
    }
}

public class Boss : EntityCollision
{
    [Header(nameof(Boss) + ".능력치")]
    [SerializeField] protected EntityAbility ability;

    [SerializeField] protected int bossPage = 1;
    [SerializeField] protected int bossMaxPage = 1;
    protected UnityAction<EntityAbility> pageChageEvent;

    [SerializeField] protected bool isGiveDamagedAction = false;
    public Vector3 oriPos { get; protected set; }

    public BossStateMachine stateMachine;
    public Boss_IdleState idleState;
    public Boss_AttackState attackState;
    public Boss_DamageState damageState;
    public Boss_MoveState moveState;
    public Boss_DeathState deathState;
    public List<BossState> states { get; protected set; }

    protected UnityAction<EntityAbility> damageEvent;
    protected UnityAction deathEvent;
    protected UnityAction<bool> onBecameVisibleEvent;

    [SerializeField] protected Collider2D bg;

    public Boss(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent,
        EntityAbnormalState knockback, EntityAbnormalState invincibility, int bossPage, int bossMaxPage, UnityAction<EntityAbility> pageChageEvent)
        
    {
        this.ability = ability;
        this.damageEvent = damageEvent;
        this.deathEvent = deathEvent;

        base.knockbackState = knockback;
        base.invincibilityState = invincibility;
        this.bossPage = bossPage;
        this.bossMaxPage = bossMaxPage;
        this.pageChageEvent = pageChageEvent;
    }

    public int GetBossPage() => bossPage;
    public int GetBossMaxPage() => bossMaxPage;
    public EntityAbility GetAbility() => ability;

    public bool IsGiveDamagedAction() => isGiveDamagedAction;
    public void InitGiveDamagedAction() => isGiveDamagedAction = false;

    public virtual void Damage()
    {
        ability.hp -= 1;
        Debug.Log(nameof(Boss) + " " + nameof(Damage) + $" {ability.hp} / {ability.maxHp}");
        if (ability.hp <= 0)
        {
            if(bossPage >= bossMaxPage)
                deathEvent?.Invoke();
            
            else
            {
                bossPage+=1;
                ability.hp = ability.maxHp;
                pageChageEvent?.Invoke(ability);
            }
            
        }
        else
            damageEvent?.Invoke(ability);
        
    }


    protected override void Start()
    {
        base.Start();
        stateMachine = new BossStateMachine();

        this.damageEvent += (ab) =>
        {
            Debug.Log("hp : "+ab.hp);
            stateMachine.ChangeState(damageState);
        };

        idleState = new Boss_IdleState(stateMachine, this, "Idle");
        AddState(idleState);
        moveState = new Boss_MoveState(stateMachine, this, "Move");
        AddState(moveState);
        attackState = new Boss_AttackState(stateMachine, this, "Attack");
        AddState(attackState);
        damageState = new Boss_DamageState(stateMachine, this, "Damage");
        AddState(damageState);
        deathState = new Boss_DeathState(stateMachine, this, "Death");
        AddState(deathState);

        oriPos = transform.position;

        
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState?.Update();

        //Debug.Log(nameof(Boss)+" "+nameof(Update) + $" damaged : {isGiveDamagedAction}");
        //Debug.Log(nameof(Boss) + " " + nameof(Update) + " statemachine : " + (stateMachine.currentState != null ? $"{stateMachine.currentState.nowAnimName} / {stateMachine.currentState.animBoolName}" : "null"));
    }

    protected virtual void AnimationFinishTrigger()
    {
        stateMachine.currentState?.AnimationFinishTrigger();
        if(isGiveDamagedAction)
            isGiveDamagedAction = false;
    }

    public virtual void AddState(BossState state)
    {
        if (states == null)
            states = new();
        if(states.Contains(state))
            return;
        states.Add(state);
    }

    public virtual void RemoveState(BossState state)
    {
        if (states.Contains(state))
            states.Remove(state);
    }   

    public BossState GetState(string stateName)
    {
        foreach (var state in states)
        {
            if (state.animBoolName == stateName)
                return state;
        }
        return null;
    }

    public virtual void GiveDamagePoint() => isGiveDamagedAction = AttackCheck() != null;

    public void AddVisibleEvent(UnityAction<bool> onBecameVisibleEvent)
    {
        if (this.onBecameVisibleEvent == null)
            this.onBecameVisibleEvent = onBecameVisibleEvent;
        else
            this.onBecameVisibleEvent += onBecameVisibleEvent;
    }

    public void RemoveVisibleEvent(UnityAction<bool> onBecameVisibleEvent)
    {
        if(this.onBecameVisibleEvent == null)
            return;
        else
            this.onBecameVisibleEvent -= onBecameVisibleEvent;
    }

    //private void OnBecameVisible() => onBecameVisibleEvent?.Invoke(true);

    //private void OnBecameInvisible() => onBecameVisibleEvent?.Invoke(false);

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (bg != null && collision == bg)
            onBecameVisibleEvent?.Invoke(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bg != null && collision == bg)
            onBecameVisibleEvent?.Invoke(true);
    }
}
