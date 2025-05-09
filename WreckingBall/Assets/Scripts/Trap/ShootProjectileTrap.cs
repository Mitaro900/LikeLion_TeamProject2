using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileTrap : TrapBase
{
    private TrapPoolManager poolManager;
    private Player player;
    [Header(nameof(ShootProjectile) + ".Prefab(PoolManager)")]
    [SerializeField] List<GameObjectIntPair> projectiles;
    Dictionary<string, List<ProjectileBase>> proj = new();

    [Header(nameof(ShootProjectile)+".Shoot")]
    [SerializeField] Transform shootPos;
    [SerializeField] List<Vector2> shootDir = new();
    [SerializeField] List<float> shootSpeed = new();
    [SerializeField] List<float> shootAccel = new();
    [SerializeField] private float delay = 0.2f;

    public ShootProjectileTrap(TrapProperty property) : base(property)
    {

    }

    private void Awake()
    {
        poolManager = TrapPoolManager.instance;
        proj = new();
    }

    protected override void Start()
    {
        base.Start();
        player = FindFirstObjectByType<Player>();
        if (poolManager != null) 
        {
            proj.Clear();
            foreach (var item in projectiles)
            {
                List<GameObject> obj = poolManager.Call(item.Key.name, shootPos.position, item.Value);
                if (obj != null)
                {
                    List<ProjectileBase> list = new();
                    foreach (var p in obj)
                    {
                        if (p.GetComponent<ProjectileBase>() != null)
                            list.Add(p.GetComponent<ProjectileBase>());
                    }
                    proj.Add(item.Key.name, list);
                }
                
            }
        }
    }

    private void Update()
    {
        if(prop.state.Contains(TrapProperty.TrapState.Idle))
        {
            if(IsLookPlayer())
            {
                anim.enabled = true;
                prop.state = new() { TrapProperty.TrapState.Move };
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void AnimationFinished()
    {
        base.AnimationFinished();
        anim.enabled = false;
        prop.state = new() { TrapProperty.TrapState.Idle };
    }

    private bool IsLookPlayer()
    {
        Vector2 distance = player.transform.position - transform.position;
        if(isDebug)
        Debug.Log($"[{gameObject.name}] Distance : " + new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y)) + " / Size : " + new Vector2(prop.thisSize.x, prop.thisSize.y)+" / Range : "+prop.moveDistance);
        for (int i=0;i<prop.trapDir.Length;i++)
        {
            if (prop.trapDir[i].x != 0)
            {
                if (Mathf.Abs(distance.x) < prop.moveDistance && Mathf.Abs(distance.y) < Mathf.Abs(prop.thisSize.y / 2f + prop.sizeOffSet.y))
                    return true;
            }

            if (prop.trapDir[i].y != 0)
            {
                if (Mathf.Abs(distance.y) < prop.moveDistance && Mathf.Abs(distance.x) < Mathf.Abs(prop.thisSize.x / 2f + prop.sizeOffSet.x))
                    return true;
            }
        }

        return false;
    }

    private void Shoot()
    {
        List<string> names = new();
        foreach(var item in proj)
        {
            names.Add(item.Key);
        }

        int index = Random.Range(0, names.Count);
        StartCoroutine(ShootProjectile(proj[names[index]], index));
    }

    private IEnumerator ShootProjectile(List<ProjectileBase> objs, int index)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position = shootPos.position;
            objs[i].gameObject.SetActive(true);
            objs[i].ChangeState(ProjectileBase.ProjectileState.Running, shootDir[index], shootSpeed.Count <= i ? 0 : shootSpeed[index], shootAccel.Count <= i ? 0 : shootAccel[index]);
            yield return wait;
        }
    }
}
