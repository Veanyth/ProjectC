using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : SingletonMB<LevelManager>
{
    public delegate void OnLevelStateChangedDelegate(LevelState levelState);
    public OnLevelStateChangedDelegate OnLevelStateChangedEvent;

    public LevelState CurrentLevelState { get; private set; }

    private Level_SO levelSO;
    public Level_SO Level_SO { get { return levelSO; } }

    private GameManager _gameManager;

    private int cardMatched = 0;
    private int cardsInLevel;
    
    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;

        // Check if the level is a custom level or not
        if (_gameManager.CustomLevelOn)
            levelSO = _gameManager.CustomLevel;
        else
            levelSO = _gameManager.Levels[_gameManager.CurrentLevelIndex];

        cardsInLevel = levelSO.H * levelSO.W - ((levelSO.H * levelSO.W) % 2); // exp: grid of (H=5,W=5) = 25, means there is 25 - (25%2) = 24 cards in the game
    }

    // We call this method to change the state of the level and trigger the event for any script subscribed to it
    public void ChangeState(LevelState levelState)
    {
        if (CurrentLevelState == levelState) return;

        CurrentLevelState = levelState;
        OnLevelStateChangedEvent?.Invoke(CurrentLevelState);

        switch (levelState)
        {
            case LevelState.Memorize:
                MemorizePhase();
                break;
            case LevelState.Start:
                break;
            case LevelState.Complete:
                LevelEndedSaveProgress();
                break;
        }
    }

    private void MemorizePhase()
    {
        StartCoroutine(MemorizeCountdownCoroutine(levelSO.MemorizeTimer));
    }

    private IEnumerator MemorizeCountdownCoroutine(float timer)
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
        }

        ChangeState(LevelState.Starting); // Start flipping the cards
    }

    public void CardMatched()
    {
        cardMatched++;

        if (cardMatched >= cardsInLevel / 2)
        {
            Debug.Log("Level Complete"); // Level Complete
            ChangeState(LevelState.Completing);
        }
    }

    private void LevelEndedSaveProgress()
    {

        SaveLoadManager.Instance.SaveStarsProgress(GameManager.Instance.CurrentLevelIndex, ScoreManager.Instance.StarsGained);
        SaveLoadManager.Instance.SaveLevelReached(GameManager.Instance.CurrentLevelIndex+1);
    }
}
