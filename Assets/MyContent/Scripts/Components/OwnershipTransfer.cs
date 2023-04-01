using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]
public class OwnershipTransfer : MonoBehaviourPun, IPunOwnershipCallbacks
{
    private VRLoggersManager _vrLogger;
    private Rigidbody _rigidBody;
    private bool _isGrab;

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
        _rigidBody = GetComponent<Rigidbody>();
        _isGrab = false;
    }

    private void Start()
    {
        ComponentCatcher componentCatcher = FindFirstObjectByType<ComponentCatcher>();
        if (componentCatcher == null)
        {
            Debug.LogWarning("[" + this.name + "] Can not find ComponentCatcher in scene");
        }
        else
        {
            _vrLogger = componentCatcher.GetVRLoggersManager();
        }
    }

    private void Update()
    {
        if (!_isGrab)
        {
            if (_rigidBody.isKinematic)
            {
                _isGrab = true;
                base.photonView.RequestOwnership();
            }
        }
        else
        {
            if (!_rigidBody.isKinematic)
            {
                _isGrab = false;
            }
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SimpleCapsuleWithStickMovement player)
             /*collision.gameObject.TryGetComponent(out GrabInteractor playerController)
              * || collision.gameObject.TryGetComponent(out HandGrabInteractable playerHands)*/
             )
        {
            base.photonView.RequestOwnership();
        }
    }

}
