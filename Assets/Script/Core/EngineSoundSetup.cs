using UnityEngine;

public class EngineSoundSetup : MonoBehaviour
{
    [Header("Engine Sound")]
    public AudioClip engineSound;
    public int soundIndex = 5; // Index trong SoundManager
    
    void Start()
    {
        AddEngineSoundToManager();
    }
    
    void AddEngineSoundToManager()
    {
        if (engineSound == null)
        {
            Debug.LogWarning("Chưa gán engineSound!");
            return;
        }
        
        // Tìm SoundManager trong scene
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            // Thêm âm thanh xe vào SoundManager
            // Lưu ý: Cần cấu hình thủ công trong Inspector của SoundManager
            Debug.Log($"Hãy thêm engineSound vào SoundManager.gameSfxClips tại index {soundIndex}");
            Debug.Log($"File âm thanh: {engineSound.name}");
        }
        else
        {
            Debug.LogError("Không tìm thấy SoundManager trong scene!");
        }
    }
} 