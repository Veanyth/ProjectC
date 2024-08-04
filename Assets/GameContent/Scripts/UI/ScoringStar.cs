using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoringStar : MonoBehaviour
{
    [SerializeField] private Image starImage;
    public void Init(Transform starHolder, Action shakeUI)
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = starHolder.localPosition + new Vector3(transform.localPosition.x + 50, transform.localPosition.y + 150, 0);
        starImage.DOFade(0, 0);
        transform.DOScale(1, 0.5f);
        transform.DOLocalMove(Vector3.zero, 2).SetEase(Ease.InQuint);
        transform.DORotate(new Vector3(0, 0, 720f), 2f, RotateMode.FastBeyond360).SetEase(Ease.InQuint).OnComplete(() =>
        shakeUI());
    }
}
