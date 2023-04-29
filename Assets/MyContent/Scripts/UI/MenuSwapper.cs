using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 ### Скрипт для переключения различных UI через InterfaceHider

Переключение вкладок меню может быть реализовано различными способами.
В данном случае, у нас есть множество UI, каждый из которых это отдельная вкладка меню.
Данный скрипт отображает одну из этих вкладок и скрывает все остальные.
@param menuUIs Массив InterfaceHider, которым управляет данный скрипт.
 */
public class MenuSwapper : MonoBehaviour
{
    [SerializeField] private InterfaceHider[] _menuUIs;

    private void Start()
    {
        HideAll();
    }

    /**
     Метод переключения на другую вкладку меню.
    @param menuNumber Индекс вкладки, на которую необходимо переключиться.
     */
    public void SwitchMenu(int menuNumber)
    {
        HideAll();
        if (menuNumber >= 0 && menuNumber < _menuUIs.Length)
        {
            _menuUIs[menuNumber].ShowInterface();
        }
    }

    /// Метод для скрытия всех вкладок меню.
    public void HideAll()
    {
        foreach (InterfaceHider hider in _menuUIs)
        {
            hider.HideInterface();
        }
    }
}
