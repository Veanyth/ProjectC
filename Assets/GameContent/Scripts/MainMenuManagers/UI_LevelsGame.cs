using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelsGame : MonoBehaviour
{
    [SerializeField] private UI_Level ui_levelPrefab;
    [SerializeField] Transform levelsGridContainer;

    private List<Level_SO> levels = new List<Level_SO>();

    private Dictionary<int, UI_Level> levelsDict = new Dictionary<int, UI_Level>();

    private void Awake()
    {
        levels = GameManager.Instance.Levels;

        foreach (Level_SO level in levels)
        {
            UI_Level ui_levelTemp = Instantiate(ui_levelPrefab, levelsGridContainer);
            ui_levelTemp.Init(level);
            levelsDict.Add(ui_levelTemp.LevelIndex, ui_levelTemp);
        }
    }

    private void OnEnable()
    {
        ShowLevelsAnimation();
    }

    private void ShowLevelsAnimation()
    {
        StartCoroutine(ShowLevelsAnimationCoroutine());
    }

    private IEnumerator ShowLevelsAnimationCoroutine()
    {
        foreach (var uilevel in levelsDict)
        {
            uilevel.Value.ShowAnimation();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
