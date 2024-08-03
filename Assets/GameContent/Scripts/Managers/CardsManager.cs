using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private TableManager _tableManager;

    private Dictionary<int, Card> CardsDict = new Dictionary<int, Card>();

    private Card lastClickedCard = null;

    private void Awake()
    {
        LevelManager.Instance.OnLevelStateChangedEvent += LevelStateChanged;
    }

    public void GenerateCards(TableManager tableManager)
    {
        _tableManager = tableManager;

        // Get the Number of card to spawn, if the grid size is an odd number we need to make it even for the cards to spawn,
        // otherwise we will end up with a card that doesn't have a match
        int numberOfCardToSpawn = _tableManager.CardHolderDict.Count - _tableManager.CardHolderDict.Count % 2;

        int cardSpawnedFromDB = 0;
        int spawnPairOfSameCard = 0;

        for (int i = 0; i < numberOfCardToSpawn; i++)
        {
            if (spawnPairOfSameCard == 2) // Check if we spawned 2 cards of the same type in the grid, if so, we need to
                                          // reset the pair counter and increament the index to the next card from the DB
            {
                spawnPairOfSameCard = 0;
                cardSpawnedFromDB++;
            }

            Transform randomCardHolderSpot = PickARandomSpotFromTheGrid();
            if (randomCardHolderSpot != null)
            {
                Card cardTemp = Instantiate(GameManager.Instance.Cards[cardSpawnedFromDB].CardPrefab, randomCardHolderSpot);
                spawnPairOfSameCard++;
                cardTemp.Init(i, GameManager.Instance.Cards[cardSpawnedFromDB], CheckMatchedCards);
                CardsDict.Add(i, cardTemp);
            }
        }
    }

    private Transform PickARandomSpotFromTheGrid()
    {
        if (_tableManager.CardHolderDict.Count == 0)
            return null;

        int randomIndex = Random.Range(0, _tableManager.CardHolderDict.Count);
        Transform randomSpot = _tableManager.CardHolderDict[randomIndex];
        _tableManager.CardHolderDict.RemoveAt(randomIndex); // Remove the spot so it cannot be reused

        return randomSpot;
    }

    private void LevelStateChanged(LevelState levelState)
    {
        switch (levelState)
        {
            case LevelState.Starting:
                FlipDownCards();
                break;
            case LevelState.Start:
                AllCardsInteractable();
                break;
            case LevelState.Complete:
                break;
        }
    }

    private void FlipDownCards()
    {
        StartCoroutine(FlipDownCardsCoroutine()); // Make a delay between each card to flip, for animation purposes
    }

    private IEnumerator FlipDownCardsCoroutine()
    {
        foreach (var card in CardsDict)
        {
            card.Value.FlipDown();
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.ChangeState(LevelState.Start); // When Memorize timer goes off and the animation finishes, the game phase should start
    }

    private void AllCardsInteractable()
    {
        foreach (var card in CardsDict)
        {
            card.Value.ChangeInteractability(true);
        }
    }

    public void CheckMatchedCards(Card card)
    {
        Debug.Log("Check Matched Cards");
        if (lastClickedCard == null)
        {
            lastClickedCard = card;
            return;
        }

        if (lastClickedCard.CardTags == card.CardTags)
        {
            Debug.Log("Matched");
            lastClickedCard.CardMatched();
            card.CardMatched();
        }
        else
        {
            Debug.Log("Not Matched");
            lastClickedCard.FlipDownAfterDelay();
            card.FlipDownAfterDelay();
        }

        lastClickedCard = null;
    }
}
