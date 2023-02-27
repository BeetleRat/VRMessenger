using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandUIManager : MonoBehaviour
{
    [SerializeField] private List<ChangeControllerTypeEvent> _uiHandHiders;
    private void Start()
    {
        _uiHandHiders.RemoveAll(item => item == null);
        foreach (ChangeControllerTypeEvent hider in _uiHandHiders)
        {
            hider.MoveUI += OnMoveUI;
        }
    }
    private void OnMoveUI(Vector3 newLocation, Vector3 newRotation, Vector3 newSacle)
    {
        gameObject.transform.localPosition = newLocation;
        gameObject.transform.localRotation = Quaternion.Euler(newRotation);
        gameObject.transform.localScale = newSacle;
    }
}
