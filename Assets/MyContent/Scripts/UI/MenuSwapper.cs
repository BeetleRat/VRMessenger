using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwapper : MonoBehaviour
{
    [SerializeField] private InterfaceHider[] _menuUIs;
    private void Start()
    {
        HideAll();
    }

    public void SwitchMenu(int menuNumber)
    {
        HideAll();
        if (menuNumber >= 0 && menuNumber < _menuUIs.Length)
        {           
            _menuUIs[menuNumber].ShowInterface();
        }
    }

    public void HideAll()
    {
        foreach (InterfaceHider hider in _menuUIs)
        {
            hider.HideInterface();
        }
    }
}
