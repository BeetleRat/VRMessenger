using System.Collections.Generic;
using UnityEngine;

/**
Класс, отвечающий за переключение типов контроллеров на сервере

Данный класс используется в prefab-е игрока на сервере. 
Когда локальный игрок меняет тип контроллера (с контроллеров на руки или наоборот), 
данный класс изменяет меняет отображаемый тип контроллера на сервере.
@param controllers Список переключаемых контроллеров.
@see ControllerModel
 */
public class ControllerTypeController : MonoBehaviour
{
    [SerializeField] private List<ControllerModel> _controllers;

    private ControllerType _currentControllerType;
    private ControllerModel _currentController;

    private void Start()
    {
        SwitchControllerView(_currentControllerType);
    }

    private void OnDestroy()
    {
        _controllers.Clear();
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
