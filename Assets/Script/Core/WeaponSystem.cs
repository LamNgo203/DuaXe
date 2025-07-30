using UnityEngine;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Transform firePoint; // Vị trí nòng súng trên xe
    public GameObject bulletPrefab;
    public float fireRate = 0.5f; // Thời gian giữa các lần bắn
    public float bulletSpeed = 15f;
    public int maxAmmo = 10;
    public int currentAmmo;
    
    [Header("Targeting")]
    public float detectionRange = 8f; // Tầm phát hiện kẻ địch
    public LayerMask enemyLayer; // Layer của kẻ địch
    public bool autoFire = true; // Tự động bắn khi có mục tiêu
    
    [Header("Visual Effects")]
    public GameObject muzzleFlashPrefab;
    public AudioClip fireSound;
    
    [Header("UI")]
    public UnityEngine.UI.Text ammoText; // Text hiển thị số đạn (nếu có UI)
    
    private float nextFireTime = 0f;
    private List<Enemy> detectedEnemies = new List<Enemy>();
    private bool isStarted = false;
    
    private void Start()
    {
        currentAmmo = 0; // Bắt đầu với 0 đạn
        UpdateAmmoUI();
        
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
        isStarted = true;
    }
    
    private void HandleGameOver()
    {
        isStarted = false;
    }
    
    private void HandleGameReset()
    {
        isStarted = false;
        currentAmmo = 0;
        UpdateAmmoUI();
    }
    
    private void Update()
    {
        if (!isStarted || currentAmmo <= 0) return;
        
        // Tìm kẻ địch trong tầm
        DetectEnemies();
        
        // Tự động bắn nếu có mục tiêu
        if (autoFire && detectedEnemies.Count > 0 && Time.time >= nextFireTime)
        {
            FireAtNearestEnemy();
        }
    }
    
    private void DetectEnemies()
    {
        detectedEnemies.Clear();
        
        // Tìm tất cả enemy trong tầm
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                detectedEnemies.Add(enemy);
            }
        }
    }
    
    private void FireAtNearestEnemy()
    {
        if (detectedEnemies.Count == 0 || currentAmmo <= 0) return;
        
        // Tìm enemy gần nhất
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        
        foreach (Enemy enemy in detectedEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        
        if (nearestEnemy != null)
        {
            FireBullet(nearestEnemy.transform.position);
        }
    }
    
    private void FireBullet(Vector2 targetPosition)
    {
        if (bulletPrefab == null || firePoint == null || currentAmmo <= 0) return;
        
        // Tính hướng bắn
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        
        // Tạo đạn
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.SetDirection(direction);
            bullet.speed = bulletSpeed;
        }
        
        // Tạo hiệu ứng muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(muzzleFlash, 0.1f); // Tự hủy sau 0.1s
        }
        
        // Phát âm thanh
        if (fireSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.sfxSource.PlayOneShot(fireSound);
        }
        
        // Giảm đạn
        currentAmmo--;
        UpdateAmmoUI();
        
        // Cập nhật thời gian bắn tiếp theo
        nextFireTime = Time.time + fireRate;
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UpdateAmmoUI();
        
        // Hiệu ứng khi nhận đạn
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSound(1); // Giả sử index 4 là âm thanh nhận đạn
        }
    }
    
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
        }
    }
    
    // Hàm để bắn thủ công (có thể gọi từ input)
    public void ManualFire()
    {
        if (isStarted && currentAmmo > 0 && Time.time >= nextFireTime)
        {
            DetectEnemies();
            if (detectedEnemies.Count > 0)
            {
                FireAtNearestEnemy();
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Vẽ tầm phát hiện
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Vẽ vị trí nòng súng
        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);
        }
    }
} 