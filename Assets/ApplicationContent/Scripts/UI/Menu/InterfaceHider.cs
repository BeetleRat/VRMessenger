using UnityEngine;
using UnityEngine.Events;

/**
 Скрипт для плавного появления/скрытия интерфейса

Данный скрипт используется, если по нажатию на кнопку нам нужно показать/скрыть интерфейс.

@param hideDuration Скорость появления/скрытия интерфейса.
 */
public class InterfaceHider : MonoBehaviour
{
    /// Событие скрытия интерфейса.
    public UnityAction OnInterfaseHide;
    /// Событие появление интерфейса.
    public UnityAction OnInterfaseShow;

    [SerializeField] private float _hideDuration = 1f;

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
            transform.LeanScale(Vector3.zero, _hideDuration)
                .setEaseInBack()
                .setOnComplete(() => OnInterfaseHide?.Invoke());
        }
    }

    /// Метод появления интерфейса.
    public void ShowInterface()
    {
        if (_isHide)
        {
            _isHide = false;
            transform.LeanScale(_currentScale, _hideDuration).setOnComplete(() => OnInterfaseShow?.Invoke());
        }
    }

    /// Метод для переключения видимости интерфейса на противоположное.
    public void SwitchInterfaceHiding()
    {
        if (_isHide)
        {
            ShowInterface();
        }
        else
        {
            HideInterface();
        }
    }
}
