using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum ControllerType
{
    OculusController, HandsPrefabs
}

[System.Serializable]
public class ControllerView
{
    public GameObject ControllerModel;
    public Animator Animator;
    public ControllerType Type;
}

public class ModelAnimator : MonoBehaviour
{
    [SerializeField] protected ControllerView _controller;
    [SerializeField] protected OVRInput.Controller _controllerType;
    [SerializeField] protected PhotonView _myPhotonView;

    public ControllerView GetControllerView()
    {
        return _controller;
    }

    public ControllerType GetControllerType()
    {
        return _controller.Type;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
