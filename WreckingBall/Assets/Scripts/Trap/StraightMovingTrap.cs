using PKR;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class StraightMovingTrap : TrapBase
{
    public enum MoveTrapState { Idle, Rushing, Returning }
    public MoveTrapState moveState { get; private set; }

    [SerializeField] private bool isHorizontalMoved;
    [SerializeField] private bool isVerticalMoved;

    [SerializeField] private bool isAutoMove = false;

    private Vector3 oriPos;
    private Player player;
    private Vector2 movingDir;

    public StraightMovingTrap(TrapProperty property) : base(property)
    {
        base.prop = property;
    }

    protected override void Start()
    {
        base.Start();
        oriPos = transform.position;
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        if (moveState == MoveTrapState.Idle)
        {
            if (isAutoMove && IsLookPlayer())
            {
                movingDir = PlayerDirection();
                pos += (Vector3) movingDir * prop.moveSpeed * Time.deltaTime;
                transform.position = pos;
                moveState = MoveTrapState.Rushing;
            }
        }
        else if(moveState == MoveTrapState.Rushing)
        {
            if (Vector2.Distance(pos, oriPos) >= prop.moveDistance)
            {
                moveState = MoveTrapState.Returning;
                movingDir *= -1;
            }
            else
            {
                pos += (Vector3)(movingDir * prop.moveSpeed * Time.deltaTime);
                transform.position = pos;
            }
        }
        else if(moveState == MoveTrapState.Returning)
        {
            if(Vector2.Distance(transform.position, oriPos) < prop.returnSpeed * Time.deltaTime)
            {
                pos = oriPos;
                transform.position = pos;
                moveState = MoveTrapState.Idle;
                prop.state = new() { TrapProperty.TrapState.Idle };
            }
            else
            {
                pos += (Vector3)(movingDir * prop.returnSpeed * Time.deltaTime);
                transform.position = pos;
            }
        }
    }

    public void MoveCall(Vector2 dir)
    {
        movingDir = dir;
        moveState = MoveTrapState.Rushing;
    }

    private bool IsLookPlayer()
    {
        Vector2 distance = player.transform.position - oriPos;
        //Debug.Log(new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y)) + " / "+new Vector2(prop.thisSize.x, prop.thisSize.y));
        if(isHorizontalMoved)
        {
            if(Mathf.Abs(distance.x) < prop.moveDistance && Mathf.Abs(distance.y) < Mathf.Abs(prop.thisSize.y / 2f + prop.sizeOffSet.y))
                return true;
        }
        
        if(isVerticalMoved)
        {
            if (Mathf.Abs(distance.y) < prop.moveDistance && Mathf.Abs(distance.x) < Mathf.Abs(prop.thisSize.x / 2f + prop.sizeOffSet.x))
                return true;
        }
        
        return false;
    }

    private Vector2 PlayerDirection()
    {
        Vector2 v2 = player.transform.position - oriPos;
        //좌우 이동
        if (isHorizontalMoved && Mathf.Abs(v2.x) < prop.moveDistance && Mathf.Abs(v2.y) < Mathf.Abs(prop.thisSize.y / 2f + prop.sizeOffSet.y))
            return new Vector2(v2.x > 0f ? 1 : -1, 0);
        //상하 이동
        else if (isVerticalMoved && Mathf.Abs(v2.y) < prop.moveDistance && Mathf.Abs(v2.x) < Mathf.Abs(prop.thisSize.x / 2f + prop.sizeOffSet.x))
            return new Vector2(0, v2.y > 0f ? 1 : -1);
        //이동X
        else
            return Vector2.zero;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null) Debug.Log(collision.gameObject.name);

        base.OnTriggerEnter2D(collision);

        if(collision.gameObject.name == "Ground")
        {
            moveState = MoveTrapState.Returning;
            movingDir *= -1;
        }
    }
}
