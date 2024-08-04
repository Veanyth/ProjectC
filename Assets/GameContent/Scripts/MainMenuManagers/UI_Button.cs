using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{

    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image backgroundImage;

    [SerializeField] Color selectedTextColor;
    [SerializeField] Color selectedBackgroundColor; 
    
    [SerializeField] Color deselectedTextColor;
    [SerializeField] Color deselectedBackgroundColor;

    private int id;
    public int ID { get { return id; } }

    public void Init(int ID)
    {
        id = ID;
    }

    public void Selected(bool state)
    {
        titleText.color= state ? selectedTextColor : deselectedTextColor;
        backgroundImage.color= state ? selectedBackgroundColor : deselectedBackgroundColor;
    }
}
