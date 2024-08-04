using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMB<ScoreManager>
{
    public delegate void OnTimerChangedDelegate(int timer);
    public OnTimerChangedDelegate OnTimerChangedEvent;

    public delegate void OnMatchesChangedDelegate(int matches);
    public OnMatchesChangedDelegate OnMatchesChangedEvent;

    public delegate void OnTurnsChangedDelegate(int turns);
    public OnTurnsChangedDelegate OnTurnsChangedEvent;

    public delegate void OnMaxComboChangedDelegate(int maxCombo);
    public OnMaxComboChangedDelegate OnMaxComboChangedEvent;

    public delegate void OnComboChangedDelegate(int currentCombo);
    public OnComboChangedDelegate OnComboChangedEvent;

    private float score = 0;
    private int currentCombo = 0;
    private int maxCombo = 0;
    private int timer = 0;
    private int matches = 0;
    private int turns = 0;
    private int stars = 0;
    public int Timer { get { return timer; } }
    private Coroutine timerCoroutine;

    protected override void Awake()
    {
        base.Awake();
        LevelManager.Instance.OnLevelStateChangedEvent += LevelStateChanged;
    }

    private void LevelStateChanged(LevelState levelState)
    {
        switch (levelState)
        {
            case LevelState.Start:
                StartTimer();
                break;
            case LevelState.Completing:
                StopTimer();
                CalculateScore();
                break;
        }
    }

    public void TryMatchCard(bool cardMatched)
    {
        currentCombo = cardMatched ? currentCombo + 1 : 0;

        if (cardMatched)
            OnComboChangedEvent?.Invoke(currentCombo);

        if (currentCombo > maxCombo)
        {
            maxCombo = currentCombo;
            MaxComboChanged();
        }
    }

    public void MatchesDone()
    {
        matches++;
        OnMatchesChangedEvent?.Invoke(matches);
    }

    public void TurnsDone()
    {
        turns++;
        OnTurnsChangedEvent?.Invoke(turns);
    }

    public void MaxComboChanged()
    {
        OnMaxComboChangedEvent?.Invoke(maxCombo);
    }

    public void BreakCombo()
    {
        currentCombo = 0;
        OnComboChangedEvent?.Invoke(currentCombo); // break combo event
    }

    private void StartTimer()
    {
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }


    private IEnumerator TimerCoroutine()
    {
        timer = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;
            OnTimerChangedEvent?.Invoke(timer);
        }
    }

    private void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    private void CalculateScore()
    {

    }
}
