using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeControllerTypeEvent : MonoBehaviour
{
    public event UnityAction<Vector3,Vector3,Vector3> MoveUI;
    
    [SerializeField] private SkinnedMeshRenderer _controllerRenderer;
    [SerializeField] private Vector3 _uiLocation;
    [SerializeField] private Vector3 _uiRotation;    
    [SerializeField] private Vector3 _uiScale;
    [SerializeField] private Vector3 _netHandsRotation;

    private bool isControllerVisible;

    private void Start()
    {
        isControllerVisible = false;
    }

    private void Update()
    {
        if (_controllerRenderer.isVisible != isControllerVisible)
        {
            isControllerVisible = _controllerRenderer.isVisible;
            if (isControllerVisible)
            {
                MoveUI?.Invoke(_uiLocation,_uiRotation,_uiScale);
            }
        }        
    }
}
