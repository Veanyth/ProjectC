using DG.Tweening;
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

    private bool interactable = false;
    private float initialYLocalPos;
    private Vector3 frontVector = new Vector3(0, 0, 0);
    private Vector3 backVector = new Vector3(0, 180, 0);

    public void Init(int id, Card_SO card_SO)
    {
        transform.localPosition = Vector3.zero;
        _id = id;
        cardTags = card_SO.Tag;
        cardImage.sprite = card_SO.CardSprite;
        initialYLocalPos = transform.localPosition.y;
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

    public void FlipDown()
    {
        transform.DOLocalMoveY(initialYLocalPos + 10f, 0.2f).OnComplete(() =>
        transform.DOLocalMoveY(initialYLocalPos, 0.2f));
        transform.DORotate(backVector,0.4f).OnComplete(() =>
        interactable = true);
        // should be after animation finishes
    }
}
