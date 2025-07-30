using UnityEngine;

public class SimpleSoundSetup : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioClip engineSound;
    public int engineSoundIndex = 5; // Index trong SoundManager
    
    void Start()
    {
        SetupEngineSound();
    }
    
    void SetupEngineSound()
    {
        if (engineSound == null)
        {
            Debug.LogWarning("Chưa gán engineSound!");
            return;
        }
        
        // Tìm SoundManager và thêm âm thanh xe vào gameSfxClips
        if (SoundManager.Instance != null)
        {
            // Sử dụng reflection để truy cập gameSfxClips
            var gameSfxClipsField = typeof(SoundManager).GetField("gameSfxClips", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (gameSfxClipsField != null)
            {
                AudioClip[] gameSfxClips = (AudioClip[])gameSfxClipsField.GetValue(SoundManager.Instance);
                
                // Mở rộng mảng nếu cần
                if (gameSfxClips == null || engineSoundIndex >= gameSfxClips.Length)
                {
                    AudioClip[] newGameSfxClips = new AudioClip[engineSoundIndex + 1];
                    if (gameSfxClips != null)
                    {
                        gameSfxClips.CopyTo(newGameSfxClips, 0);
                    }
                    gameSfxClips = newGameSfxClips;
                }
                
                // Gán âm thanh xe
                gameSfxClips[engineSoundIndex] = engineSound;
                gameSfxClipsField.SetValue(SoundManager.Instance, gameSfxClips);
                
                Debug.Log($"Đã thêm âm thanh xe vào SoundManager tại index {engineSoundIndex}");
            }
        }
        
        // Tìm tất cả SimpleMotorSound và cập nhật engineSoundIndex
        SimpleMotorSound[] motorSounds = FindObjectsOfType<SimpleMotorSound>();
        foreach (SimpleMotorSound motorSound in motorSounds)
        {
            motorSound.engineSoundIndex = engineSoundIndex;
        }
        
        // Tìm tất cả SmartBikeController và cập nhật engineSoundIndex
        SmartBikeController[] bikeControllers = FindObjectsOfType<SmartBikeController>();
        foreach (SmartBikeController controller in bikeControllers)
        {
            controller.engineSoundIndex = engineSoundIndex;
        }
    }
} 