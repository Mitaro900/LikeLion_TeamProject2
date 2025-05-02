using UnityEngine;

public class SpawnFly : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 10f;

    private Transform player;
    private Animator anim;
    //public int damage = 20;
    private bool isDead = false;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            FlipController(direction.x);
        }
    }

    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어랑 충돌!");
            isDead = true;
            anim.SetTrigger("Die");

            // collision.GetComponent<Player>().TakeDamage(damage);
        }
        
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}