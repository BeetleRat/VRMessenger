using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// Тип внешнего вида контроллеров.
public enum ControllerType
{
    OculusController, ///< Контроллеры Oculus.
    HandsPrefabs ///< Руки игрока.
}

/**
Суперкласс хранящий информацию о отображаемом на сервере контроллере
 */
public class ControllerModel : MonoBehaviour
{
    /// PhotonView отвечающий за синхронизацию данного объекта.
    [SerializeField] protected PhotonView _myPhotonView;

    /// Отображаемая модель контроллера.
    [SerializeField] protected GameObject _objectModel;

    /** ControllerType отображаемого контроллера.
     - OculusController;
     - HandsPrefabs;
     */
    [SerializeField] protected ControllerType _type;
    /**
     Тип контроллера.
    - OVRInput.Controller.LTouch;
    - OVRInput.Controller.RTouch;
    - OVRInput.Controller.LHand;
    - OVRInput.Controller.RHand;
     */
    [SerializeField] protected OVRInput.Controller _controllerType;

    /**
     Геттер модели контроллера.
    @return GameObject используемая модель контроллера.
     */
    public GameObject GetObjectModel()
    {
        return _objectModel;
    }

    /**
     Геттер типа контроллера.
    @return ControllerType данного контроллера
     */
    public ControllerType GetControllerType()
    {
        return _type;
    }

    /** 
     Установка существования данного объекта. 
 
     Когда мы переключаемся на другое отображение контроллеров, мы должны отключить предыдущее и включить новое отображение.
     */
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
