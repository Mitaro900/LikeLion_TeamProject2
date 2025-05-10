using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private BoxCollider2D cd;

    private void Start()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().UpdateCheckPoint(transform.position);
            cd.enabled = false;
        }
    }
}
