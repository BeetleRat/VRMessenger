using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerEvents : MonoBehaviour
{
    public UnityAction<bool> ControllerTypeChange;

    private bool isAttachToController;

    private void Start()
    {
        isAttachToController = OVRPlugin.GetHandTrackingEnabled();
    }

    private void Update()
    {
        if (OVRPlugin.GetHandTrackingEnabled())
        {
            if (isAttachToController)
            {
                isAttachToController = false;
                ControllerTypeChange?.Invoke(isAttachToController);
            }
        }
        else
        {
            if (!isAttachToController)
            {
                isAttachToController = true;
                ControllerTypeChange?.Invoke(isAttachToController);
            }
        }
    }
}
