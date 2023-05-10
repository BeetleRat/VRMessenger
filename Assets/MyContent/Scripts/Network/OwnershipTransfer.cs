using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Photon.Pun;
using Photon.Realtime;

/**
Скрипт, передающий права на объект

В Photon у интерактивных объектов есть владелец. И Photon синхронизирует с сервером только данные владельца объекта.
Владельцем становится первый схвативший объект. 

Но нам необходимо, что бы все пользователи могли взаимодействовать с объектами. 

Для этого необходимо передавать владение объектом тому пользователю, который взял объект. 
Этим и занимается данный скрипт.

Данный скрипт вешается на объект. Если объект кто-то берет, то этот кто-то становится его владельцем, 
независимо от того есть ли у объекта сейчас другой владелец.

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- VRLoggersManager;
- NetworkVariables;
@see VRLoggersManager; ComponentCatcher
 */
[RequireComponent(typeof(Grabbable))]
public class OwnershipTransfer : MonoBehaviourPun, IPunOwnershipCallbacks
{
    private VRLoggersManager _vrLogger;
    private bool _isGrab;
    private Grabbable _grabbable;

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
        _grabbable = GetComponent<Grabbable>();        
        _grabbable.WhenPointerEventRaised += OnObjectGrabChange;
        _isGrab = false;
    }

    private void Start()
    {
        ComponentCatcher catcher = FindObjectOfType<ComponentCatcher>();
        if (catcher == null)
        {
            Debug.LogWarning("[" + this.name + "] Can not find ComponentCatcher in scene");
        }
        else
        {
            _vrLogger = catcher.GetVRLoggersManager();            
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        _grabbable.WhenPointerEventRaised -= OnObjectGrabChange;
    }

    private void OnObjectGrabChange(PointerEvent grabEvent)
    {
        switch (grabEvent.Type)
        {
            case PointerEventType.Select:
                if (!_isGrab)
                {
                    _isGrab = true;
                    base.photonView.RequestOwnership();                    
                }
                break;
            case PointerEventType.Unselect:
                if (_isGrab)
                {
                    _isGrab = false;
                }
                break;
        }
    }

    /**
     Метод, вызываемый при запросе на смену владельца.
    @param targetView PhotonView данного объекта.
    @param requestingPlayer Player, который хочет стать владельцем.
     */
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
        {
            return;
        }

        base.photonView.TransferOwnership(requestingPlayer);
    }

    /**
     Метод, вызываемый по окончании передачи прав на объект.
    @param targetView PhotonView данного объекта.
    @param previousOwner Player предыдущего владельца.
     */
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        _vrLogger?.Log(this.name + " is change owner to " + targetView.name);
        Debug.Log(this.name + " is change owner to " + targetView.name);
        if (targetView != base.photonView)
        {
            return;
        }
    }

    /**
     Метод, вызываемый при ошибке передачи прав на объект.
    @param targetView PhotonView данного объекта.
    @param requestingPlayer Player, который пытался стать владельцем.
     */
    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        _vrLogger?.Log("Ownership Transfer Failed");
        Debug.LogWarning("Ownership Transfer Failed");
    }
}
