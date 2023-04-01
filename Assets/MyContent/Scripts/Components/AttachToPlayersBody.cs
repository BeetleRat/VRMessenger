using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class AttachToPlayersBody : MonoBehaviour
{
    [SerializeField] private Transform _controllerBodyPart;
    [SerializeField, Optional] private Transform _handBodyPart;

    [SerializeField] private ComponentCatcher _catcher;

    private Vector3 _currentPosition;
    private Vector3 _currentRotation;

    private ControllerEvents _controllerEvents;

    private void Start()
    {

        _controllerEvents = _catcher.GetControllerEvents();
        _controllerEvents.ControllerTypeChange += OnAttachChange;

        _currentPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        _currentRotation = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        gameObject.transform.parent = _controllerBodyPart;

        OnAttachChange(!OVRPlugin.GetHandTrackingEnabled());

        if (_handBodyPart == null)
        {
            _handBodyPart = _controllerBodyPart;
        }
    }

    private void OnDestroy()
    {
        if (_controllerEvents != null)
        {
            _controllerEvents.ControllerTypeChange -= OnAttachChange;
        }
    }

    private void OnAttachChange(bool isAttachToController)
    {
        if (isAttachToController)
        {
            gameObject.transform.parent = _controllerBodyPart;
            RestoreLocalTransform();
        }
        else
        {
            gameObject.transform.parent = _handBodyPart;
            RestoreLocalTransform();
        }
    }

    private void RestoreLocalTransform()
    {
        transform.localPosition = new Vector3(_currentPosition.x, _currentPosition.y, _currentPosition.z);
        transform.localRotation = Quaternion.Euler(new Vector3(_currentRotation.x, _currentRotation.y, _currentRotation.z));
    }
}
