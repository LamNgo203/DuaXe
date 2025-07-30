using UnityEngine;
using System.Collections.Generic;

public class EndlessMapSpawner : MonoBehaviour
{
    [Header("Map Spawn Settings")]
    public GameObject[] mapPrefabs; // Các prefab map khác nhau
    public float mapSpacing = 100f; // Khoảng cách giữa các map prefab
    public float mapHeight = 0f; // Độ cao của map (Y = 0)
    public float triggerDistance = 90f; // Khoảng cách trigger spawn
    public float cleanupDistance = 200f; // Khoảng cách để xóa map cũ
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    public KeyCode debugKey = KeyCode.M;
    
    private Transform player;
    private float lastPlayerX = 0f;
    private float nextSpawnX = 0f; // Vị trí X tiếp theo để spawn
    private List<GameObject> spawnedMaps = new List<GameObject>();
    private bool isInitialized = false;
    
    private void Start()
    {
        // Tìm player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player! Đảm bảo xe có tag 'Player'");
            return;
        }
        
        // Khởi tạo vị trí spawn đầu tiên
        nextSpawnX = 0f; // Bắt đầu từ 0 để spawn map đầu tiên tại (0,0,0)
        
        lastPlayerX = player.position.x;
        isInitialized = true;
        
        // Đăng ký với GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnGameReset += HandleGameReset;
            GameManager.Instance.OnGameOver += HandleGameReset;
        }
        
        // Spawn map đầu tiên tại (0,0,0)
        SpawnNewMap();
        
        Debug.Log("EndlessMapSpawner đã được khởi tạo!");
    }
    
    private void Update()
    {
        if (!isInitialized || player == null) return;
        
        float currentPlayerX = player.position.x;
        
        // Kiểm tra nếu player đã đi được đủ khoảng cách để spawn map mới
        if (currentPlayerX >= nextSpawnX - triggerDistance)
        {
            SpawnNewMap();
        }
        
        // Cleanup maps cũ
        CleanupOldMaps(currentPlayerX);
        
        lastPlayerX = currentPlayerX;
        
        // Debug input
        if (Input.GetKeyDown(debugKey))
        {
            DebugMapInfo();
        }
    }
    
    private void SpawnNewMap()
    {
        if (mapPrefabs == null || mapPrefabs.Length == 0)
        {
            Debug.LogWarning("Chưa gán mapPrefabs!");
            return;
        }
        
        // Chọn map prefab ngẫu nhiên
        GameObject selectedPrefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
        
        // Tính vị trí spawn
        Vector3 spawnPosition = new Vector3(nextSpawnX, mapHeight, 0);
        
        // Spawn map
        GameObject newMap = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        spawnedMaps.Add(newMap);
        
        Debug.Log($"Đã spawn map mới tại {spawnPosition}");
        
        // Cập nhật vị trí spawn tiếp theo
        nextSpawnX += mapSpacing;
    }
    
    private void CleanupOldMaps(float currentPlayerX)
    {
        for (int i = spawnedMaps.Count - 1; i >= 0; i--)
        {
            if (spawnedMaps[i] != null)
            {
                float distance = currentPlayerX - spawnedMaps[i].transform.position.x;
                if (distance > cleanupDistance)
                {
                    Debug.Log($"Xóa map cũ tại {spawnedMaps[i].transform.position}");
                    Destroy(spawnedMaps[i]);
                    spawnedMaps.RemoveAt(i);
                }
            }
            else
            {
                spawnedMaps.RemoveAt(i);
            }
        }
    }
    
    private void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new Rect(10, Screen.height - 150, 300, 140));
        GUILayout.Label("=== ENDLESS MAP SPAWNER ===");
        GUILayout.Label($"Player X: {(player != null ? player.position.x.ToString("F1") : "N/A")}");
        GUILayout.Label($"Next Spawn X: {nextSpawnX:F1}");
        GUILayout.Label($"Map Spacing: {mapSpacing}");
        GUILayout.Label($"Trigger Distance: {triggerDistance}");
        GUILayout.Label($"Maps Active: {spawnedMaps.Count}");
        GUILayout.Label($"M: Debug Info");
        GUILayout.EndArea();
    }
    
    private void DebugMapInfo()
    {
        Debug.Log("=== ENDLESS MAP DEBUG ===");
        Debug.Log($"Player position: {player.position}");
        Debug.Log($"Next spawn X: {nextSpawnX}");
        Debug.Log($"Distance traveled: {player.position.x - lastPlayerX}");
        Debug.Log($"Active maps: {spawnedMaps.Count}");
        
        for (int i = 0; i < spawnedMaps.Count; i++)
        {
            if (spawnedMaps[i] != null)
            {
                Debug.Log($"Map {i}: {spawnedMaps[i].name} tại {spawnedMaps[i].transform.position}");
            }
        }
    }
    
    // Hàm để reset spawner (khi restart game)
    public void ResetSpawner()
    {
        lastPlayerX = 0f;
        nextSpawnX = 0f; // Reset về 0 để spawn map đầu tiên tại (0,0,0)
        
        // Xóa tất cả maps hiện tại
        foreach (GameObject map in spawnedMaps)
        {
            if (map != null)
            {
                Destroy(map);
            }
        }
        spawnedMaps.Clear();
        
        // Spawn map đầu tiên sau khi reset
        SpawnNewMap();
        
        Debug.Log("EndlessMapSpawner đã được reset!");
    }
    
    private void HandleGameStart()
    {
        // Bắt đầu spawn maps
        isInitialized = true;
        Debug.Log("EndlessMapSpawner bắt đầu hoạt động!");
    }
    
    private void HandleGameReset()
    {
        // Reset khi game restart
        ResetSpawner();
        if (player != null)
        {
            lastPlayerX = player.position.x;
        }
    }
    
    // Hàm để spawn map thủ công
    [ContextMenu("Spawn Map Manually")]
    public void SpawnMapManually()
    {
        if (player != null)
        {
            SpawnNewMap();
        }
    }
} 