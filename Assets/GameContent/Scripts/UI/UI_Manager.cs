using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Memorize Section")]
    [SerializeField] private GameObject memorizeSectionGO;
    [SerializeField] private TextMeshProUGUI memorizeCountdownText;

    [Header("Start Section")]
    [SerializeField] private GameObject startSectionGO;
    [SerializeField] private TextMeshProUGUI startSectionText;

    private void Awake()
    {
        LevelManager.Instance.OnLevelStateChangedEvent += LevelStateChanged;
        memorizeSectionGO.SetActive(false);
        startSectionGO.gameObject.SetActive(false);
    }

    private void LevelStateChanged(LevelState levelState)
    {
        Debug.Log("levelState changed" + levelState);
        switch (levelState)
        {
            case LevelState.Memorize:
                MemorizePhase();
                break;
            case LevelState.Start:
                StartCoroutine(StartPhaseCoroutine());
                break;
            case LevelState.Complete:
                break;
        }
    }

    private void MemorizePhase()
    {
        memorizeSectionGO.SetActive(true);
        StartCoroutine(MemorizeCountdownCoroutine(LevelManager.Instance.Level_SO.MemorizeTimer));
    }

    private IEnumerator MemorizeCountdownCoroutine(float timer)
    {
        while (timer > 0)
        {
            memorizeCountdownText.text = timer.ToString("F0");
            memorizeCountdownText.transform.DOScale(Vector3.one * 1.2f, 0.1f) // Scale up animation on text change
                 .OnComplete(() => memorizeCountdownText.transform.DOScale(Vector3.one, 0.1f)); // Scale back down

            yield return new WaitForSeconds(1f);
            timer--;
        }

        memorizeCountdownText.text = "0"; // Ensure it ends at 0
        memorizeSectionGO.transform.DOScale(Vector3.zero, 0.1f) // Scale Down until it desepears then turn it off
                 .OnComplete(() => memorizeSectionGO.SetActive(false));

    }

    private IEnumerator StartPhaseCoroutine()
    {
        startSectionGO.SetActive(true);
        startSectionGO.transform.localScale = Vector3.one * 0.5f;
        startSectionText.alpha = 0f;
        startSectionText.DOFade(1, 0.2f);
        startSectionGO.transform.DOScale(Vector3.one, 0.5f);

        yield return new WaitForSeconds(1f);

        startSectionText.DOFade(0, 0.2f).OnComplete(() =>
        startSectionGO.SetActive(false));
    }
}
