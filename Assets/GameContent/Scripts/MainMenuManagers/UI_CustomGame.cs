using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CustomGame : MonoBehaviour
{
    [SerializeField] private GameObject gridWidth;
    [SerializeField] private GameObject gridheight;
    [SerializeField] private TMP_InputField gridWidthInput;
    [SerializeField] private TMP_InputField gridHeightInput;
    [SerializeField] private Button playCustomBtn;
    private Coroutine playInitAnimationCoroutine;

    private void Awake()
    {
        playCustomBtn.onClick.AddListener(PlayCustom);
        gridWidthInput.onEndEdit.AddListener(ValidateGridInput);
        gridHeightInput.onEndEdit.AddListener(ValidateGridInput);
    }


    private void OnEnable()
    {
        gridWidth.transform.localScale = Vector3.zero;
        gridheight.transform.localScale = Vector3.zero;
        playCustomBtn.transform.localScale = Vector3.zero;

        playInitAnimationCoroutine = StartCoroutine(PlayInitAnimationCoroutine());
    }

    private void OnDisable()
    {
        if (playInitAnimationCoroutine != null)
            StopCoroutine(playInitAnimationCoroutine);
    }

    private IEnumerator PlayInitAnimationCoroutine()
    {
       
        gridWidth.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.1f);

        gridheight.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.1f);

        playCustomBtn.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);

    }

    private void ValidateGridInput(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 2)
                value = 2;
            else if (value > 6)
                value = 6;

            // Update the input field to ensure the value is within the range
            TMP_InputField inputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
            inputField.text = value.ToString();
        }
    }

    private void PlayCustom()
    {
        // Get grid W and H
        int W = int.Parse(gridWidthInput.text);
        int H = int.Parse(gridHeightInput.text);

        GameManager.Instance.PlayCustomGame(W, H);
    }
}
