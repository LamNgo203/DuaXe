using UnityEngine;

public class SimpleMotorAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip engineSound;
    public AudioSource audioSource;
    
    private bool isPlaying = false;
    
    void Start()
    {
        // Tạo AudioSource nếu chưa có
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Cấu hình AudioSource
        if (engineSound != null)
        {
            audioSource.clip = engineSound;
            audioSource.loop = true;
            audioSource.volume = 0.3f;
            audioSource.playOnAwake = false;
        }
    }
    
    void Update()
    {
        // Khi giữ chuột trái
        if (Input.GetMouseButton(0))
        {
            if (!isPlaying && audioSource != null && engineSound != null)
            {
                audioSource.Play();
                isPlaying = true;
            }
        }
        // Khi thả chuột
        else if (Input.GetMouseButtonUp(0))
        {
            if (isPlaying && audioSource != null)
            {
                audioSource.Stop();
                isPlaying = false;
            }
        }
    }
} 