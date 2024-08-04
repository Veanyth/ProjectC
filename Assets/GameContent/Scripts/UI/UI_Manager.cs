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

    [Header("Timer Section")]
    [SerializeField] private GameObject timerSectionGO;
    [SerializeField] private TextMeshProUGUI timerSectionText;

    [Header("Matches Section")]
    [SerializeField] private GameObject matchesSectionGO;
    [SerializeField] private TextMeshProUGUI matchesScoreSectionText;

    [Header("Turns Section")]
    [SerializeField] private GameObject turnsSectionGO;
    [SerializeField] private TextMeshProUGUI turnsScoreSectionText;

    [Header("MaxCombo Section")]
    [SerializeField] private GameObject maxComboSectionGO;
    [SerializeField] private CanvasGroup maxComboSectionCGrp;
    [SerializeField] private TextMeshProUGUI maxComboScoreSectionText;

    [Header("Combo Section")]
    [SerializeField] private GameObject comboSectionGO;
    [SerializeField] private CanvasGroup comboSectionCGrp;
    [SerializeField] private TextMeshProUGUI comboScoreSectionText;

    private Vector3 initLocalPosMaxCombo;
    private Vector3 initLocalPosComboCombo;

    private void Awake()
    {
        LevelManager.Instance.OnLevelStateChangedEvent += LevelStateChanged;
        ScoreManager.Instance.OnTimerChangedEvent += OnTimerChanged;
        ScoreManager.Instance.OnMatchesChangedEvent += OnMatchesChanged;
        ScoreManager.Instance.OnTurnsChangedEvent += OnTurnsChanged;
        ScoreManager.Instance.OnComboChangedEvent += OnComboChanged;
        ScoreManager.Instance.OnMaxComboChangedEvent += OnMaxComboChanged;

        memorizeSectionGO.SetActive(false);
        startSectionGO.SetActive(false);
        timerSectionGO.SetActive(false);
        matchesSectionGO.SetActive(false);
        turnsSectionGO.SetActive(false);
        maxComboSectionGO.SetActive(false);
        comboSectionGO.SetActive(false);

        maxComboSectionCGrp.alpha = 0;
        comboSectionCGrp.alpha = 0;

        // Init current LocalPosition for Combo And MaxCombo (for animations)
        initLocalPosMaxCombo = maxComboSectionGO.transform.localPosition;
        initLocalPosComboCombo = comboSectionGO.transform.localPosition;
    }

    private void OnMatchesChanged(int matches)
    {
        matchesScoreSectionText.text = matches.ToString();
        matchesScoreSectionText.transform.DOScale(Vector3.one * 1.2f, 0.1f) // Scale up animation on text change
                .OnComplete(() => matchesScoreSectionText.transform.DOScale(Vector3.one, 0.1f)); // Scale back down
    }

    private void OnTurnsChanged(int turns)
    {
        turnsScoreSectionText.text = turns.ToString();
        turnsScoreSectionText.transform.DOScale(Vector3.one * 1.2f, 0.1f) // Scale up animation on text change
                .OnComplete(() => turnsScoreSectionText.transform.DOScale(Vector3.one, 0.1f)); // Scale back down
    }

    private void OnComboChanged(int combo)
    {
        if (combo >= 2)
        {
            if (!comboSectionGO.activeSelf) // if combo showing for the first time or showing again, we play animation
            {
                comboSectionCGrp.alpha = 0;
                comboSectionGO.transform.localPosition = new Vector3(initLocalPosComboCombo.x - 350f, initLocalPosComboCombo.y, initLocalPosComboCombo.z);
                comboSectionGO.SetActive(true);
                comboSectionGO.transform.DOLocalMoveX(initLocalPosComboCombo.x, 0.3f);
                comboSectionCGrp.DOFade(1, 0.15f);
            }

            comboScoreSectionText.text = combo.ToString();
        }
        else
        {
            comboSectionCGrp.DOFade(0, 0.3f).OnComplete(() => comboSectionGO.SetActive(false));

        }
    }

    private void OnMaxComboChanged(int maxCombo)
    {
        if (maxCombo >= 2)
        {
            if (!maxComboSectionGO.activeSelf) // if max combo showing for the first time we play animation
            {
                maxComboSectionCGrp.alpha = 0;
                maxComboSectionGO.transform.localPosition = new Vector3(initLocalPosMaxCombo.x - 250f, initLocalPosMaxCombo.y, initLocalPosMaxCombo.z);
                maxComboSectionGO.SetActive(true);
                maxComboSectionGO.transform.DOLocalMoveX(initLocalPosMaxCombo.x, 0.3f);
                maxComboSectionCGrp.DOFade(1, 0.15f);
            }

            maxComboScoreSectionText.text = maxCombo.ToString();
        }
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
                StartTimer();
                ShowMatchesSection();
                ShowTurnsSection();
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

    private void StartTimer()
    {
        timerSectionGO.SetActive(true);
        timerSectionGO.transform.localScale = Vector3.zero;
        timerSectionGO.transform.DOScale(Vector3.one, 0.5f);
    }

    private void ShowMatchesSection()
    {
        matchesSectionGO.SetActive(true);
        matchesSectionGO.transform.localScale = Vector3.zero;
        matchesSectionGO.transform.DOScale(Vector3.one, 0.5f);
    }

    private void ShowTurnsSection()
    {
        turnsSectionGO.SetActive(true);
        turnsSectionGO.transform.localScale = Vector3.zero;
        turnsSectionGO.transform.DOScale(Vector3.one, 0.5f);
    }

    private void OnTimerChanged(int timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        timerSectionText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

}
