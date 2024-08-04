using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMB<GameManager>
{
    [SerializeField] private List<Level_SO> levels = new List<Level_SO>();
    public List<Level_SO> Levels { get { return levels; } }

    [SerializeField] private int currentLevelIndex =0;
    public int CurrentLevelIndex {  get { return currentLevelIndex; } }

    [Header("Custom Level Parameters")]
    [SerializeField] private bool customLevelOn = false;
    public bool CustomLevelOn { get { return customLevelOn; } }

    [SerializeField] private Level_SO customLevel;
    public Level_SO CustomLevel { get { return customLevel; } }

    [Header("Cards")]
    [SerializeField] private List<Card_SO> cards = new List<Card_SO>();
    public List<Card_SO> Cards { get { return cards; } }

    public int CardInDBSize { get { return cards.Count; } }

    public void PlayCustomGame(int W,int H)
    {
        customLevelOn = true;
        ChangeCustomLevelValues(W, H);
        SceneNavigator.Instance.LoadIndexScene(1);
    }

    public void ChangeCustomLevelValues(int W,int H)
    {
        customLevel.W = W;
        customLevel.H = H;
    }

    public void ReloadScene()
    {
        // Get the active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Reload the active scene
        SceneNavigator.Instance.LoadIndexScene(activeScene.buildIndex);
    }

    public void PlayLevel(int index)
    {
        currentLevelIndex = index;
        SceneNavigator.Instance.LoadIndexScene(1);
    }
}
