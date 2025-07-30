using UnityEngine;

/// <summary>
/// Hướng dẫn setup nhanh cho hệ thống vũ khí
/// 
/// CÁCH SETUP NHANH:
/// 
/// 1. TRÊN XE (Player):
///    - Đảm bảo xe có tag "Player"
///    - Thêm SmartBikeController (đã có sẵn)
///    - Gán bulletPrefab vào SmartBikeController
///    - Thiết lập enemyLayer (layer của enemy)
///    
/// 2. BULLET PREFAB:
///    - Tạo GameObject với component Bullet
///    - Thêm Rigidbody2D (IsTrigger = true)
///    - Thêm Collider2D (IsTrigger = true)
///    - Thiết lập targetLayer (layer của enemy)
///    
/// 3. ENEMY PREFAB:
///    - Tạo GameObject với component SimpleEnemy
///    - Thêm Collider2D
///    - Đặt tag "Enemy"
///    - Thiết lập layer cho enemy
///    
/// 4. AMMO PREFAB:
///    - Tạo GameObject với component AmmoItem
///    - Thêm Collider2D (IsTrigger = true)
///    - Thêm SpriteRenderer
///    
/// 5. TEST:
///    - Thêm SimpleWeaponTest vào scene
///    - Thêm DebugHelper vào scene
///    - Gán các prefab cần thiết
///    - Chạy và test
/// </summary>
public class QuickSetup : MonoBehaviour
{
    [Header("Setup Instructions")]
    [TextArea(15, 25)]
    public string setupInstructions = @"
    HƯỚNG DẪN SETUP NHANH:
    
    1. TRÊN XE (Player):
       - Đảm bảo xe có tag 'Player'
       - Thêm SmartBikeController (đã có sẵn)
       - Gán bulletPrefab vào SmartBikeController
       - Thiết lập enemyLayer (layer của enemy)
       
    2. BULLET PREFAB:
       - Tạo GameObject với component Bullet
       - Thêm Rigidbody2D (IsTrigger = true)
       - Thêm Collider2D (IsTrigger = true)
       - Thiết lập targetLayer (layer của enemy)
       
    3. ENEMY PREFAB:
       - Tạo GameObject với component SimpleEnemy
       - Thêm Collider2D
       - Đặt tag 'Enemy'
       - Thiết lập layer cho enemy
       
    4. AMMO PREFAB:
       - Tạo GameObject với component AmmoItem
       - Thêm Collider2D (IsTrigger = true)
       - Thêm SpriteRenderer
       
    5. TEST:
       - Thêm SimpleWeaponTest vào scene
       - Thêm DebugHelper vào scene
       - Gán các prefab cần thiết
       - Chạy và test
       
    LAYERS CẦN THIẾT LẬP:
    - Ground: Đất, nền
    - Deadzone: Vùng nguy hiểm
    - Enemy: Kẻ địch (tạo layer mới)
    - Player: Xe (đã có sẵn)
    
    TAGS CẦN THIẾT LẬP:
    - Player: Cho xe
    - Enemy: Cho kẻ địch
    
    TEST KEYS:
    - T: Test bắn đạn
    - E: Spawn enemy test
    - A: Spawn ammo test
    - D: Debug scene info
    ";
    
    [Header("Common Issues")]
    [TextArea(10, 15)]
    public string commonIssues = @"
    CÁC VẤN ĐỀ THƯỜNG GẶP:
    
    1. Đạn không bắn:
       - Kiểm tra bulletPrefab đã gán chưa
       - Kiểm tra firePoint có được tạo không
       - Kiểm tra enemyLayer đã thiết lập chưa
       
    2. Đạn bắn nhưng không sát thương:
       - Kiểm tra enemy có tag 'Enemy' không
       - Kiểm tra enemy có component SimpleEnemy không
       - Kiểm tra enemy layer có trong targetLayer không
       
    3. Không tìm thấy enemy:
       - Kiểm tra enemy có tag 'Enemy' không
       - Kiểm tra enemy có trong scene không
       
    4. Ammo không hoạt động:
       - Kiểm tra ammo có component AmmoItem không
       - Kiểm tra xe có tag 'Player' không
       - Kiểm tra xe có SmartBikeController không
    ";
    
    private void Start()
    {
        // Tự động ẩn script này trong build
        if (!Application.isEditor)
        {
            gameObject.SetActive(false);
        }
    }
    
    [ContextMenu("Check Setup")]
    public void CheckSetup()
    {
        Debug.Log("=== CHECKING SETUP ===");
        
        // Kiểm tra Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("✓ Player found");
            SmartBikeController bikeController = player.GetComponent<SmartBikeController>();
            if (bikeController != null)
            {
                Debug.Log("✓ SmartBikeController found");
                if (bikeController.bulletPrefab != null)
                    Debug.Log("✓ bulletPrefab assigned");
                else
                    Debug.LogError("✗ bulletPrefab not assigned!");
            }
            else
            {
                Debug.LogError("✗ SmartBikeController not found!");
            }
        }
        else
        {
            Debug.LogError("✗ Player not found!");
        }
        
        // Kiểm tra Test scripts
        SimpleWeaponTest test = FindObjectOfType<SimpleWeaponTest>();
        if (test != null)
            Debug.Log("✓ SimpleWeaponTest found");
        else
            Debug.LogWarning("⚠ SimpleWeaponTest not found");
            
        DebugHelper debug = FindObjectOfType<DebugHelper>();
        if (debug != null)
            Debug.Log("✓ DebugHelper found");
        else
            Debug.LogWarning("⚠ DebugHelper not found");
            
        Debug.Log("=== SETUP CHECK COMPLETE ===");
    }
} 