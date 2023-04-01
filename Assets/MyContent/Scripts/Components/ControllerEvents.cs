using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerEvents : MonoBehaviour
{
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

    public bool IsAttachToControllerNow()
    {
        return _isAttachToController;
    }
}
