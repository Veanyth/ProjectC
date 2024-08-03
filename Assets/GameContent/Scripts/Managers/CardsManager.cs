using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private TableManager _tableManager;

    private Dictionary<int, Card> CardsDict = new Dictionary<int, Card>();

    public void GenerateCards(TableManager tableManager)
    {
        _tableManager = tableManager;

        // Get the Number of card to spawn, if the grid size is an odd number we need to make it even for the cards to spawn,
        // otherwise we will end up with a card that doesn't have a match
        int numberOfCardToSpawn = _tableManager.CardHolderDict.Count - _tableManager.CardHolderDict.Count%2;

        int cardSpawnedFromDB = 0;
        int spawnPairOfSameCard = 0;

        for (int i = 0; i < numberOfCardToSpawn; i++)
        {
            if(spawnPairOfSameCard==2) // Check if we spawned 2 cards of the same type in the grid, if so, we need to
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
                cardTemp.Init(i, GameManager.Instance.Cards[cardSpawnedFromDB]);
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
}
