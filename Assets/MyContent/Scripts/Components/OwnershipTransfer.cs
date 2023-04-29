using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Photon.Pun;
using Photon.Realtime;

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
        _grabbable.WhenPointerEventRaised += HandlePointerEventRaised;
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
        _grabbable.WhenPointerEventRaised -= HandlePointerEventRaised;
    }

    private void HandlePointerEventRaised(PointerEvent evt)
    {
        switch (evt.Type)
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

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
        {
            return;
        }

        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        _vrLogger?.Log(this.name + " is change owner to " + targetView.name);
        Debug.Log(this.name + " is change owner to " + targetView.name);
        if (targetView != base.photonView)
        {
            return;
        }
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        _vrLogger?.Log("Ownership Transfer Failed");
        Debug.LogWarning("Ownership Transfer Failed");
    }
}
