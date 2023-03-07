using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private bool _dublicateMainPlayer;


    private PhotonView _photonView;
    private Transform _headRig;
    private Transform _leftHandRig;
    private Transform _rightHandRig;


    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        OVRCameraRig ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        _headRig = ovrCameraRig.transform.Find("TrackingSpace/CenterEyeAnchor");
        _leftHandRig = ovrCameraRig.transform.Find("TrackingSpace/LeftHandAnchor");
        _rightHandRig = ovrCameraRig.transform.Find("TrackingSpace/RightHandAnchor");

        if (_photonView.IsMine && !_dublicateMainPlayer)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            MapPosition(_head, _headRig);
            MapPosition(_leftHand, _leftHandRig);
            MapPosition(_rightHand, _rightHandRig);
        }
    }

    private void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

}
