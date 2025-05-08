using UnityEngine;

public class SpawnAttack : MonoBehaviour
{
    private Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 5f;    //미사일 속도
    public float lifeTime = 10f; //미사일 생존 시간
    //public int damage = 10;     //미사일 데미지


    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Destroy(gameObject, lifeTime);  //일정 시간 후 미사일 제거     
    }

    

    void Update()
    {
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어랑 충돌!");
            Destroy(gameObject);
            // collision.GetComponent<Player>().TakeDamage(damage);
        }

    }
}
