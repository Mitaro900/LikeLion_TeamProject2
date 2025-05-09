using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool isFlip;

    private float spawnTimer;
    private GameObject spawnedEnemy;

    private void Start()
    {
        spawnTimer = 0f;
        spawnedEnemy = null;
    }

    private void Update()
    {
        if(spawnedEnemy != null)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }
    private void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        if (isFlip)
        {
            spawnedEnemy.GetComponent<Enemy>().Flip();
        }
    }
}
