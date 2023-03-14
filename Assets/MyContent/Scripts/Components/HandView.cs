using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandType { Left, Right };


public class HandView : MonoBehaviour
{
    [SerializeField] private HandType _handType;
    [SerializeField] private ComponentCatcher _catcher;

    public HandType GetHandType()
    {
        return _handType;
    }

    public ControllerEvents GetControllerSwitcher()
    {
        return _catcher.GetControllerEvents();
    }
}
