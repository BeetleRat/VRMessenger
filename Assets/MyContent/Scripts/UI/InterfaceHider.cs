using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 ### Скрипт для плавного появления/скрытия интерфейса

Данный скрипт используется, если по нажатию на кнопку нам нужно показать/скрыть интерфейс.
@param hideDuration Скорость появления/скрытия интерфейса.
 */
public class InterfaceHider : MonoBehaviour
{
    [SerializeField] private float _hideDuration;

    private Vector3 _currentScale;

    private bool _isHide;

    private void Awake()
    {
        _isHide = false;
        _currentScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    /// Метод скрытия интерфейса.
    public void HideInterface()
    {
        if (!_isHide)
        {
            _isHide = true;
            transform.LeanScale(Vector3.zero, _hideDuration).setEaseInBack();
        }
    }

    /// Метод появления интерфейса.
    public void ShowInterface()
    {
        if (_isHide)
        {
            _isHide = false;
            transform.LeanScale(_currentScale, _hideDuration);
        }
    }
}
