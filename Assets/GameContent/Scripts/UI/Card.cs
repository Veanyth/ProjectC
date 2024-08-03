using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private RectTransform frontPanel;
    [SerializeField] private RectTransform backPanel;

    [SerializeField] private Image cardImage;
    [SerializeField] private int _id;
    public int ID { get { return _id; } }
    private CardTags cardTags;
    public CardTags CardTags { get { return cardTags; } }

    public void Init(int id,Card_SO card_SO)
    {
        _id = id;
        cardTags = card_SO.Tag;
        cardImage.sprite = card_SO.CardSprite;
    }
    private void Update()
    {
        // Get the rotation on the Y axis
        float yRotation = transform.eulerAngles.y;

        // Determine which panel to show
        bool showFront = (yRotation < 90 || yRotation > 270);
        bool showBack = !showFront;

        frontPanel.gameObject.SetActive(showFront);
        backPanel.gameObject.SetActive(showBack);
    }
}
