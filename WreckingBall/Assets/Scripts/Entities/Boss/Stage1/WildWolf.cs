using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WildWolf : Boss
{
    public Player player { get; private set; }


    #region States

    //[Header(nameof(WildWolf) + ".공동상태")]
    public WildWolf_RunState runState;
    public WildWolf_RunAttackState runAttackState;

    //[Header(nameof(WildWolf) + ".1페이즈상태")]
    public WildWolf_ThrowTrapState throwTrapState;
    public WildWolf_FloorSlideState floorSlideState;
    public WildWolf_JumpAttackState jumpAttackState; //통통 튕기기 공격 / 1.2페이즈 같은 공격

    //[Header(nameof(WildWolf) + ".2페이즈상태")]
    public WildWolf_AerialSlideState aerialSlideState;
    public WildWolf_TakeDown_VAttackState vattackState;
    public WildWolf_TakeDown_DirectAttackState directAttackState;
    public WildWolf_DroppingTrapState droppingTrapState;

    [HideInInspector] public WildWolf_PatternController controller;

    #endregion

    #region Traps
    [Header(nameof(WildWolf) + ".함정")]
    [SerializeField] private List<StringIntPair> trapsPrefab;
    private List<TrapBase> throwTraps = new();
    private List<TrapBase> droppingTraps = new();
    public Transform throwPos;
    public Transform dropPos;
    #endregion



    public WildWolf(EntityAbility ability, UnityAction<EntityAbility> damageEvent, UnityAction deathEvent, EntityAbnormalState knockback, EntityAbnormalState invincibility, int bossPage, int bossMaxPage, UnityAction<EntityAbility> pageChageEvent) : base(ability, damageEvent, deathEvent, knockback, invincibility, bossPage, bossMaxPage, pageChageEvent)
    {
        Debug.Log(nameof(WildWolf) + " Set");
        base.bossPage = bossPage;
        base.bossMaxPage = bossMaxPage;
        base.damageEvent = damageEvent;
        base.damageEvent += (ab) =>
        {
            controller.NextAction(true);
        };
        base.deathEvent = deathEvent;
        base.pageChageEvent = pageChageEvent;
        base.pageChageEvent += (ab) =>
        {
            if (sr.color == Color.white)
                sr.DOColor(Color.red, 0.5f);
        };
    }


    protected override void Start()
    {
        base.Start();

        runAttackState = new WildWolf_RunAttackState(stateMachine, this, "RunAttack", this);
        AddState(runAttackState);

        throwTrapState = new WildWolf_ThrowTrapState(stateMachine, this, "ThrowTrap", this, trapsPrefab[0].Key, trapsPrefab[0].Value);
        AddState(throwTrapState);
        floorSlideState = new WildWolf_FloorSlideState(stateMachine, this, "FloorSlide", this, Random.Range(2, 4));
        AddState(floorSlideState);
        jumpAttackState = new WildWolf_JumpAttackState(stateMachine, this, "JumpAttack", this, Random.Range(4, 6));
        AddState(jumpAttackState);

        aerialSlideState = new WildWolf_AerialSlideState(stateMachine, this, "AerialSlide", this, Random.Range(3, 6));
        AddState(aerialSlideState);
        vattackState = new WildWolf_TakeDown_VAttackState(stateMachine, this, "TakeDown_VAttack", this, Random.Range(3, 5));
        AddState(vattackState);
        directAttackState = new WildWolf_TakeDown_DirectAttackState(stateMachine, this, "TakeDown_DirectAttack", this, 3);
        AddState(directAttackState);
        droppingTrapState = new WildWolf_DroppingTrapState(stateMachine, this, "DroppingTrap", this, trapsPrefab[1].Key, trapsPrefab[1].Value);
        AddState(droppingTrapState);

        controller = GetComponent<WildWolf_PatternController>();
        controller.Initialize(this);

        if (TrapPoolManager.instance != null)
        {
            TrapPoolManager pool = TrapPoolManager.instance;
            throwTraps = new();
            droppingTraps = new();
            for (int i = 0; i < trapsPrefab.Count; i++)
            {
                List<GameObject> gos = pool.Call(trapsPrefab[i].Key, Vector3.zero, trapsPrefab[i].Value);
                if (gos != null)
                {
                    for (int j = 0; j < gos.Count; j++)
                    {
                        TrapBase trap = gos[j].GetComponent<TrapBase>();
                        if(trap.name.Contains("ThrowTrap"))
                            throwTraps.Add(trap);
                        else if(trap.name.Contains("DroppingTrap"))
                            droppingTraps.Add(trap);
                        else
                            Debug.LogError("트랩 이름이 잘못되었습니다.");
                    }
                }
            }
        }

        player = FindFirstObjectByType<Player>();

        //controller.NextAction();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.facingDir != facingDir)
                Damage();
        }
    }

}
