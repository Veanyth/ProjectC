using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Scriptable Object/Level")]
public class Level_SO : ScriptableObject
{
    [SerializeField] private int w = 2; 
    [SerializeField] private int h = 2;

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

    // Ensure values are clamped when the scriptable object is loaded or reset
    private void OnValidate()
    {
        W = w;
        H = h;
    }
}
