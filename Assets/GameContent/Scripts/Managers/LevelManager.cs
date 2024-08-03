using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : SingletonMB<LevelManager>
{
    public GameObject cardContainer;
    public GameObject HorizotalLayoutPrefab;
    public GameObject cardHolderPrefab;
    private int numberOfCardHolders;

    public delegate void OnLevelStateChangedDelegate(LevelState levelState);
    public OnLevelStateChangedDelegate OnLevelStateChangedEvent;


    public LevelState CurrentLevelState { get; private set; }

    private Level_SO levelSO;

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
        SpawnAndAdjustCellSize();
    }

    // We call this method to change the state of the level and trigger the event for any script subscribed to it
    public void ChangeState(LevelState state)
    {
        if (CurrentLevelState == state) return;

        CurrentLevelState = state;
        OnLevelStateChangedEvent?.Invoke(CurrentLevelState);
    }

    private void SpawnAndAdjustCellSize()
    {
        // Get the size of the container
        RectTransform containerRect = cardContainer.GetComponent<RectTransform>();
        float containerWidth = containerRect.rect.width;
        float containerHeight = containerRect.rect.height;

        // Calculate the size for each element
        float cellWidth = containerWidth / levelSO.W;
        float cellHeight = containerHeight / levelSO.H;

        // Adjust the size of each card holder
        for (int i = 0; i < levelSO.H; i++)
        {

            GameObject horizontalLayoutTemp = Instantiate(HorizotalLayoutPrefab, cardContainer.transform);
            RectTransform horizontalLayoutRect = horizontalLayoutTemp.GetComponent<RectTransform>();

            // Set the size of the horizontal layout
            horizontalLayoutRect.sizeDelta = new Vector2(containerWidth, cellHeight);

            for (int j = 0; j < levelSO.W; j++)
            {
                GameObject cardHolderTemp = Instantiate(cardHolderPrefab, horizontalLayoutTemp.transform);

                RectTransform cardHolderRect = cardHolderTemp.GetComponent<RectTransform>();
                cardHolderRect.sizeDelta = new Vector2(cellWidth, cellHeight);
            }
        }
    }


}
