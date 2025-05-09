using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("이동 정보")]
    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed { get => moveSpeed; }
    [SerializeField] private float jumpForce = 9f;
    public float JumpForce { get => jumpForce; }
    [SerializeField] private float acceleration = 7f;
    public float Acceleration { get => acceleration; }
    [SerializeField] private float dashSpeedThereshold = 15f;
    public float DashSpeedThereshold { get => dashSpeedThereshold; }
    [SerializeField] private float maxSpeed = 25f;
    public float MaxSpeed { get => maxSpeed; }
    private bool isAccelerating = false;
    public bool IsAccelerating { get => isAccelerating; set => isAccelerating = value; }
    private bool isOverSpeedThreshold = false;
    public bool IsOverSpeedThreshold { get => isOverSpeedThreshold; set => isOverSpeedThreshold = value; }

    [Header("중력 정보")]
    [SerializeField] private float defaultGravityScale = 2.5f;
    public float DefaultGravityScale { get => defaultGravityScale; }
    [SerializeField] private float jumpGravityScale = 1.0f;
    public float JumpGravityScale { get => jumpGravityScale; }

    [Header("로프 정보")]
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float ropeSpeed = 10f;
    public float RopeSpeed { get => ropeSpeed; }
    [SerializeField] private float minRopeDistance = 1.25f;
    [SerializeField] private float maxRopeDistance = 8f;

    private Coroutine ropeCo = null;
    private bool isRopeActive = false;
    public bool IsRopeActive { get => isRopeActive; set => isRopeActive = value; } // 로프가 활성화 되어 있는지 여부
    public bool IsAnchored { get => distanceJoint.enabled; set => distanceJoint.enabled = value; } // 물체에 매달려 있는지 여부
    public bool isPull { get; private set; }

    private bool isBusy = false;
    public bool IsBusy { get => isBusy; set => isBusy = value; }

    public GameObject grabbedObject { get; private set; }


    #region States
    public StateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerTurnState turnState { get; private set; }
    public PlayerAnchoredState anchoredState { get; private set; }
    public PlayerGrabState grabState { get; private set; }
    public PlayerBodyslamState bodyslamState { get; private set; }
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
        turnState = new PlayerTurnState(this, stateMachine, "Dash", this);
        anchoredState = new PlayerAnchoredState(this, stateMachine, "Anchored", this);
        grabState = new PlayerGrabState(this, stateMachine, "Anchored", this);
        bodyslamState = new PlayerBodyslamState(this, stateMachine, "Bodyslam", this);
    }

    protected override void Start()
    {
        base.Start();

        distanceJoint = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint.enabled = false; // 초기에는 비활성화
        lineRenderer.positionCount = 0; // 라인 렌더러 초기화

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (isOverSpeedThreshold)
        {
            fx.InvokeRepeating("OrangeColorBlink", 0f, 0.3f);
        }
        else
        {
            fx.CancelColorChange();
        }
    }

    public void ReleaseRope()
    {
        if (isRopeActive)
        {
            isPull = false;
            StopCoroutine("PullPlayerRope");
            StopSwing();
        }
    }

    public void LaunchRope(float xInput, float yInput)
    {
        //바라보는 방향으로 45도 방향.
        Vector3 dir = new Vector2(facingDir, 1).normalized;

        if (xInput != 0 || yInput != 0)
        {
            dir = new Vector2(xInput, yInput).normalized;
        }

        Vector2 endPos = transform.position + dir.normalized * maxRopeDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, maxRopeDistance);
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.gameObject == this.gameObject) continue;
            if (hit.collider.CompareTag("Ropeable"))
            {
                ropeCo = StartCoroutine(AnchorRope(hit.point));
                return;
            }
            else if(hit.collider.CompareTag("Enemy"))
            {
                ropeCo = StartCoroutine(PullEnemyRope(hit.point, hit.collider.gameObject));
                return;
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                endPos = hit.point;
                break;
            }
        }

        ropeCo = StartCoroutine(ThrowRope(endPos));
    }

    public void RopeAction(float _speed)
    {
        //캐릭터시선기준 AddForce.
        Vector2 anchorToPlayer = (Vector2)transform.position - distanceJoint.connectedAnchor;
        Vector2 tangent = new Vector2(-anchorToPlayer.y, anchorToPlayer.x).normalized;
        rb.AddForce(tangent * facingDir * _speed, ForceMode2D.Force);

        //스크린기준 AddForce.
        //rb.AddForce(new Vector2(moveInput.x * swingForce, 0), ForceMode2D.Force);
    }

    public void PullRope()
    {
        isPull = true;
        StartCoroutine("PullPlayerRope", distanceJoint.distance);
    }

    public void ThrowObject(float xInput, float yInput)
    {
        if (grabbedObject == null)
            return;

        grabbedObject.transform.SetParent(null);
        grabbedObject.layer = LayerMask.NameToLayer("Default");
        Enemy enemyScript = grabbedObject.GetComponent<Enemy>();
        enemyScript.cd.isTrigger = true;
        enemyScript.cd.enabled = true;

        Vector3 dir = Vector3.right * facingDir;

        if(xInput != 0 || yInput != 0)
        {
            dir = new Vector2(xInput, yInput).normalized;
        }

        enemyScript.rb.linearVelocity = dir * 25f;

        grabbedObject = null;
    }

    private void StartSwing(Vector2 anchorPoint)
    {
        distanceJoint.autoConfigureConnectedAnchor = false;
        distanceJoint.connectedAnchor = anchorPoint;
        distanceJoint.enabled = true;
    }

    private void StopSwing()
    {
        if (ropeCo != null) StopCoroutine(ropeCo);

        distanceJoint.enabled = false;
        isRopeActive = false;
        lineRenderer.positionCount = 0; // 라인 렌더러 초기화
    }

    private IEnumerator ThrowRope(Vector2 targetPos)
    {
        isRopeActive = true;
        lineRenderer.positionCount = 2;
        Vector2 startPos = transform.position;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, targetPos);

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
        isRopeActive = false;
    }

    private IEnumerator AnchorRope(Vector2 targetPos)
    {
        isRopeActive = true;
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
            lineRenderer.SetPosition(1, targetPos);
            yield return null;
        }
    }

    private IEnumerator PullEnemyRope(Vector2 targetPos, GameObject enemy)
    {
        isRopeActive = true;
        lineRenderer.positionCount = 2;
        Vector2 startPos = transform.position;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, targetPos);
        Enemy enemyScript = enemy.GetComponent<Enemy>();

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

        enemyScript.IsGrabbed = true;
        enemyScript.rb.bodyType = RigidbodyType2D.Kinematic;
        enemyScript.cd.enabled = false;

        progress = 0f;
        while (progress < 1f)
        {
            startPos = transform.position;
            progress += Time.deltaTime * ropeSpeed;
            Vector2 curPos = Vector2.Lerp(targetPos, startPos, progress);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, curPos);
            enemy.transform.position = curPos;
            yield return null;
        }

        lineRenderer.positionCount = 0;
        isRopeActive = false;

        grabbedObject = enemy;
        grabbedObject.transform.SetParent(transform);
        grabbedObject.transform.localPosition = new Vector2(0.5f, 0f);
        stateMachine.ChangeState(grabState);
    }

    private IEnumerator PullPlayerRope(float startDistance)
    {
        while(distanceJoint.distance > minRopeDistance)
        {
            distanceJoint.distance -= Time.deltaTime * ropeSpeed;
            yield return null;
        }
        distanceJoint.distance = minRopeDistance;
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
