using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private ControllerTypeController[] _controllertTypeController;

    [SerializeField] private bool _dublicateMainPlayer;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _handsOpacity;
    private bool _isAttachToController;

    private PhotonView _photonView;
    private Transform _headRig;
    private Transform _leftHandRig;
    private Transform _rightHandRig;

    private HandView[] _handViews;
    private ControllerEvents _controllerChangeTypeEvent;

    private NetworkVariables _networkVariables;


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        CreatePlayer();
        ComponentCatcher catcher = FindObjectOfType<ComponentCatcher>();
        _networkVariables = catcher?.GetNetworkVariables();
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
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
        if (_controllerChangeTypeEvent)
        {
            _controllerChangeTypeEvent.ControllerTypeChange -= OnControllerChange;
        }

        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
        }
    }

    private void CreatePlayer()
    {
        OVRCameraRig ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        _headRig = ovrCameraRig.transform.Find("TrackingSpace/CenterEyeAnchor");

        _handViews = FindObjectsOfType<HandView>();
        foreach (HandView handView in _handViews)
        {
            if (handView.GetHandType() == HandType.Left)
            {
                _leftHandRig = handView.transform;
            }
            else
            {
                _rightHandRig = handView.transform;
            }
            if (_controllerChangeTypeEvent == null)
            {
                _controllerChangeTypeEvent = handView.GetControllerSwitcher();
                if (_controllerChangeTypeEvent != null)
                {
                    _controllerChangeTypeEvent.ControllerTypeChange += OnControllerChange;
                    OnControllerChange(_controllerChangeTypeEvent.IsAttachToControllerNow());
                }
            }

        }

        if (!_dublicateMainPlayer)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
        else
        {
            if (_photonView.IsMine)
            {
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        // Устанавливаем прозрачность дублирующим рукам
                        Color oldColor = renderer.materials[i].color;
                        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, _handsOpacity);
                        renderer.materials[i].SetColor("_Color", newColor);
                    }
                }
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
        if (_photonView.IsMine)
        {
            _isAttachToController = isAttachToController;
            if (isAttachToController)
            {
                ChangeControllerView(ControllerType.OculusController);
            }
            else
            {
                ChangeControllerView(ControllerType.HandsPrefabs);
            }
        }
    }
    private void ChangeControllerView(ControllerType type)
    {
        // Обновляем локального player-a
        ChangeLocalControllerView(type);
        // Обновляем player-a на сервере через свойства photon
        NetworkVariables.SendPropertyToServer(PlayersProperty.CONTROLLER_TYPE, type);
    }

    private void ChangeLocalControllerView(ControllerType type)
    {
        foreach (ControllerTypeController myControllerrPrefab in _controllertTypeController)
        {
            myControllerrPrefab.SwitchControllerView(type);
        }
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayersProperty.CONTROLLER_TYPE))
        {
            if (!_photonView.IsMine)
            {
                if (targetPlayer == _photonView.Owner)
                {
                    ChangeLocalControllerView((ControllerType)changedProps[PlayersProperty.CONTROLLER_TYPE]);
                }
            }
        }

        if (changedProps.ContainsKey(PlayersProperty.UPDATE_STATUS))
        {
            OnControllerChange(_isAttachToController);
        }
    }
}
