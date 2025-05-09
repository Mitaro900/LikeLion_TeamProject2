using UnityEngine;

public class SpikeTrap : TrapBase
{
    private Player player;
    public SpikeTrap(TrapProperty property) : base(property)
    {

    }

    private void Update()
    {
        if(prop.state.Contains(TrapProperty.TrapState.Idle))
        { 
            if(IsLookPlayer())
            {
                prop.state = new() { TrapProperty.TrapState.Move };
                anim.enabled = true;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDebug)
            Debug.Log(collision.gameObject.name);
        base.OnTriggerEnter2D(collision);
    }

    protected override void AnimationFinished()
    {
        base.AnimationFinished();
        if(anim != null)
        {
            anim.enabled = false;
            prop.state = new() { TrapProperty.TrapState.Idle };
        }
    }

    protected override void Start()
    {
        base.Start();
        player = FindFirstObjectByType<Player>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private bool IsLookPlayer()
    {
        Vector2 distance = player.transform.position - transform.position; 
        Vector2 offset = new Vector2(prop.sizeOffSet.x + transform.position.x + (prop.thisSize.x / 2f),
                    prop.sizeOffSet.x + transform.position.x - (prop.thisSize.x / 2f));
        if (isDebug)
        {
            //Debug.Log($"[{gameObject.name}] " + "Distance : " + new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y)) + " / Size : " + new Vector2(prop.thisSize.x, prop.thisSize.y) + " / Range : " + prop.moveDistance);
            //Debug.Log($"V3 Distance : {Vector3.Distance(player.transform.position, transform.position)}");
            //Debug.Log($"V2 Distance : {Vector2.Distance(player.transform.position, transform.position)}");

            Debug.Log($"[{gameObject.name}] offset : " + offset+" / pos : "+(Vector2)player.transform.position);
        }
        for (int i = 0; i < prop.trapDir.Length; i++)
        {
            if (prop.trapDir[i].x != 0)
            {
                //if (Mathf.Abs(distance.x) < prop.moveDistance && Mathf.Abs(distance.y) < Mathf.Abs(prop.thisSize.y / 2f + prop.sizeOffSet.y))
                //    return true;                    

                if (Mathf.Abs(distance.x) < prop.moveDistance 
                    && (player.transform.position.y < offset.x) 
                    && player.transform.position.y > offset.y)
                    return true;
            }

            if (prop.trapDir[i].y != 0)
            {
                //if (Mathf.Abs(distance.y) < prop.moveDistance && Mathf.Abs(distance.x) < Mathf.Abs(prop.thisSize.x / 2f + prop.sizeOffSet.x))
                //    return true;

                if(Mathf.Abs(distance.y) < prop.moveDistance
                    && (player.transform.position.x < offset.x)
                    && player.transform.position.x > offset.y)
                    return true;
            }
        }

        return false;
    }
}
