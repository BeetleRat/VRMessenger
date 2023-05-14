using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;

/**
Класс, обеспечивающий взаимодействие UI с VirtualKeyboardController.

Взаимодействуя с ComponentCatcher, данный класс предоставляет элементам UI взаимодействовать с VirtualKeyboardController.

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- VirtualKeyboardController;

@param catcher ComponentCatcher, который используется для получения доступа к VirtualKeyboardController.
@see ComponentCatcher; VirtualKeyboardController; InterfaceHider

 */
public class KeyboardProvider : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;

    private VirtualKeyboardController _keyboard;
    private GameObject _keyboardSurface;
    private InterfaceHider _keyboardHider;

    private void Start()
    {
        _keyboard = _catcher.GetVirtualKeyboard();

        if (_keyboard)
        {
            _keyboardHider = _keyboard.GetComponent<InterfaceHider>();
            _keyboardHider.OnInterfaseHide += DisableKeyboard;
            _keyboardSurface = _keyboard.GetComponentInParent<Cylinder>().gameObject;
            _keyboard.CloseVirtualKeyboard();
        }
    }

    private void OnDestroy()
    {
        _keyboardHider.OnInterfaseHide -= DisableKeyboard;
    }

    public void CreateVirtualKeyboard(TMP_InputField inputText)
    {
        _keyboardSurface?.SetActive(true);
        _keyboard?.CreateVirtualKeyboard(inputText);
    }

    public void CloseVirtualKeyboard()
    {
        _keyboard?.CloseVirtualKeyboard();
    }

    private void DisableKeyboard()
    {
        _keyboardSurface?.SetActive(false);
    }
}
