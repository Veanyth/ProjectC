using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMB<LevelManager>
{
    public delegate void OnLevelStateChangedDelegate(LevelState levelState);
    public OnLevelStateChangedDelegate OnLevelStateChangedEvent;


    public LevelState CurrentLevelState { get; private set; }

    private Level_SO levelSO;

    private GameManager _gameManager;
    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;
        if (_gameManager.CustomLevelOn)
            levelSO = _gameManager.CustomLevel;
        else
            levelSO = _gameManager.Levels[_gameManager.CurrentLevelIndex];
        Debug.Log("Level W" + levelSO.W);
        Debug.Log("Level H" + levelSO.H);
    }

    public void ChangeState(LevelState state)
    {
        if (CurrentLevelState == state) return;

        CurrentLevelState = state;
        OnLevelStateChangedEvent?.Invoke(CurrentLevelState);
    }
}
