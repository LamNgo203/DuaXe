using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool showDebugInfo = true;
    public KeyCode debugKey = KeyCode.D;
    
    private void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            DebugSceneInfo();
        }
    }
    
    private void DebugSceneInfo()
    {
        Debug.Log("=== DEBUG SCENE INFO ===");
        
        // Kiểm tra Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log($"Player: {player.name} tại {player.transform.position}");
            SmartBikeController bikeController = player.GetComponent<SmartBikeController>();
            if (bikeController != null)
            {
                
            }
            else
            {
                Debug.LogError("Player không có SmartBikeController!");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy GameObject với tag 'Player'!");
        }
        
        // Kiểm tra Enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log($"Tìm thấy {enemies.Length} enemies:");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"- {enemy.name} tại {enemy.transform.position} (Layer: {enemy.layer})");
            SimpleEnemy simpleEnemy = enemy.GetComponent<SimpleEnemy>();
            if (simpleEnemy != null)
            {
                Debug.Log($"  + Có SimpleEnemy, máu: {simpleEnemy.currentHealth}/{simpleEnemy.maxHealth}");
            }
            else
            {
                Debug.LogWarning($"  + Không có SimpleEnemy component!");
            }
        }
        
        // Kiểm tra Bullets
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        Debug.Log($"Tìm thấy {bullets.Length} bullets đang bay");
        
        // Kiểm tra AmmoItems
        AmmoItem[] ammoItems = FindObjectsOfType<AmmoItem>();
        Debug.Log($"Tìm thấy {ammoItems.Length} ammo items");
        
        Debug.Log("=== END DEBUG INFO ===");
    }
    
    private void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new Rect(Screen.width - 300, 10, 290, 200));
        GUILayout.Label("=== DEBUG HELPER ===");
        GUILayout.Label($"D: Debug Scene Info");
        GUILayout.Label($"");
        GUILayout.Label($"Player: {(GameObject.FindGameObjectWithTag("Player") != null ? "✓" : "✗")}");
        GUILayout.Label($"Enemies: {GameObject.FindGameObjectsWithTag("Enemy").Length}");
        GUILayout.Label($"Bullets: {FindObjectsOfType<Bullet>().Length}");
        GUILayout.Label($"Ammo Items: {FindObjectsOfType<AmmoItem>().Length}");
        GUILayout.EndArea();
    }
} 