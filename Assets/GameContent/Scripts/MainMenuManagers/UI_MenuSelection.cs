using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuSelection : MonoBehaviour
{
    [SerializeField] private List<UI_Button> buttons = new List<UI_Button>();
    [SerializeField] private List<GameObject> menus = new List<GameObject>();


    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int idTemps = i;
            buttons[idTemps].Init(idTemps);
            buttons[idTemps].button.onClick.AddListener(() => SelectMenu(idTemps));
        }

        SelectMenu(0); // By Default Select Menu 0
    }

    private void SelectMenu(int id)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == id)
            {
                menus[i].gameObject.SetActive(true);
                buttons[i].Selected(true);
            }
            else
            {
                menus[i].gameObject.SetActive(false);
                buttons[i].Selected(false);
            }
        }
    }
}
