using UnityEngine;

/// <summary>
/// Hướng dẫn setup hệ thống vũ khí cho game Dua Xe
/// 
/// CÁCH THIẾT LẬP:
/// 
/// 1. TRÊN XE (Player):
///    - Thêm component WeaponSystem vào xe
///    - Tạo một Transform làm firePoint (vị trí nòng súng)
///    - Gán firePoint vào WeaponSystem
///    - Tạo bullet prefab và gán vào bulletPrefab
///    - Thiết lập enemyLayer (layer của kẻ địch)
///    
/// 2. BULLET PREFAB:
///    - Tạo GameObject với component Bullet
///    - Thêm Rigidbody2D (IsTrigger = true)
///    - Thêm Collider2D (IsTrigger = true)
///    - Thêm TrailRenderer cho hiệu ứng
///    - Thiết lập targetLayer (layer của enemy)
///    
/// 3. AMMO ITEM PREFAB:
///    - Tạo GameObject với component AmmoItem
///    - Thêm Collider2D (IsTrigger = true)
///    - Thêm SpriteRenderer
///    - Thiết lập ammoValue và fxPrefab
///    
/// 4. ENEMY PREFAB:
///    - Tạo GameObject với component Enemy
///    - Thêm Rigidbody2D và Collider2D
///    - Thiết lập health, moveSpeed, detectionRange
///    - Tạo waypoints cho patrol (nếu cần)
///    
/// 5. ITEM SPAWNER:
///    - Tạo GameObject với component ItemSpawner
///    - Gán ammoItemPrefab và enemyPrefab
///    - Thiết lập spawnInterval và spawnRange
///    
/// 6. UI:
///    - Tạo UI với component WeaponUI
///    - Gán các UI elements (Text, Image, Slider)
///    
/// LAYERS CẦN THIẾT LẬP:
/// - Ground: Đất, nền
/// - Deadzone: Vùng nguy hiểm
/// - Enemy: Kẻ địch
/// - Player: Xe (đã có sẵn)
/// 
/// TAGS CẦN THIẾT LẬP:
/// - Player: Cho xe
/// 
/// ĐIỀU KHIỂN:
/// - Space: Bắn thủ công
/// - Tự động bắn khi có enemy trong tầm
/// 
/// ÂM THANH:
/// - Index 0: Âm thanh ăn item
/// - Index 1: Âm thanh bắn trúng
/// - Index 2: Âm thanh enemy bị đánh
/// - Index 3: Âm thanh enemy chết
/// - Index 4: Âm thanh nhận đạn
/// </summary>
public class WeaponSystemSetup : MonoBehaviour
{
    [Header("Setup Instructions")]
    [TextArea(10, 20)]
    public string setupInstructions = @"
    HƯỚNG DẪN SETUP HỆ THỐNG VŨ KHÍ:
    
    1. TRÊN XE (Player):
       - Thêm component WeaponSystem vào xe
       - Tạo một Transform làm firePoint (vị trí nòng súng)
       - Gán firePoint vào WeaponSystem
       - Tạo bullet prefab và gán vào bulletPrefab
       - Thiết lập enemyLayer (layer của kẻ địch)
       
    2. BULLET PREFAB:
       - Tạo GameObject với component Bullet
       - Thêm Rigidbody2D (IsTrigger = true)
       - Thêm Collider2D (IsTrigger = true)
       - Thêm TrailRenderer cho hiệu ứng
       - Thiết lập targetLayer (layer của enemy)
       
    3. AMMO ITEM PREFAB:
       - Tạo GameObject với component AmmoItem
       - Thêm Collider2D (IsTrigger = true)
       - Thêm SpriteRenderer
       - Thiết lập ammoValue và fxPrefab
       
    4. ENEMY PREFAB:
       - Tạo GameObject với component Enemy
       - Thêm Rigidbody2D và Collider2D
       - Thiết lập health, moveSpeed, detectionRange
       - Tạo waypoints cho patrol (nếu cần)
       
    5. ITEM SPAWNER:
       - Tạo GameObject với component ItemSpawner
       - Gán ammoItemPrefab và enemyPrefab
       - Thiết lập spawnInterval và spawnRange
       
    6. UI:
       - Tạo UI với component WeaponUI
       - Gán các UI elements (Text, Image, Slider)
       
    LAYERS CẦN THIẾT LẬP:
    - Ground: Đất, nền
    - Deadzone: Vùng nguy hiểm
    - Enemy: Kẻ địch
    - Player: Xe (đã có sẵn)
    
    TAGS CẦN THIẾT LẬP:
    - Player: Cho xe
    
    ĐIỀU KHIỂN:
    - Space: Bắn thủ công
    - Tự động bắn khi có enemy trong tầm
    
    ÂM THANH:
    - Index 0: Âm thanh ăn item
    - Index 1: Âm thanh bắn trúng
    - Index 2: Âm thanh enemy bị đánh
    - Index 3: Âm thanh enemy chết
    - Index 4: Âm thanh nhận đạn
    ";
    
    [Header("Test Functions")]
    public bool enableTestFunctions = true;
    
    private void Start()
    {
        // Tự động ẩn script này trong build
        if (!Application.isEditor)
        {
            gameObject.SetActive(false);
        }
    }
    
    [ContextMenu("Test Weapon System")]
    public void TestWeaponSystem()
    {
        if (!enableTestFunctions) return;
        
        WeaponSystem weaponSystem = FindObjectOfType<WeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.AddAmmo(10);
            Debug.Log("Đã thêm 10 đạn vào vũ khí");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy WeaponSystem trong scene!");
        }
    }
    
    [ContextMenu("Spawn Test Enemy")]
    public void SpawnTestEnemy()
    {
        if (!enableTestFunctions) return;
        
        ItemSpawner spawner = FindObjectOfType<ItemSpawner>();
        if (spawner != null && spawner.enemyPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.right * 5f;
            Instantiate(spawner.enemyPrefab, spawnPos, Quaternion.identity);
            Debug.Log("Đã spawn enemy test");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ItemSpawner hoặc enemyPrefab!");
        }
    }
    
    [ContextMenu("Spawn Test Ammo")]
    public void SpawnTestAmmo()
    {
        if (!enableTestFunctions) return;
        
        ItemSpawner spawner = FindObjectOfType<ItemSpawner>();
        if (spawner != null && spawner.ammoItemPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.left * 5f;
            Instantiate(spawner.ammoItemPrefab, spawnPos, Quaternion.identity);
            Debug.Log("Đã spawn ammo test");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ItemSpawner hoặc ammoItemPrefab!");
        }
    }
} 