using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// ��� �������� ���� ������������.
public enum ControllerType
{
    OculusController, ///< ����������� Oculus.
    HandsPrefabs ///< ���� ������.
}

/**
 ### ���������� �������� ���������� � ������������ �� ������� �����������
 */
public class ControllerModel : MonoBehaviour
{
    /// PhotonView ���������� �� ������������� ������� �������.
    [SerializeField] protected PhotonView _myPhotonView;

    /// ������������ ������ �����������.
    [SerializeField] protected GameObject _objectModel;

    /** ControllerType ������������� �����������.
     - OculusController;
     - HandsPrefabs;
     */
    [SerializeField] protected ControllerType _type;
    /**
     ��� �����������.
    - OVRInput.Controller.LTouch;
    - OVRInput.Controller.RTouch;
    - OVRInput.Controller.LHand;
    - OVRInput.Controller.RHand;
     */
    [SerializeField] protected OVRInput.Controller _controllerType;

    /**
     ������ ������ �����������.
    @return GameObject ������������ ������ �����������.
     */
    public GameObject GetObjectModel()
    {
        return _objectModel;
    }

    /**
     ������ ���� �����������.
    @return ControllerType ������� �����������
     */
    public ControllerType GetControllerType()
    {
        return _type;
    }

    /** 
     ��������� ������������� ������� �������. 
 
     ����� �� ������������� �� ������ ����������� ������������, �� ������ ��������� ���������� � �������� ����� �����������.
     */
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
