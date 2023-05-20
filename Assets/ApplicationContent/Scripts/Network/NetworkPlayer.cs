using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/**
 Класс, отвечающий за синхронизацию локального игрока и его отображения на сервере

Данный класс используется в prefab-е игрока, находящегося по пути Assets/Resources/Avatars.


Данный класс отвечает за:
- синхронизацию положения головы (шлема oculus);
- синхронизацию положения рук (контроллеров oculus/отслеживаемых человеческих рук);
- вид контроллеров (контроллеры oculus/отслеживаемые человеческие руки);

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkVariables;
- HandView;
- OVRCameraRig;
- ControllerEvents;

@param head Объект отображения головы на сервере (голова в prefab-е игрока).
@param leftHand Объект отображения левой руки на сервере (левая рука в prefab-е игрока).
@param rightHand Объект отображения правой руки на сервере (правая рука в prefab-е игрока).
@param controllertTypeController Массив ControllerTypeController, которыми управляет данный класс.
@param dublicateMainPlayer Если true, то пользователь будет видеть как его контроллеры отображаются на сервере.
@see ControllerTypeController; HandView; ControllerEvents; NetworkVariables; ComponentCatcher
 */
[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private ControllerTypeController[] _controllertTypeController;

    [SerializeField] private bool _dublicateMainPlayer;

    private bool _isAttachToController;

    /// Объект синхронизации Photon
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
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
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

        if (_photonView.IsMine)
        {
            if (!_dublicateMainPlayer)
            {
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
        }

    }

    /**
     Метод для установки положения и поворота одного объекта, согласно соответствующим характеристикам другого объекта.
    @param target Объект, которому устанавливаются положение и поворот.
    @param rigTransform Объект, из которого берутся положение и поворот для первого объекта.
     */
    protected void MapPosition(Transform target, Transform rigTransform)
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
