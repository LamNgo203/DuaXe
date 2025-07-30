using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    [Header("Test Settings")]
    public KeyCode testKey = KeyCode.C;
    public GameObject testBulletPrefab;
    
    private void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            TestCollision();
        }
    }
    
    private void TestCollision()
    {
        Debug.Log("=== TESTING COLLISION ===");
        
        // Tạo bullet test
        if (testBulletPrefab != null)
        {
            Vector3 bulletPos = transform.position + Vector3.left * 2f;
            GameObject bullet = Instantiate(testBulletPrefab, bulletPos, Quaternion.identity);
            
            // Tạo enemy test
            Vector3 enemyPos = transform.position + Vector3.right * 2f;
            GameObject enemy = new GameObject("TestEnemy");
            enemy.transform.position = enemyPos;
            enemy.tag = "Enemy";
            enemy.layer = 8; // Layer 8 cho enemy
            
            // Thêm components cho enemy
            enemy.AddComponent<BoxCollider2D>();
            enemy.AddComponent<SpriteRenderer>();
            enemy.AddComponent<SimpleEnemy>();
            
            Debug.Log($"Tạo bullet tại {bulletPos}");
            Debug.Log($"Tạo enemy tại {enemyPos}");
            Debug.Log("Di chuyển bullet về phía enemy để test va chạm");
        }
        else
        {
            Debug.LogWarning("Chưa gán testBulletPrefab!");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"CollisionTest va chạm với: {other.gameObject.name}");
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 200, Screen.height - 100, 190, 90));
        GUILayout.Label("=== COLLISION TEST ===");
        GUILayout.Label($"C: Test Collision");
        GUILayout.Label($"Tạo bullet và enemy");
        GUILayout.Label($"để test va chạm");
        GUILayout.EndArea();
    }
} 