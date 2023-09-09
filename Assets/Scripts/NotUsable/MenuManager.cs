using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private List<MenuUI> _menus;
    private void Start()
    {
        instance = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (MenuUI menu in _menus)
        {
            if (menu.menuName == menuName)
            {
                menu.OpenMenu();
            }
            else
            {
                menu.CloseMenu();
            }
        }
    }
}
