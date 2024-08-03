using DG.Tweening;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform frontPanel;
    [SerializeField] private RectTransform backPanel;

    [SerializeField] private Image cardImage;
    [SerializeField] private int _id;
    public int ID { get { return _id; } }
    private CardTags cardTags;
    public CardTags CardTags { get { return cardTags; } }

    private bool _interactable = false;
    private float initialYLocalPos;
    private Vector3 frontVector = new Vector3(0, 0, 0);
    private Vector3 backVector = new Vector3(0, 180, 0);

    private Action<Card> CheckMatchedCards;
    public void Init(int id, Card_SO card_SO, Action<Card> checkMatchedCards)
    {
        CheckMatchedCards = checkMatchedCards;

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

    public void FlipDown(bool interactable = false) // change interactable only if it's added to the parameter, otherwise it will stay false as a default value
    {
        transform.DOLocalMoveY(initialYLocalPos + 10f, 0.2f).OnComplete(() =>
        transform.DOLocalMoveY(initialYLocalPos, 0.2f));
        transform.DORotate(backVector, 0.4f).OnComplete(() =>
        ChangeInteractability(interactable));
    }

    public void FlipDownAfterDelay()
    {
        StartCoroutine(FlipDownAfterDelayCoroutine());
    }

    private IEnumerator FlipDownAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveY(initialYLocalPos + 10f, 0.2f).OnComplete(() =>
        transform.DOLocalMoveY(initialYLocalPos, 0.2f));
        transform.DORotate(backVector, 0.4f).OnComplete(() =>
        ChangeInteractability(true));
    }

    public void FlipUP()
    {
        ChangeInteractability(false);
        transform.DOLocalMoveY(initialYLocalPos + 10f, 0.2f).OnComplete(() =>
        transform.DOLocalMoveY(initialYLocalPos, 0.2f));
        transform.DORotate(frontVector, 0.4f);
    }

    public void ChangeInteractability(bool state)
    {
        _interactable = state;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked, _interactable " + _interactable);
        if (_interactable)
        {
            FlipUP();
            CheckMatchedCards(this);
        }
    }

    public void CardMatched()
    {
        StartCoroutine(CardMatchedAnimation());
    }

    public IEnumerator CardMatchedAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        float randomYValue = UnityEngine.Random.Range(50f, 20f);
        float randomXValue = UnityEngine.Random.Range(-100f, 100f);
        float randomRotation = UnityEngine.Random.Range(360f, 720f);
        transform.DOLocalMoveY(initialYLocalPos + randomYValue, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
        transform.DOLocalMoveY(initialYLocalPos + randomYValue - 50f, 0.7f).SetEase(Ease.InSine));
        transform.DOLocalMoveX(transform.localPosition.x + randomXValue, 0.7f).SetEase(Ease.OutSine);
        transform.DORotate(new Vector3(0, 0, randomRotation), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        transform.DOScale(Vector3.zero, 1f);
    }
}
