using UnityEngine;

public class SimpleMotorSound : MonoBehaviour
{
    [Header("Audio")]
    public int engineSoundIndex = 5; // Index của âm thanh xe trong SoundManager
    
    [Header("Components")]
    public ParticleSystem smokeTrailPrefab;
    
    private bool isPlaying = false;
    
    void Update()
    {
        // Khi giữ chuột trái
        if (Input.GetMouseButton(0))
        {
            StartEngine();
        }
        // Khi thả chuột
        else if (Input.GetMouseButtonUp(0))
        {
            StopEngine();
        }
    }
    
    void StartEngine()
    {
        // Phát hiệu ứng khói
        if (smokeTrailPrefab != null)
        {
            smokeTrailPrefab.Play();
        }
        
        // Phát âm thanh xe qua SoundManager
        if (!isPlaying && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSound(engineSoundIndex);
            isPlaying = true;
        }
    }
    
    void StopEngine()
    {
        // Dừng hiệu ứng khói
        if (smokeTrailPrefab != null)
        {
            smokeTrailPrefab.Stop();
        }
        
        // Dừng âm thanh xe
        if (isPlaying)
        {
            isPlaying = false;
            // SoundManager sẽ tự động dừng âm thanh khi không còn gọi PlayGameSound
        }
    }
} 