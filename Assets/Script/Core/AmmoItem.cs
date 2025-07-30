using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int ammoValue = 5; // Số đạn nhận được khi ăn hộp đạn
    public GameObject fxPrefab; // Hiệu ứng khi ăn hộp đạn
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public float rotationSpeed = 50f; // Tốc độ xoay của hộp đạn
    
    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        // Xoay hộp đạn để tạo hiệu ứng
        if (spriteRenderer != null)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu xe (Player) chạm vào hộp đạn
        if (other.CompareTag("Player"))
        {
            // Tìm component SmartBikeController trên xe
            SmartBikeController bikeController = other.GetComponent<SmartBikeController>();
            if (bikeController != null)
            {
                // Bắt đầu bắn đạn
                bikeController.StartFiring();
                
                // Tạo hiệu ứng
                if (fxPrefab != null)
                {
                    Instantiate(fxPrefab, transform.position, Quaternion.identity);
                }
                
                // Phát âm thanh
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayGameSound(2); // Âm thanh ăn item
                }
                
                // Hủy hộp đạn
                Destroy(gameObject);
            }
        }
    }
} 