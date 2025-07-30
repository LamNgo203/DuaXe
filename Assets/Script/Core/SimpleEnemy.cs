using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 3;
    public int currentHealth;
    
    [Header("Visual Effects")]
    public GameObject deathEffectPrefab;
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    public float damageFlashDuration = 0.1f;
    
    private float flashTimer = 0f;
    
    private void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        // Đảm bảo có tag Enemy
        gameObject.tag = "Enemy";
        
        Debug.Log($"Enemy {gameObject.name} được tạo với {currentHealth} máu");
    }
    
    private void Update()
    {
        // Xử lý hiệu ứng flash khi bị đánh
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log($"Enemy {gameObject.name} nhận {damage} sát thương!");
        
        currentHealth -= damage;
        
        // Hiệu ứng flash khi bị đánh
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            flashTimer = damageFlashDuration;
        }
        
        // Phát âm thanh
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayGameSound(2); // Âm thanh enemy bị đánh
        }
        
        Debug.Log($"Enemy {gameObject.name} còn {currentHealth}/{maxHealth} máu");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log($"Enemy {gameObject.name} chết!");
        
        // Tạo hiệu ứng chết
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Cộng điểm
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10); // Cộng 10 điểm khi giết enemy
        }
        
        // Phát âm thanh
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSound(1); // Âm thanh enemy chết
        }
        
        // Hủy enemy
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        // Vẽ vùng enemy để dễ nhìn
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
} 