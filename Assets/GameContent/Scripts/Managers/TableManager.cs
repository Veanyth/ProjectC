using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private CardsManager cardsManager;
    public GameObject cardContainer;
    public GameObject HorizotalLayoutPrefab;
    public GameObject cardHolderPrefab;

    private Level_SO levelSO;

    private List<Transform> cardHoldersDict = new List<Transform>();
    public List<Transform> CardHolderDict { get { return cardHoldersDict; } }

    private LevelManager _levelManager;
    private void Awake()
    {
        _levelManager = LevelManager.Instance;
        levelSO = _levelManager.Level_SO;

        SpawnAndAdjustCellSize();
        cardsManager.GenerateCards(this);
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

                cardHoldersDict.Add(cardHolderTemp.transform);
            }
        }
    }
}
