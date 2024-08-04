using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private CanvasGroup backgroundDarkCGrp;
    [SerializeField] private AudioClip levelCompleteSFX;

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

    [Header("Level Complete Section")]
    [SerializeField] private GameObject levelSectionGO;
    [SerializeField] private CanvasGroup levelSectionCGrp;

    [Header("Stars Section")]
    [SerializeField] private ScoringStar starPrefab;
    [SerializeField] private Transform[] starHolders = new Transform[3];
    [SerializeField] private Button restartLevelBtn;
    [SerializeField] private Button nextLevelBtn;

    private void Awake()
    {
        LevelManager.Instance.OnLevelStateChangedEvent += LevelStateChanged;
        ScoreManager.Instance.OnTimerChangedEvent += OnTimerChanged;
        ScoreManager.Instance.OnMatchesChangedEvent += OnMatchesChanged;
        ScoreManager.Instance.OnTurnsChangedEvent += OnTurnsChanged;
        ScoreManager.Instance.OnComboChangedEvent += OnComboChanged;
        ScoreManager.Instance.OnMaxComboChangedEvent += OnMaxComboChanged;

        restartLevelBtn.onClick.AddListener(RestartLevel);
        nextLevelBtn.onClick.AddListener(NextLevel);

        backgroundDarkCGrp.gameObject.SetActive(false);
        memorizeSectionGO.SetActive(false);
        startSectionGO.SetActive(false);
        timerSectionGO.SetActive(false);
        matchesSectionGO.SetActive(false);
        turnsSectionGO.SetActive(false);
        maxComboSectionGO.SetActive(false);
        comboSectionGO.SetActive(false);
        levelSectionGO.SetActive(false);
        restartLevelBtn.gameObject.SetActive(false);
        nextLevelBtn.gameObject.SetActive(false);

        maxComboSectionCGrp.alpha = 0;
        comboSectionCGrp.alpha = 0;
      
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
                comboSectionGO.SetActive(true);
                comboSectionCGrp.alpha = 0;
                comboSectionGO.transform.localPosition = new Vector3(comboSectionGO.transform.localPosition.x - 350f, comboSectionGO.transform.localPosition.y, comboSectionGO.transform.localPosition.z);
                comboSectionGO.transform.DOLocalMoveX(comboSectionGO.transform.localPosition.x+350, 0.3f);
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
                maxComboSectionGO.SetActive(true);
                maxComboSectionCGrp.alpha = 0;
                maxComboSectionGO.transform.localPosition = new Vector3(maxComboSectionGO.transform.localPosition.x - 250f, maxComboSectionGO.transform.localPosition.y, maxComboSectionGO.transform.localPosition.z);
                maxComboSectionGO.transform.DOLocalMoveX(maxComboSectionGO.transform.localPosition.x+250, 0.3f);
                maxComboSectionCGrp.DOFade(1, 0.15f);
            }

            maxComboScoreSectionText.text = maxCombo.ToString();
        }
    }

    private void LevelStateChanged(LevelState levelState)
    {
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
            case LevelState.Completing:
                StartCoroutine(CompletingPhaseCoroutine());
                break;
            case LevelState.Scoring:
                ShowScoring();
                break;
            case LevelState.Complete:
                CompletePhase();
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

    private IEnumerator CompletingPhaseCoroutine()
    {
        backgroundDarkCGrp.gameObject.SetActive(true);
        backgroundDarkCGrp.DOFade(1, 1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2f);

        SoundManager.Instance.PlaySFX(levelCompleteSFX);
        levelSectionGO.SetActive(true);
        levelSectionGO.transform.localScale = Vector3.one * 0.5f;
        levelSectionCGrp.alpha = 0f;
        levelSectionCGrp.DOFade(1, 0.2f);
        levelSectionGO.transform.DOScale(Vector3.one, 0.5f);

        yield return new WaitForSeconds(1f);

        LevelManager.Instance.ChangeState(LevelState.Scoring);
    }

    private void ShowScoring()
    {
        int starsCount = ScoreManager.Instance.StarsGained;
        StartCoroutine(ShowScoringCoroutine(starsCount));
    }

    private IEnumerator ShowScoringCoroutine(int starsCount)
    {
        for (int i = 0; i < starsCount; i++)
        {
            ScoringStar scoringStarTemp = Instantiate(starPrefab, starHolders[i]);
            scoringStarTemp.Init(starHolders[i], LevelCompleteSectionShake);
            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(1.5f);

        LevelManager.Instance.ChangeState(LevelState.Complete);
    }

    public void LevelCompleteSectionShake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 initialLocalPosition = levelSectionGO.transform.localPosition;
        float duration = 0.2f;
        float strength = 15f;
        int vibrato = 20;
        float randomness = 45f;

        levelSectionGO.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);

        yield return new WaitForSeconds(duration);

        levelSectionGO.transform.localPosition = initialLocalPosition;
    }

    private void CompletePhase()
    {
        restartLevelBtn.gameObject.SetActive(true);
        restartLevelBtn.transform.localScale = Vector3.zero;
        restartLevelBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);

        bool nextLevelAvailable = GameManager.Instance.CurrentLevelIndex + 1 < GameManager.Instance.Levels.Count;

        if (!GameManager.Instance.CustomLevelOn && nextLevelAvailable)
        {
            nextLevelBtn.gameObject.SetActive(true);
            nextLevelBtn.transform.localScale = Vector3.zero;
            nextLevelBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
        }
    }


    private void NextLevel()
    {
        GameManager.Instance.PlayLevel(GameManager.Instance.CurrentLevelIndex + 1);
    }

    private void RestartLevel()
    {
        Debug.Log("works");
        GameManager.Instance.ReloadScene();
    }
}
