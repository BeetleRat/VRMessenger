using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private MyControllerPrefab[] _myControllerrPrefabs;

    [SerializeField] private bool _dublicateMainPlayer;


    private PhotonView _photonView;
    private Transform _headRig;
    private Transform _leftHandRig;
    private Transform _rightHandRig;

    private HandView[] handViews;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        OVRCameraRig ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        _headRig = ovrCameraRig.transform.Find("TrackingSpace/CenterEyeAnchor");

        handViews = FindObjectsOfType<HandView>();
        foreach (HandView handView in handViews)
        {
            if (handView.GetHandType() == HandType.Left)
            {
                _leftHandRig = handView.transform;
            }
            else
            {
                _rightHandRig = handView.transform;
            }
            ControllerEvents controllerEvents = handView.GetControllerSwitcher();
            if (controllerEvents != null)
            {
                controllerEvents.ControllerTypeChange += OnControllerChange;
            }
        }

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

    private void OnDestroy()
    {
        foreach (HandView handView in handViews)
        {
            ControllerEvents controllerEvents = handView.GetControllerSwitcher();
            if (controllerEvents != null)
            {
                controllerEvents.ControllerTypeChange -= OnControllerChange;
            }
        }
    }

    private void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

    private void OnControllerChange(bool isAttachToController)
    {
        if (isAttachToController)
        {
            foreach (MyControllerPrefab myControllerrPrefab in _myControllerrPrefabs)
            {
                myControllerrPrefab.SwitchControllerView(ControllerType.OculusController);
            }
        }
        else
        {
            foreach (MyControllerPrefab myControllerrPrefab in _myControllerrPrefabs)
            {
                myControllerrPrefab.SwitchControllerView(ControllerType.HandsPrefabs);
            }
        }
    }

}
