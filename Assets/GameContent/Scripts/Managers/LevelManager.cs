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

        // Spawn and Adjust the cell size of card Holders and horizontal layouts
       
    }

    // We call this method to change the state of the level and trigger the event for any script subscribed to it
    public void ChangeState(LevelState state)
    {
        if (CurrentLevelState == state) return;

        CurrentLevelState = state;
        OnLevelStateChangedEvent?.Invoke(CurrentLevelState);
    }

   


}
