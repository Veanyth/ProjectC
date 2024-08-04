using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Scriptable Object/Level")]
public class Level_SO : ScriptableObject
{
    [SerializeField] private int levelID;
    public int LevelID { get { return levelID; } }
    [SerializeField] private int w = 2;
    [SerializeField] private int h = 2;
    [SerializeField] private float memorizeTimer = 5f;

    public int W
    {
        get { return w; }
        set { w = Mathf.Clamp(value, 2, 6); }
    }

    public int H
    {
        get { return h; }
        set { h = Mathf.Clamp(value, 2, 6); }
    }
    public float MemorizeTimer
    {
        get { return memorizeTimer; }
        set { memorizeTimer = Mathf.Clamp(value, 2, 60); }
    }

    // Ensure values are clamped when the scriptable object is loaded or reset
    private void OnValidate()
    {
        W = w;
        H = h;
        MemorizeTimer = memorizeTimer;
    }
}
