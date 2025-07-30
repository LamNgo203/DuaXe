using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UIBase
{
    public Button pauseButton;

    public Text scoreText;
    
    protected override void Setup()
    {
        base.Setup();
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnClickPause);
        }
    }
    public override void Show(System.Action onHideDone)
    {
        base.Show(onHideDone);
        UIManager.Instance.GetUiActive<UIGameplay>(UIName.UIGameplay).pauseButton.gameObject.SetActive(true);
    }
    public void OnClickPause()
    {
        // Pause the game logic here
        Time.timeScale = 0f; // Stop the game time
        UIManager.Instance.ShowUI(UIName.UIPause);
        SoundManager.Instance.PlayGameSound(0);
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
