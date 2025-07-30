using UnityEngine;
using master;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // Nhạc nền
    public AudioSource sfxSource; // Hiệu ứng (button, game sounds)
    public AudioSource engineSource; // Tiếng xe

    [Header("Audio Clips")]
    public AudioClip buttonClip;
    public AudioClip backgroundMusicClip;
    public AudioClip[] gameSfxClips; // Các hiệu ứng game
    public AudioClip engineSfx;

    private bool isMuted = false;
    
    // Keys cho PlayerPrefs
    private const string SOUND_ENABLED_KEY = "SoundEnabled";
    private const string MUSIC_ENABLED_KEY = "MusicEnabled";

    protected override void Awake()
    {
        base.Awake();
        
        // Khôi phục trạng thái từ PlayerPrefs
        RestoreAudioSettings();
        
        if (bgmSource != null && backgroundMusicClip != null)
        {
            bgmSource.clip = backgroundMusicClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private void RestoreAudioSettings()
    {
        // Khôi phục trạng thái âm thanh
        bool soundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1;
        bool musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
        
        if (sfxSource != null)
            sfxSource.mute = !soundEnabled;
            
        if (engineSource != null)
            engineSource.mute = !soundEnabled;
            
        if (bgmSource != null)
            bgmSource.mute = !musicEnabled;
    }

    public void PlayButtonSound()
    {
        if (!isMuted && IsSoundEnabled() && buttonClip != null && sfxSource != null)
            sfxSource.PlayOneShot(buttonClip);
    }

    public void PlayBackgroundMusic()
    {
        if (!isMuted && IsMusicEnabled() && bgmSource != null && backgroundMusicClip != null)
        {
            bgmSource.clip = backgroundMusicClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayGameSound(int index)
    {
        if (!isMuted && IsSoundEnabled() && gameSfxClips != null && index >= 0 && index < gameSfxClips.Length && sfxSource != null)
        {
            sfxSource.PlayOneShot(gameSfxClips[index]);
        }
    }
    
    public void PlayEngineSound(int index)
    {
        if (!isMuted && IsSoundEnabled() && gameSfxClips != null && index >= 0 && index < gameSfxClips.Length && engineSource != null)
        {
            // Dừng engine sound hiện tại
            engineSource.Stop();
            
            // Phát engine sound mới
            engineSource.clip = gameSfxClips[index];
            engineSource.loop = true;
            engineSource.Play();
        }
    }
    
    public void StopEngineSound()
    {
        if (engineSource != null)
        {
            engineSource.Stop();
        }
    }

    public void MuteAll(bool mute)
    {
        isMuted = mute;
        if (bgmSource != null) bgmSource.mute = mute;
        if (sfxSource != null) sfxSource.mute = mute;
        if (engineSource != null) engineSource.mute = mute;
    }

    public bool IsMuted() => isMuted;
    
    public bool IsSoundEnabled()
    {
        return PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1;
    }
    
    public void SetSoundEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(SOUND_ENABLED_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
        
        if (sfxSource != null)
            sfxSource.mute = !enabled;
            
        if (engineSource != null)
            engineSource.mute = !enabled;
    }
    
    public bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
    }
    
    public void SetMusicEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
        
        if (bgmSource != null)
            bgmSource.mute = !enabled;
    }
}
