using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    public Button startButton;
    public Button shopButton;
    public Button soundButton;
    public Text highScoreText;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public override void Show(System.Action onHideDone)
    {
        base.Show(onHideDone);
        UpdateSoundButtonSprite();
        highScoreText.text = GameManager.Instance.GetHighScore().ToString();
        // startButton.onClick.AddListener(OnStartButtonClick);
        // soundButton.onClick.AddListener(OnSoundButtonClick);
        // shopButton.onClick.AddListener(OnShopButtonClick);
    }
    protected override void Setup()
    {
        base.Setup();
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
        if (soundButton != null)
        {
            soundButton.onClick.AddListener(OnSoundButtonClick);
        }
        if (shopButton != null)
        {
            shopButton.onClick.AddListener(OnShopButtonClick);
        }
    }
    public void OnStartButtonClick()
    {
        GameManager.Instance.StartGame();
        Debug.Log("OnStartButtonClick");
        SoundManager.Instance.PlayGameSound(0);
        // UIManager.Instance.ShowUI(UIName.UIGameplay);
    }
    public void OnSoundButtonClick()
    {
        SoundManager.Instance.PlayGameSound(0);
        bool currentSoundState = SoundManager.Instance.IsSoundEnabled();
        bool newSoundState = !currentSoundState;
        SoundManager.Instance.SetSoundEnabled(newSoundState);
        UpdateSoundButtonSprite();
    }
    public void OnShopButtonClick()
    {
        SoundManager.Instance.PlayGameSound(0);
        // UIManager.Instance.ShowUI(UIName.Shop);
    }

    private void UpdateSoundButtonSprite()
    {
        if (soundButton != null && soundButton.image != null)
        {
            soundButton.image.sprite = SoundManager.Instance.IsSoundEnabled() ? soundOnSprite : soundOffSprite;
        }
    }
}
