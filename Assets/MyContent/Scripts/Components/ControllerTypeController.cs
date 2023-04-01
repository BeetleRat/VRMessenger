using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControllerTypeController : MonoBehaviour
{
    [SerializeField] private ModelAnimator[] _controllers;

    private ControllerType _currentControllerType;
    private ModelAnimator _currentController;

    private void Start()
    {
        SwitchControllerView(_currentControllerType);
    }

    public void SwitchControllerView(ControllerType controllerType)
    {
        _currentControllerType = controllerType;
        if (_currentController == null || _currentController.GetControllerType() != controllerType)
        {
            foreach (ModelAnimator controller in _controllers)
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
