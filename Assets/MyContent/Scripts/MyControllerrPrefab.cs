using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ControllerType
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

public class MyControllerrPrefab : MonoBehaviour
{
    [SerializeField] private ControllerView[] _controllers;
    [SerializeField] OVRInput.Controller _controller;
    [SerializeField] private ControllerType _controllerType;

    private ControllerView currentControllerView;

    private void Start()
    {
        switchControllerView(_controllerType);
    }

    private void Update()
    {
        switchControllerView(_controllerType);
        UpdateAnimation(currentControllerView.Animator);
    }

    private void switchControllerView(ControllerType controllerType)
    {
        if (currentControllerView == null || currentControllerView.Type != _controllerType)
        {
            foreach (ControllerView controllerView in _controllers)
            {
                if (controllerView.Type == _controllerType)
                {
                    controllerView.ControllerModel.SetActive(true);
                    currentControllerView = controllerView;
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
        if (animator != null)
        {
            animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, _controller) ? 1.0f : 0.0f);
            animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, _controller) ? 1.0f : 0.0f);
            animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, _controller) ? 1.0f : 0.0f);

            animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controller).x);
            animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controller).y);

            animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _controller));
            animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller));
        }
    }
}
