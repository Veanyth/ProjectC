using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card_", menuName = "Scriptable Object/Card")]
public class Card_SO : ScriptableObject
{
    [SerializeField] private CardTags tag;
    public CardTags Tag { get { return tag; } }

    [SerializeField] private Card cardPrefab;
    public Card CardPrefab { get { return cardPrefab; } }

    [SerializeField] private Sprite cardSprite;
    public Sprite CardSprite { get { return cardSprite; } }
}
