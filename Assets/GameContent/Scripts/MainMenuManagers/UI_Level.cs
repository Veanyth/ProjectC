using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private Transform levelContainer;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject[] stars = new GameObject[3];
    [SerializeField] private GameObject lockedGO;

    private int levelIndex;
    public int LevelIndex { get { return levelIndex; } }
    private float initialYLocalPos;
    private bool locked = false;
    public void Init(Level_SO level_SO)
    {
        levelIndex = level_SO.LevelID;
        levelText.text = (levelIndex + 1).ToString();
        levelContainer.localScale = Vector3.zero;
        m_Button.onClick.AddListener(() => PlayLevel());
    }

    public void ShowAnimation()
    {
        levelContainer.localScale = Vector3.zero;
        levelContainer.DOLocalMoveY(10f, 0.2f).OnComplete(() =>
        levelContainer.DOLocalMoveY(0, 0.2f));
        levelContainer.DOScale(Vector3.one, 0.4f);
    }

    private void OnDisable()
    {
        levelContainer.localScale = Vector3.zero;
    }

    private void PlayLevel()
    {
        if (!locked)
            GameManager.Instance.PlayLevel(levelIndex);
    }
}
