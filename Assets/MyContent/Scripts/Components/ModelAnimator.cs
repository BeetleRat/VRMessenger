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
 ### Класс, хранящий совокупность данных об отображаемом контроллере.
 */
[System.Serializable]
public class ControllerView
{
    /// Отображаемая модель контроллера.
    public GameObject ControllerModel;
    /// Аниматор, который используется для анимации модели контроллера.
    public Animator Animator;
    /** ControllerType отображаемого контроллера.
     - OculusController;
     - HandsPrefabs;
     */
    public ControllerType Type;
}

/**
 ### Суперкласс для работы с анимацией объектов, находящихся на сервере
 */
public class ModelAnimator : MonoBehaviour
{
    /// ControllerView объекта, с которым будем работать.
    [SerializeField] protected ControllerView _controller;
    /**
     Тип контроллера.
    - OVRInput.Controller.LTouch;
    - OVRInput.Controller.RTouch;
    - OVRInput.Controller.LHand;
    - OVRInput.Controller.RHand;
     */
    [SerializeField] protected OVRInput.Controller _controllerType;
    /// PhotonView отвечающий за синхронизацию данного объекта.
    [SerializeField] protected PhotonView _myPhotonView;

    /// Геттер ControllerView.
    public ControllerView GetControllerView()
    {
        return _controller;
    }

    /// Геттер ControllerType.
    public ControllerType GetControllerType()
    {
        return _controller.Type;
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
