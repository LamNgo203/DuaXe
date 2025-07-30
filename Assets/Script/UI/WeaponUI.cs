using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text ammoText;
    public Text ammoCountText;
    public Image ammoIcon;
    public Slider ammoSlider;
    
    [Header("Visual Effects")]
    public Color normalColor = Color.white;
    public Color lowAmmoColor = Color.red;
    public Color noAmmoColor = Color.gray;
    
    [Header("Animation")]
    public float pulseSpeed = 2f;
    public float pulseScale = 1.2f;
    
    private WeaponSystem weaponSystem;
    private int lastAmmoCount = 0;
    private bool isPulsing = false;
    private Vector3 originalScale;
    
    private void Start()
    {
        originalScale = transform.localScale;
        
        // Tìm WeaponSystem trên player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            weaponSystem = player.GetComponent<WeaponSystem>();
        }
        
        // Nếu không tìm thấy, tìm trong scene
        if (weaponSystem == null)
        {
            weaponSystem = FindObjectOfType<WeaponSystem>();
        }
        
        UpdateUI();
    }
    
    private void Update()
    {
        if (weaponSystem != null)
        {
            // Cập nhật UI khi số đạn thay đổi
            if (weaponSystem.currentAmmo != lastAmmoCount)
            {
                lastAmmoCount = weaponSystem.currentAmmo;
                UpdateUI();
                
                // Hiệu ứng pulse khi nhận đạn
                if (weaponSystem.currentAmmo > lastAmmoCount)
                {
                    StartPulse();
                }
            }
        }
        
        // Xử lý hiệu ứng pulse
        if (isPulsing)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
            transform.localScale = originalScale * (1f + pulse * (pulseScale - 1f));
            
            if (Time.time > 1f) // Dừng pulse sau 1 giây
            {
                StopPulse();
            }
        }
    }
    
    private void UpdateUI()
    {
        if (weaponSystem == null) return;
        
        int currentAmmo = weaponSystem.currentAmmo;
        int maxAmmo = weaponSystem.maxAmmo;
        
        // Cập nhật text
        if (ammoText != null)
        {
            ammoText.text = "AMMO";
        }
        
        if (ammoCountText != null)
        {
            ammoCountText.text = currentAmmo + "/" + maxAmmo;
        }
        
        // Cập nhật slider
        if (ammoSlider != null)
        {
            ammoSlider.value = (float)currentAmmo / maxAmmo;
        }
        
        // Cập nhật màu sắc
        Color targetColor = normalColor;
        if (currentAmmo == 0)
        {
            targetColor = noAmmoColor;
        }
        else if (currentAmmo <= maxAmmo * 0.3f) // Dưới 30% đạn
        {
            targetColor = lowAmmoColor;
        }
        
        // Áp dụng màu cho icon
        if (ammoIcon != null)
        {
            ammoIcon.color = targetColor;
        }
        
        // Áp dụng màu cho text
        if (ammoCountText != null)
        {
            ammoCountText.color = targetColor;
        }
    }
    
    private void StartPulse()
    {
        isPulsing = true;
    }
    
    private void StopPulse()
    {
        isPulsing = false;
        transform.localScale = originalScale;
    }
    
    // Hàm để test UI
    public void TestAddAmmo()
    {
        if (weaponSystem != null)
        {
            weaponSystem.AddAmmo(5);
        }
    }
    
    public void TestRemoveAmmo()
    {
        if (weaponSystem != null)
        {
            // Giảm đạn bằng cách set currentAmmo
            weaponSystem.currentAmmo = Mathf.Max(0, weaponSystem.currentAmmo - 1);
        }
    }
} 