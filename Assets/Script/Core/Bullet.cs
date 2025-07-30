using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 15f;
    public float lifetime = 3f; // Thời gian tồn tại của đạn
    public int damage = 1;
    public LayerMask targetLayer; // Layer của kẻ địch
    
    [Header("Movement Effects")]
    public bool useSimpleMotion = true; // Sử dụng chuyển động đơn giản
    public float upwardForce = 3f; // Lực phóng lên nhẹ
    public float gravity = 1f; // Trọng lực nhẹ
    
    [Header("Visual Effects")]
    public GameObject hitEffectPrefab;
    public TrailRenderer trailRenderer;
    
    private Vector2 direction;
    private float timer;
    private Vector3 startPosition;
    private Vector3 velocity;
    
    private void Start()
    {
        timer = 0f;
        startPosition = transform.position;
        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();
            
        Debug.Log($"Bullet được tạo tại {transform.position}");
        Debug.Log($"Bullet targetLayer: {targetLayer.value}");
        Debug.Log($"Bullet speed: {speed}, damage: {damage}");
    }
    
    private void Update()
    {
        if (useSimpleMotion)
        {
            UpdateSimpleMotion();
        }
        else
        {
            // Chuyển động thẳng đơn giản
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        
        // Tự hủy sau thời gian lifetime
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            DestroyBullet();
        }
    }
    
    private void UpdateSimpleMotion()
    {
        // Chuyển động đơn giản: bay thẳng với phóng lên nhẹ
        Vector3 movement = direction * speed * Time.deltaTime;
        
        // Thêm hiệu ứng phóng lên nhẹ
        float upwardMovement = upwardForce * timer - 0.5f * gravity * timer * timer;
        movement.y += upwardMovement * Time.deltaTime;
        
        // Cập nhật vị trí
        transform.position += movement;
        
        // Xoay bullet theo hướng di chuyển
        if (movement.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // Xoay đạn theo hướng di chuyển
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bullet va chạm với: {other.gameObject.name} (Tag: {other.tag})");
        
        // Kiểm tra nếu đạn chạm vào kẻ địch (đơn giản hơn)
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Đạn trúng enemy!");
            
            // Gây sát thương cho kẻ địch
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Tìm thấy SimpleEnemy, đang gây sát thương...");
                enemy.TakeDamage();

            }
            else
            {
                Debug.LogWarning("Không tìm thấy SimpleEnemy component!");
            }
            
            // Tạo hiệu ứng va chạm
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
            
            // Phát âm thanh
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayGameSound(6); // Âm thanh bắn trúng
            }
            
            // Hủy đạn
            DestroyBullet();
        }
        // Kiểm tra nếu đạn chạm vào ground hoặc deadzone
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                 other.gameObject.layer == LayerMask.NameToLayer("Deadzone"))
        {
            Debug.Log("Đạn chạm ground/deadzone, hủy đạn");
            DestroyBullet();
        }
        else
        {
            Debug.Log($"Đạn va chạm với {other.gameObject.name} (tag: {other.tag}) nhưng không phải enemy");
        }
    }
    
    private void DestroyBullet()
    {
        // Tắt trail renderer trước khi hủy
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        
        Destroy(gameObject);
    }
}