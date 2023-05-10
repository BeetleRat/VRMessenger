using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
Класс, отслеживающий переключение с контроллеров на руки
 */
public class ControllerEvents : MonoBehaviour
{
    /// Событие переключения контроллеров на руки или наоборот.
    public UnityAction<bool> ControllerTypeChange;

    private bool _isAttachToController;

    private void Start()
    {
        _isAttachToController = OVRPlugin.GetHandTrackingEnabled();
        ControllerTypeChange?.Invoke(_isAttachToController);
    }

    private void Update()
    {
        if (OVRPlugin.GetHandTrackingEnabled())
        {
            if (_isAttachToController)
            {
                _isAttachToController = false;
                ControllerTypeChange?.Invoke(_isAttachToController);
            }
        }
        else
        {
            if (!_isAttachToController)
            {
                _isAttachToController = true;
                ControllerTypeChange?.Invoke(_isAttachToController);
            }
        }
    }

    /**
    Геттер текущего состояния.
    @return bool Если true, значит в данный момент используются контроллеры.
    Если false, значит в данный момент используются руки.
     */
    public bool IsAttachToControllerNow()
    {
        return _isAttachToController;
    }
}
