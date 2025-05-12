using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected bool isDebug;
    public enum ProjectileState { Idle, Running, Crushed, Destoring, None };
    [Header(nameof(ProjectileBase) + ".움직임 상태")]
    [SerializeField] protected ProjectileState state;
    private Rigidbody2D rb;
    public Animator anim { get; private set; }

    [Header(nameof(ProjectileBase)+".이동시 설정값")]
    [Tooltip("이동방향")]
    [SerializeField] protected Vector2 dir;
    [Tooltip("이동속도")]
    [SerializeField] protected float moveSpeed;
    [Tooltip("가속도")]
    [SerializeField] protected float acceleration;

    public void Setup(Vector2 dir, float speed, float acceleration = 0)
    {
        this.dir = dir;
        this.moveSpeed = speed;
        this.acceleration = acceleration;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void ChangeState(ProjectileState state, Vector2 dir = default, float speed = default, float acceleration = default)
    {
        this.state = state;
        if (dir != default)
            this.dir = dir;
        if(speed != default)
            this.moveSpeed = speed;
        if (acceleration != default)
            this.acceleration = acceleration;
    }

    protected virtual void Update()
    {
        if (state == ProjectileState.Idle)
            return;

        else if (state == ProjectileState.Running)
        {
            //trastorm 이용해 이동
            if (acceleration == 0)
            {
                Vector3 pos = transform.position;
                pos += (Vector3)dir * moveSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }
        // 충돌한 경우 멈춤
        else if(rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected virtual void FixedUpdate()
    {
        if(state == ProjectileState.Running && acceleration != 0)
        {
            //rb 이용해 가속하기
            rb.linearVelocity = dir * moveSpeed * Time.fixedDeltaTime * acceleration;
        }
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDebug)
            Debug.Log(gameObject.name+" Crush To : "+ collision.gameObject.name);
        if (state == ProjectileState.Destoring)
            return;
        
        if (collision.tag == "Ground" || collision.tag == "Wall" || collision.gameObject.name == "Ground" || collision.gameObject.name == "Wall")
        {
            ObjectDisable();
        }
        else if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().DamageImpact();
        }
    }

    private void ObjectDisable()
    {
        state = ProjectileState.Destoring;
        if (anim != null)
        {
            anim.SetBool("Destroy", true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void AnimationFinished()
    {
        string animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animName == "Destroy")
            gameObject.SetActive(false);
    }

    protected virtual void OnDisable() => TrapPoolManager.instance?.Rerurn(gameObject);
}
