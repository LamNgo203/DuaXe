using UnityEngine;

public class MotorAudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip engineSound;
    public float volume = 0.5f;
    public bool loop = true;
    
    [Header("Components")]
    public ParticleSystem smokeTrailPrefab;
    
    private AudioSource audioSource;
    private bool isPlaying = false;
    
    void Start()
    {
        SetupAudioSource();
    }
    
    void SetupAudioSource()
    {
        // Tạo AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Cấu hình AudioSource
        if (engineSound != null)
        {
            audioSource.clip = engineSound;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.playOnAwake = false;
        }
    }
    
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
    
    public void StartEngine()
    {
        // Phát hiệu ứng khói
        if (smokeTrailPrefab != null)
        {
            smokeTrailPrefab.Play();
        }
        
        // Phát âm thanh xe
        if (!isPlaying && audioSource != null && engineSound != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }
    
    public void StopEngine()
    {
        // Dừng hiệu ứng khói
        if (smokeTrailPrefab != null)
        {
            smokeTrailPrefab.Stop();
        }
        
        // Dừng âm thanh xe
        if (isPlaying && audioSource != null)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
    
    // Hàm để gán âm thanh từ bên ngoài
    public void SetEngineSound(AudioClip clip)
    {
        engineSound = clip;
        if (audioSource != null)
        {
            audioSource.clip = clip;
        }
    }
} 