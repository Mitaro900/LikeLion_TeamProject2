using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private DistanceJoint2D distanceJoint2D;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private LayerMask ropeLayer; // Ray가 충돌할 레이어
    [SerializeField] private float maxAnchorDistance = 8f;

    [Header("이동 정보")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float ropeSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float maxSpeed = 50f;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float RopeSpeed { get => ropeSpeed; set => ropeSpeed = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    private Coroutine ropeCo = null;
    private bool ropeAnimating = false;

    public bool isAchored { get => distanceJoint2D.enabled; set => distanceJoint2D.enabled = value; } // 물체에 매달려 있는지 여부

    #region States
    public StateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerAnchoredState anchoredState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle", this);
        moveState = new PlayerMoveState(this, stateMachine, "Move", this);
        jumpState = new PlayerJumpState(this, stateMachine, "Jump", this);
        fallState = new PlayerFallState(this, stateMachine, "Jump", this);
        dashState = new PlayerDashState(this, stateMachine, "Dash", this);
        anchoredState = new PlayerAnchoredState(this, stateMachine, "Anchored", this);
    }

    protected override void Start()
    {
        base.Start();

        distanceJoint2D = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint2D.enabled = false; // 초기에는 비활성화
        lineRenderer.positionCount = 0; // 라인 렌더러 초기화

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ReleaseRope();
        }

        if (Input.GetKeyDown(KeyCode.X) && !ropeAnimating)
        {
            LaunchRope();
        }
    }

    public void ReleaseRope()
    {
        if (ropeAnimating)
        {
            if (ropeCo != null) StopCoroutine(ropeCo);
            StopSwing();
        }
    }

    public void LaunchRope()
    {
        //바라보는 방향으로 45도 방향.
        Vector3 dir = Vector3.zero;
        if (facingRight) dir = new Vector3(1, 1, 0).normalized;
        else dir = new Vector3(-1, 1, 0).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, maxAnchorDistance, ropeLayer);
        bool find = false;
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.gameObject == this.gameObject) continue;
            find = true;
            ropeCo = StartCoroutine(ThrowRopeAnimSuccess(hit.point));
            break;
        }

        if (!find)
        {
            Vector2 endPos = transform.position + dir.normalized * maxAnchorDistance;
            ropeCo = StartCoroutine(ThrowRopeAnimFail(endPos));
        }
    }

    public void RopeAction(float _speed)
    {
        //캐릭터시선기준 AddForce.
        Vector2 anchorToPlayer = (Vector2)transform.position - distanceJoint2D.connectedAnchor;
        Vector2 tangent = new Vector2(-anchorToPlayer.y, anchorToPlayer.x).normalized;
        rb.AddForce(tangent * facingDir * _speed, ForceMode2D.Force);

        //스크린기준 AddForce.
        //rb.AddForce(new Vector2(moveInput.x * swingForce, 0), ForceMode2D.Force);
    }

    private void StartSwing(Vector2 anchorPoint)
    {
        distanceJoint2D.autoConfigureConnectedAnchor = false;
        distanceJoint2D.connectedAnchor = anchorPoint;
        distanceJoint2D.enabled = true;
    }

    private void StopSwing()
    {
        distanceJoint2D.enabled = false;
        ropeAnimating = false;
        lineRenderer.positionCount = 0; // 라인 렌더러 초기화
    }

    IEnumerator ThrowRopeAnimFail(Vector2 targetPos)
    {
        ropeAnimating = true;
        lineRenderer.positionCount = 2;
        Vector2 startPos = transform.position;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);

        float progress = 0f;
        while (progress < 1f)
        {
            startPos = transform.position;
            progress += Time.deltaTime * ropeSpeed;
            Vector2 curPos = Vector2.Lerp(startPos, targetPos, progress);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, curPos);
            yield return null;
        }

        progress = 0f;
        while (progress < 1f)
        {
            startPos = transform.position;
            progress += Time.deltaTime * ropeSpeed;
            Vector2 curPos = Vector2.Lerp(targetPos, startPos, progress);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, curPos);
            yield return null;
        }

        lineRenderer.positionCount = 0;
        ropeAnimating = false;
    }

    IEnumerator ThrowRopeAnimSuccess(Vector2 targetPos)
    {
        ropeAnimating = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);


        float progress = 0f;
        while (progress < 1f)
        {
            Vector2 startPos = transform.position;
            progress += Time.deltaTime * ropeSpeed;
            Vector2 curPos = Vector2.Lerp(startPos, targetPos, progress);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, curPos);
            yield return null;
        }

        StartSwing(targetPos);

        while (true)
        {
            lineRenderer.SetPosition(0, transform.position);
            yield return null;
        }
    }

    public void Damage()
    {

    }

    public override bool IsGroundDetected()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(cd.bounds.center, cd.bounds.size - new Vector3(0.01f, 0.01f, 0f), 0f, Vector2.down, groundCheckDistance, whatIsGround);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(cd.bounds.center + new Vector3(cd.bounds.extents.x, 0), Vector2.down * (cd.bounds.extents.y + groundCheckDistance), rayColor);
        Debug.DrawRay(cd.bounds.center - new Vector3(cd.bounds.extents.x, 0), Vector2.down * (cd.bounds.extents.y + groundCheckDistance), rayColor);
        Debug.DrawRay(cd.bounds.center - new Vector3(cd.bounds.extents.x, cd.bounds.extents.y + groundCheckDistance), Vector2.right * (cd.bounds.extents.x * 2), rayColor);
        return raycastHit.collider != null;
    }
}
