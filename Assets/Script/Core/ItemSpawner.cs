using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ammoItemPrefab;
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public float spawnRange = 10f;
    public LayerMask groundLayer;
    
    [Header("Spawn Areas")]
    public Transform[] spawnPoints; // Các điểm spawn cố định
    public bool useRandomSpawn = true; // Spawn ngẫu nhiên
    
    private float nextSpawnTime = 0f;
    private bool isSpawning = false;
    
    private void Start()
    {
        // Đăng ký với GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnGameOver += HandleGameOver;
            GameManager.Instance.OnGameReset += HandleGameReset;
        }
    }
    
    private void HandleGameStart()
    {
        isSpawning = true;
        nextSpawnTime = Time.time + spawnInterval;
    }
    
    private void HandleGameOver()
    {
        isSpawning = false;
    }
    
    private void HandleGameReset()
    {
        isSpawning = false;
        // Xóa tất cả item và enemy hiện tại
        ClearAllItems();
    }
    
    private void Update()
    {
        if (!isSpawning) return;
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomItem();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    private void SpawnRandomItem()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        if (spawnPosition != Vector3.zero)
        {
            // 70% cơ hội spawn ammo, 30% cơ hội spawn enemy
            float random = Random.Range(0f, 1f);
            
            if (random < 0.7f && ammoItemPrefab != null)
            {
                SpawnAmmoItem(spawnPosition);
            }
            else if (enemyPrefab != null)
            {
                SpawnEnemy(spawnPosition);
            }
        }
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints != null && spawnPoints.Length > 0 && !useRandomSpawn)
        {
            // Sử dụng spawn points cố định
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            return randomSpawnPoint.position;
        }
        else
        {
            // Spawn ngẫu nhiên trong tầm
            Vector3 playerPosition = Vector3.zero;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPosition = player.transform.position;
            }
            
            // Tìm vị trí spawn ngẫu nhiên
            for (int i = 0; i < 10; i++) // Thử tối đa 10 lần
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 randomPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * Random.Range(5f, spawnRange);
                
                // Kiểm tra xem vị trí có hợp lệ không (trên ground)
                RaycastHit2D hit = Physics2D.Raycast(randomPosition, Vector2.down, 2f, groundLayer);
                if (hit.collider != null)
                {
                    return new Vector3(randomPosition.x, randomPosition.y + 1f, 0);
                }
            }
        }
        
        return Vector3.zero;
    }
    
    private void SpawnAmmoItem(Vector3 position)
    {
        if (ammoItemPrefab != null)
        {
            GameObject ammoItem = Instantiate(ammoItemPrefab, position, Quaternion.identity);
            // Tự hủy sau 10 giây nếu không ai ăn
            Destroy(ammoItem, 10f);
        }
    }
    
    private void SpawnEnemy(Vector3 position)
    {
        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            // Tự hủy sau 15 giây nếu không bị giết
            Destroy(enemy, 15f);
        }
    }
    
    private void ClearAllItems()
    {
        // Xóa tất cả ammo items
        AmmoItem[] ammoItems = FindObjectsOfType<AmmoItem>();
        foreach (AmmoItem item in ammoItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        
        // Xóa tất cả enemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Vẽ tầm spawn
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        
        // Vẽ các spawn points
        if (spawnPoints != null)
        {
            Gizmos.color = Color.blue;
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
                }
            }
        }
    }
} 