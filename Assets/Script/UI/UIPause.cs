using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
    public Button replayButton;
    public Button closeButton;

    [Header("Sound Settings")]
    public Button soundButton;
    public Image soundIcon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Text soundStatusText;

    [Header("Vibration Settings")]
    public Button vibrationButton;
    public Image vibrationIcon;
    public Sprite vibrationOnSprite;
    public Sprite vibrationOffSprite;
    public Text vibrationStatusText;

    [Header("Music Settings")]
    public Button musicButton;
    public Image musicIcon;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Text musicStatusText;

    private bool isVibrationOn = true;

    protected override void Setup()
    {
        base.Setup();

        if (replayButton != null)
            replayButton.onClick.AddListener(OnClickReplay);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnClickClose);

        if (soundButton != null)
            soundButton.onClick.AddListener(OnClickSound);

        if (vibrationButton != null)
            vibrationButton.onClick.AddListener(OnClickVibration);

        if (musicButton != null)
            musicButton.onClick.AddListener(OnClickMusic);

        UpdateAllStatusText();
    }

    public override void Show(System.Action onHideDone)
    {
        base.Show(onHideDone);
        Time.timeScale = 0f;
    }

    public override void Hide(bool hasShowOutAnim = false)
    {
        Time.timeScale = 1f;
        base.Hide(hasShowOutAnim);
    }

    public void OnClickReplay()
    {
        Hide();
        GameManager.Instance.ResetGame();
        GameManager.Instance.StartGame();
    }

    public void OnClickClose()
    {
        SoundManager.Instance.PlayGameSound(0);
        Hide();
    }

    public void OnClickSound()
    {
        if (SoundManager.Instance != null)
        {
            bool currentSoundState = SoundManager.Instance.IsSoundEnabled();
            bool newSoundState = !currentSoundState;
            SoundManager.Instance.SetSoundEnabled(newSoundState);
            UpdateSoundStatusText();
        }
    }

    public void OnClickVibration()
    {
        isVibrationOn = !isVibrationOn;
        UpdateVibrationStatusText();
        // Nếu có quản lý rung, xử lý ở đây
        // Ví dụ: VibrationManager.Instance.SetVibration(isVibrationOn);
    }

    public void OnClickMusic()
    {
        if (SoundManager.Instance != null)
        {
            bool currentMusicState = SoundManager.Instance.IsMusicEnabled();
            bool newMusicState = !currentMusicState;
            SoundManager.Instance.SetMusicEnabled(newMusicState);
            UpdateMusicStatusText();
        }
    }

    private void UpdateAllStatusText()
    {
        UpdateSoundStatusText();
        UpdateVibrationStatusText();
        UpdateMusicStatusText();
    }

    private void UpdateSoundStatusText()
    {
        if (soundStatusText != null)
        {
            bool isSoundOn = SoundManager.Instance != null ? SoundManager.Instance.IsSoundEnabled() : true;
            soundStatusText.text = isSoundOn ? "ON" : "OFF";
        }
        
        if (soundIcon != null)
        {
            bool isSoundOn = SoundManager.Instance != null ? SoundManager.Instance.IsSoundEnabled() : true;
            soundIcon.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
    }

    private void UpdateVibrationStatusText()
    {
        if (vibrationStatusText != null)
        {
            vibrationStatusText.text = isVibrationOn ? "ON" : "OFF";
        }
        
        if (vibrationIcon != null)
        {
            vibrationIcon.sprite = isVibrationOn ? vibrationOnSprite : vibrationOffSprite;
        }
    }

    private void UpdateMusicStatusText()
    {
        if (musicStatusText != null)
        {
            bool isMusicOn = SoundManager.Instance != null ? SoundManager.Instance.IsMusicEnabled() : true;
            musicStatusText.text = isMusicOn ? "ON" : "OFF";
        }
        
        if (musicIcon != null)
        {
            bool isMusicOn = SoundManager.Instance != null ? SoundManager.Instance.IsMusicEnabled() : true;
            musicIcon.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }
}