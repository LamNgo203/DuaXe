using System;
using System.Collections;
using System.Collections.Generic;
using master;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameReset;
    public bool isGameStarted = false;


    //high score mana
    
    public int highScore = 0;
    public int currentScore = 0;
    
    public void AddScore(int score)
    {
        currentScore += score;
        UIManager.Instance.GetUI(UIName.UIGameplay).GetComponent<UIGameplay>().UpdateScore(currentScore);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void Start()
    {
        InitializeGame();
    }
    public void InitializeGame()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        currentScore = 0;
        UIManager.Instance.ShowUI(UIName.MainMenu);
        Debug.Log("Game Initialized");
    }
    
    public void StartGame()
    {
        currentScore = 0;
        if (isGameStarted) return;
        UIManager.Instance.HideUiActive(UIName.MainMenu);
        UIManager.Instance.ShowUI(UIName.UIGameplay);
        isGameStarted = true;
        OnGameStart?.Invoke();
        Debug.Log("Game Started");
    }
    public void GameOver()
    {
        if (!isGameStarted) return;
        isGameStarted = false;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {

        yield return new WaitForSeconds(2f);
        ResetGame();
        UIManager.Instance.ShowUI(UIName.MainMenu);
        UIManager.Instance.HideUiActive(UIName.UIGameplay);
    }

    public void ResetGame()
    {
        currentScore = 0;
        isGameStarted = false;
        OnGameReset?.Invoke();
        
    }
}
