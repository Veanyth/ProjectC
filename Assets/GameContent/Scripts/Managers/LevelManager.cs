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
    

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;

        // Check if the level is a custom level or not
        if (_gameManager.CustomLevelOn)
            levelSO = _gameManager.CustomLevel;
        else
            levelSO = _gameManager.Levels[_gameManager.CurrentLevelIndex];
    }

    // We call this method to change the state of the level and trigger the event for any script subscribed to it
    public void ChangeState(LevelState levelState)
    {
        Debug.Log("Try to change state " + levelState);
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
                break;
        }
        Debug.Log("State Changed " + levelState);

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

}
