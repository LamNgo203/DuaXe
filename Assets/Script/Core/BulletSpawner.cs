using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Prefab")]
    public GameObject bulletPrefab;
    
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public float bulletSpeed = 15f;
    public float bulletLifetime = 3f;
    public int bulletDamage = 1;
    public LayerMask targetLayer;
    
    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public GameObject muzzleFlashPrefab;
    public AudioClip fireSound;
    
    public static BulletSpawner Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public GameObject SpawnBullet(Vector3 position, Vector2 direction)
    {
        if (bulletPrefab == null) return null;
        
        // Tạo bullet
        GameObject bulletObj = Instantiate(bulletPrefab, position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            // Cấu hình bullet
            bullet.speed = bulletSpeed;
            bullet.lifetime = bulletLifetime;
            bullet.targetLayer = targetLayer;
            bullet.hitEffectPrefab = hitEffectPrefab;
            
            // Set hướng
            bullet.SetDirection(direction);
        }
        
        // Tạo muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, position, Quaternion.identity);
            Destroy(muzzleFlash, 0.1f);
        }
        
        // Phát âm thanh
        if (fireSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.sfxSource.PlayOneShot(fireSound);
        }
        
        return bulletObj;
    }
    
    public GameObject SpawnBulletAtSpawnPoint(Vector2 direction)
    {
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        return SpawnBullet(spawnPosition, direction);
    }
    
    // Hàm tiện ích để tạo bullet với các thông số tùy chỉnh
    public GameObject SpawnBulletWithCustomSettings(Vector3 position, Vector2 direction, float speed, float lifetime, int damage)
    {
        if (bulletPrefab == null) return null;
        
        GameObject bulletObj = Instantiate(bulletPrefab, position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.speed = speed;
            bullet.lifetime = lifetime;
            bullet.targetLayer = targetLayer;
            bullet.hitEffectPrefab = hitEffectPrefab;
            bullet.SetDirection(direction);
        }
        
        return bulletObj;
    }
} 