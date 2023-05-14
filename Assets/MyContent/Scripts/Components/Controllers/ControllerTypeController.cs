using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/**
Класс, отвечающий за переключение типов контроллеров на сервере

Данный класс используется в prefab-е игрока на сервере. 
Когда локальный игрок меняет тип контроллера (с контроллеров на руки или наоборот), 
данный класс изменяет меняет отображаемый тип контроллера на сервере.
@param controllers ControllerModel[] Массив переключаемых контроллеров.
@see ControllerModel
 */
public class ControllerTypeController : MonoBehaviour
{
    [SerializeField] private ControllerModel[] _controllers;

    private ControllerType _currentControllerType;
    private ControllerModel _currentController;

    private void Start()
    {
        SwitchControllerView(_currentControllerType);
    }

    public void SwitchControllerView(ControllerType controllerType)
    {
        _currentControllerType = controllerType;
        if (_currentController == null || _currentController.GetControllerType() != controllerType)
        {
            foreach (ControllerModel controller in _controllers)
            {
                if (controller.GetControllerType() == controllerType)
                {
                    controller.SetActive(true);
                    _currentController = controller;
                }
                else
                {
                    controller.SetActive(false);
                }
            }
        }
    }
}
