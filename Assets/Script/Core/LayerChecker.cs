using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    [Header("Layer Settings")]
    public LayerMask enemyLayerMask = 256; // Layer 8 (2^8 = 256)
    public KeyCode checkKey = KeyCode.L;
    
    private void Update()
    {
        if (Input.GetKeyDown(checkKey))
        {
            CheckLayers();
        }
    }
    
    private void CheckLayers()
    {
        Debug.Log("=== LAYER CHECK ===");
        
        // Kiểm tra Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log($"Player layer: {player.layer}");
            SmartBikeController bikeController = player.GetComponent<SmartBikeController>();
            if (bikeController != null)
            {
                
            }
        }
        
        // Kiểm tra Enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log($"Tìm thấy {enemies.Length} enemies:");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"- {enemy.name}: Layer {enemy.layer}");
            
            // Kiểm tra xem layer có trong enemyLayerMask không
            bool isInMask = ((1 << enemy.layer) & enemyLayerMask) != 0;
            Debug.Log($"  + Trong enemyLayerMask: {isInMask}");
        }
        
        // Kiểm tra Bullets
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        Debug.Log($"Tìm thấy {bullets.Length} bullets:");
        foreach (Bullet bullet in bullets)
        {
            Debug.Log($"- Bullet targetLayer: {bullet.targetLayer.value}");
        }
        
        Debug.Log("=== END LAYER CHECK ===");
    }
    
    [ContextMenu("Fix Layer Settings")]
    public void FixLayerSettings()
    {
        Debug.Log("=== FIXING LAYER SETTINGS ===");
        
        // Sửa enemyLayer trong SmartBikeController
        SmartBikeController bikeController = FindObjectOfType<SmartBikeController>();
        if (bikeController != null)
        {
            
        }
        
        // Sửa targetLayer trong tất cả bullets
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            bullet.targetLayer = enemyLayerMask;
        }
        Debug.Log($"Đã sửa targetLayer cho {bullets.Length} bullets");
        
        // Đảm bảo tất cả enemies có đúng layer
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = 8; // Layer 8 cho enemy
        }
        Debug.Log($"Đã set layer 8 cho {enemies.Length} enemies");
        
        Debug.Log("=== LAYER SETTINGS FIXED ===");
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 100, 300, 90));
        GUILayout.Label("=== LAYER CHECKER ===");
        GUILayout.Label($"L: Check Layers");
        GUILayout.Label($"Right-click → Fix Layer Settings");
        GUILayout.Label($"Enemy Layer Mask: {enemyLayerMask.value}");
        GUILayout.EndArea();
    }
} 