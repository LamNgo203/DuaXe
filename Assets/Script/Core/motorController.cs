using UnityEngine;
using GameManagerNamespace = master; // Đảm bảo import đúng namespace nếu có

public class SmartBikeController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D rb;
    public Collider2D backWheelCollider;
    public Collider2D frontWheelCollider;
    public Collider2D humanCollider;
    public ParticleSystem fxPrefab;
    public LayerMask groundLayer;
    public Transform frontWheel, backWheel;

    [Header("Movement")]
    public float moveForce = 10f;
    public float rotateForce = 5f;
    public float maxAngularVelocity = 100f;        // Giới hạn tốc độ xoay (độ/giây)
    public float angularDamping = 5f;              // Mức độ giảm xoay khi thả
    public float airTimeThreshold = 0.3f;

    [Header("Weapon System")]
    public Transform firePoint; // Vị trí nòng súng trên xe
    public GameObject bulletPrefab; // Prefab của đạn
    public float bulletSpeed = 15f;
    public float bulletLifetime = 3f;
    public int bulletDamage = 1;
    
    [Header("Audio")]
    public int engineSoundIndex = 5; // Index của âm thanh xe trong SoundManager
    
    private bool isFiring = false;
    private float fireTimer = 0f;
    private float fireDuration = 5f; // Thời gian bắn (5 giây)
    private float fireInterval = 1f; // Khoảng cách giữa các viên đạn (1 giây)
    private float lastFireTime = 0f;
    private float nextFireTime = 0f; // Thời điểm bắn viên đạn tiếp theo

    private float lastGroundTime;
    private bool isStarted = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public ParticleSystem smokeTrailPrefab; // Hiệu ứng khói đuôi
    
    // Âm thanh xe
    private bool isEngineSoundPlaying = false;
    private bool shouldPlayEngineSound = false;
    public bool isMotorOn;
    public Transform gunDirection;
    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        // Tìm firePoint nếu chưa được gán
        if (firePoint == null)
        {
            // Tạo firePoint nếu chưa có
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(transform);
            firePointObj.transform.localPosition = new Vector3(1f, 0.5f, 0); // Vị trí nòng súng
            firePoint = firePointObj.transform;
        }
        
        // Không cần setup AudioSource nữa, sẽ dùng SoundManager
        
        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGameOver += HandleGameOverOrReset;
        GameManager.Instance.OnGameReset += HandleGameOverOrReset;
    }
    
    private void HandleGameStart()
    {
        StartMotor();
    }

    private void HandleGameOverOrReset()
    {
        ResetMotor(initialPosition, initialRotation);
        StopFiring(); // Dừng bắn khi game over hoặc reset
        
        // Dừng âm thanh xe
        if (isEngineSoundPlaying && SoundManager.Instance != null)
        {
            SoundManager.Instance.StopEngineSound();
            isEngineSoundPlaying = false;
        }
    }

    public void StartMotor()
    {
        isStarted = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ResetMotor(Vector3 position, Quaternion rotation)
    {
        isStarted = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = position;
        transform.rotation = rotation;
        
        // Active lại gameObject nếu nó bị tắt
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (!isStarted) return;
        isMotorOn = false;
        bool isOnGround =
            frontWheelCollider.IsTouchingLayers(groundLayer) ||
            backWheelCollider.IsTouchingLayers(groundLayer);

        if (isOnGround)
        {
            lastGroundTime = Time.time;
        }

        float airTime = Time.time - lastGroundTime;
        
        if (Input.GetMouseButton(0))
        {
            isMotorOn = true;
            if (airTime < airTimeThreshold)
            {
                Vector2 boostDir = (transform.right + new Vector3(0, -0.4f, 0)).normalized;
                rb.AddForce(boostDir * moveForce, ForceMode2D.Force);
            }
            else
            {
                if (Mathf.Abs(rb.angularVelocity) < maxAngularVelocity)
                {
                    rb.AddTorque(rotateForce, ForceMode2D.Force);
                }
            }
        }
        else
        {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, angularDamping * Time.fixedDeltaTime);
        }
        if (isMotorOn)
        {
            // Phát âm thanh xe nếu chưa phát
            if (!isEngineSoundPlaying && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayEngineSound(engineSoundIndex);
                isEngineSoundPlaying = true;
            }
        }
        else
        {
            // Dừng âm thanh xe nếu đang phát
            if (isEngineSoundPlaying && SoundManager.Instance != null)
            {
                SoundManager.Instance.StopEngineSound();
                isEngineSoundPlaying = false;
            }
        }
        // if(Input.GetMouseButtonUp(0))
        // {
        //     smokeTrailPrefab.Stop();
            
        //     // Dừng âm thanh xe khi thả tay
        //     if (isEngineSoundPlaying)
        //     {
        //         SoundManager.Instance.StopEngineSound();
        //         isEngineSoundPlaying = false;
        //     }
        // }
        // if(Input.GetMouseButtonDown(0))
        // {
        //     smokeTrailPrefab.Play();
        //     if (!isEngineSoundPlaying && SoundManager.Instance != null)
        //     {
        //         SoundManager.Instance.PlayEngineSound(engineSoundIndex);
        //         isEngineSoundPlaying = true;
        //     }
        // }
        
        // Tiếp tục phát âm thanh khi giữ chuột
        // if(Input.GetMouseButton(0) && !isEngineSoundPlaying && SoundManager.Instance != null)
        // {
        //     SoundManager.Instance.PlayEngineSound(engineSoundIndex);
        //     isEngineSoundPlaying = true;
        // }

        // Xử lý bắn đạn
        if (isFiring)
        {
            fireTimer += Time.deltaTime;

            // Kiểm tra xem có nên bắn viên đạn tiếp theo không
            if (Time.time >= nextFireTime)
            {
                Debug.Log($"Bắn đạn! fireTimer: {fireTimer:F2}s, thời gian còn lại: {fireDuration - fireTimer:F2}s");
                FireBullet();
                lastFireTime = Time.time;
                nextFireTime = Time.time + fireInterval; // Tính thời điểm bắn viên tiếp theo
            }

            // Dừng bắn sau 5 giây
            if (fireTimer >= fireDuration)
            {
                Debug.Log("Đã hết thời gian bắn, dừng bắn!");
                StopFiring();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            other.gameObject.layer == LayerMask.NameToLayer("Deadzone") ||
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (fxPrefab != null)
            {
                var a = Instantiate(fxPrefab, transform.position, Quaternion.identity);
                a.Play();
            }
            SoundManager.Instance.StopEngineSound();
            SoundManager.Instance.PlayGameSound(7); 
            gameObject.SetActive(false);
            UIManager.Instance.GetUiActive<UIGameplay>(UIName.UIGameplay).pauseButton.gameObject.SetActive(false);
            Debug.Log("Motor stopped due to collision with " + LayerMask.LayerToName(other.gameObject.layer));
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
    
    // Bắt đầu bắn đạn
    public void StartFiring()
    {
        Debug.Log("StartFiring() được gọi!");
        isFiring = true;
        fireTimer = 0f;
        lastFireTime = Time.time;
        nextFireTime = Time.time; // Bắn viên đầu tiên ngay lập tức
    }
    
    // Dừng bắn đạn
    private void StopFiring()
    {
        Debug.Log("StopFiring() được gọi!");
        isFiring = false;
        fireTimer = 0f;
        nextFireTime = 0f;
    }
    
    // Bắn một viên đạn
    private void FireBullet()
    {
        Debug.Log("FireBullet() được gọi!");
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab chưa được gán!");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("firePoint chưa được gán!");
            return;
        }
        
        // Tìm enemy gần nhất
        GameObject nearestEnemy = FindNearestEnemy();
        Vector2 fireDirection;
        
        if (nearestEnemy != null)
        {
            // Bắn về phía enemy gần nhất
            fireDirection = (nearestEnemy.transform.position - firePoint.position).normalized;
            Debug.Log($"Bắn về enemy tại {nearestEnemy.transform.position}");
        }
        else
        {
            // Bắn theo hướng xe đang di chuyển
            fireDirection = transform.up;
            Debug.Log("Không tìm thấy enemy, bắn theo hướng xe");
        }
        
        // Cập nhật hướng của gunDirection theo hướng bắn
        if (gunDirection != null)
        {
            // Tính góc quay từ Vector2 fireDirection
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            gunDirection.rotation = Quaternion.Euler(0, 0, angle);
            Debug.Log($"Đã quay gunDirection theo góc {angle} độ");
        }
        else
        {
            Debug.LogWarning("gunDirection chưa được gán!");
        }
        
        // Tạo đạn
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Debug.Log($"Đã tạo bullet tại {firePoint.position}");
        
        MonoBehaviour bullet = bulletObj.GetComponent<MonoBehaviour>();
        
        if (bullet != null && bullet.GetType().Name == "Bullet")
        {
            Debug.Log("Tìm thấy component Bullet, đang cấu hình...");
            // Set các thuộc tính thông qua reflection
            var speedField = bullet.GetType().GetField("speed");
            var lifetimeField = bullet.GetType().GetField("lifetime");
            var damageField = bullet.GetType().GetField("damage");
            var setDirectionMethod = bullet.GetType().GetMethod("SetDirection");
            
            if (speedField != null) speedField.SetValue(bullet, bulletSpeed);
            if (lifetimeField != null) lifetimeField.SetValue(bullet, bulletLifetime);
            if (damageField != null) damageField.SetValue(bullet, bulletDamage);
            if (setDirectionMethod != null) setDirectionMethod.Invoke(bullet, new object[] { fireDirection });
            
            Debug.Log("Đã cấu hình bullet thành công!");
        }
        else
        {
            Debug.LogError("Không tìm thấy component Bullet trên bulletPrefab!");
        }
        
        // Phát âm thanh
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSound(1); // Âm thanh bắn
        }
    }
    
    // Tìm enemy gần nhất
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log($"Tìm thấy {enemies.Length} enemies với tag 'Enemy'");
        
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                Debug.Log($"Enemy tại {enemy.transform.position}, khoảng cách: {distance}");
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        
        if (nearestEnemy != null)
        {
            Debug.Log($"Enemy gần nhất: {nearestEnemy.name} tại {nearestEnemy.transform.position}");
        }
        else
        {
            Debug.Log("Không tìm thấy enemy nào!");
        }
        
        return nearestEnemy;
    }
}
