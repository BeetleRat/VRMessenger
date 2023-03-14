using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum ControllerType
{
    OculusController, HandsPrefabs
}

[System.Serializable]
class ControllerView
{
    public GameObject ControllerModel;
    public Animator Animator;
    public ControllerType Type;
}

public class MyControllerPrefab : MonoBehaviour
{
    [SerializeField] private ControllerView[] _controllers;
    [SerializeField] OVRInput.Controller _controller;
    [SerializeField] PhotonView _myPhotonView;

    private ControllerType _currentControllerType;
    private ControllerView _currentControllerView;

    private void Start()
    {
        SwitchControllerView(ControllerType.OculusController);
    }

    private void Update()
    {
        UpdateAnimation(_currentControllerView.Animator);
    }
    public void SwitchControllerView(ControllerType controllerType)
    {
        _currentControllerType = controllerType;
        if (_currentControllerView == null || _currentControllerView.Type != controllerType)
        {
            foreach (ControllerView controllerView in _controllers)
            {
                if (controllerView.Type == controllerType)
                {
                    controllerView.ControllerModel.SetActive(true);
                    _currentControllerView = controllerView;
                }
                else
                {
                    controllerView.ControllerModel.SetActive(false);
                }
            }
        }
    }

    private void UpdateAnimation(Animator animator)
    {
        if (animator != null && _myPhotonView.IsMine)
        {
            switch (_currentControllerType)
            {
                case ControllerType.OculusController:
                    animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, _controller) ? 1.0f : 0.0f);
                    animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, _controller) ? 1.0f : 0.0f);
                    animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, _controller) ? 1.0f : 0.0f);

                    animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controller).x);
                    animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controller).y);
                    break;
                case ControllerType.HandsPrefabs:
                    break;
            }

            animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _controller));
            animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller));
        }
    }
}
