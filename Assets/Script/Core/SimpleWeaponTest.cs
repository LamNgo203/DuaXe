using UnityEngine;

/// <summary>
/// Script test đơn giản cho hệ thống vũ khí
/// 
/// CÁCH SỬ DỤNG:
/// 1. Thêm script này vào scene
/// 2. Gán bulletPrefab và enemyPrefab
/// 3. Nhấn T để test bắn đạn
/// 4. Nhấn E để spawn enemy test
/// 5. Nhấn A để spawn ammo test
/// </summary>
public class SimpleWeaponTest : MonoBehaviour
{
    [Header("Test Prefabs")]
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public GameObject ammoItemPrefab;
    
    [Header("Test Settings")]
    public KeyCode testFireKey = KeyCode.T;
    public KeyCode spawnEnemyKey = KeyCode.E;
    public KeyCode spawnAmmoKey = KeyCode.A;
    
    private SmartBikeController bikeController;
    
    private void Start()
    {
        // Tìm bike controller
        bikeController = FindObjectOfType<SmartBikeController>();
        if (bikeController == null)
        {
            Debug.LogWarning("Không tìm thấy SmartBikeController!");
        }
    }
    
    private void Update()
    {
        // Test bắn đạn
        if (Input.GetKeyDown(testFireKey))
        {
            if (bikeController != null)
            {
                bikeController.StartFiring();
                Debug.Log("Bắt đầu bắn đạn!");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy SmartBikeController!");
            }
        }
        
        // Spawn enemy test
        if (Input.GetKeyDown(spawnEnemyKey))
        {
            if (enemyPrefab != null)
            {
                Vector3 spawnPos = transform.position + Vector3.right * 5f;
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                
                // Đảm bảo có tag Enemy
                enemy.tag = "Enemy";
                
                // Đảm bảo có component SimpleEnemy
                if (enemy.GetComponent<SimpleEnemy>() == null)
                {
                    enemy.AddComponent<SimpleEnemy>();
                }
                
                Debug.Log("Đã spawn enemy test!");
            }
            else
            {
                Debug.LogWarning("Chưa gán enemyPrefab!");
            }
        }
        
        // Spawn ammo test
        if (Input.GetKeyDown(spawnAmmoKey))
        {
            if (ammoItemPrefab != null)
            {
                Vector3 spawnPos = transform.position + Vector3.left * 5f;
                Instantiate(ammoItemPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Đã spawn ammo test!");
            }
            else
            {
                Debug.LogWarning("Chưa gán ammoItemPrefab!");
            }
        }
        

    }
    
    private void OnGUI()
    {
        // Hiển thị hướng dẫn trên màn hình
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== WEAPON SYSTEM TEST ===");
        GUILayout.Label($"T: Test bắn đạn");
        GUILayout.Label($"E: Spawn enemy test");
        GUILayout.Label($"A: Spawn ammo test");
        GUILayout.Label($"");
        GUILayout.Label($"Cách sử dụng:");
        GUILayout.Label($"1. Nhấn A để spawn ammo");
        GUILayout.Label($"2. Chạy xe vào ammo để bắn");
        GUILayout.Label($"3. Xe sẽ tự động bắn 5 giây");
        GUILayout.Label($"4. Nhấn E để spawn enemy test");
        GUILayout.EndArea();
    }
    

} 