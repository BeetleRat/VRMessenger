using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/**
 Скрипт для синхронизации RigidBody на сервере

Данный скрипт синхронизирует с сервером параметры RigidBody объекта:
- isKinematic;

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkVariables;
@param photonView PhotonView данного объекта
@see NetworkVariables; ComponentCatcher
 */
[RequireComponent(typeof(Rigidbody))]
public class RigidBodySycn : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    private Rigidbody _rigidbody;
    private NetworkVariables _networkVariables;
    private bool _isKinematic;
    private bool _isInit;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isKinematic = _rigidbody.isKinematic;
        _isInit = false;
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
            _networkVariables = catcher.GetNetworkVariables();
            if (_networkVariables)
            {
                _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
            }
        }
    }

    private void OnDestroy()
    {
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
        }
    }

    private void Update()
    {
        if (_rigidbody.isKinematic != _isKinematic)
        {
            _isKinematic = _rigidbody.isKinematic;
            NetworkVariables.SendPropertyToServer(PhotonServerActions.CHANGE_KINEMATIC, _isKinematic);
            _isInit = true;
        }
    }

    private void ChangeKinematic(bool isKinematic)
    {
        _rigidbody.isKinematic = isKinematic;
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PhotonServerActions.CHANGE_KINEMATIC))
        {
            if (!_photonView.IsMine)
            {
                if (targetPlayer == _photonView.Owner)
                {
                    ChangeKinematic((bool)changedProps[PhotonServerActions.CHANGE_KINEMATIC]);
                    _isInit = true;
                }
            }
        }

        if (changedProps.ContainsKey(PhotonServerActions.UPDATE_STATUS))
        {
            if (_isInit)
            {
                NetworkVariables.SendPropertyToServer(PhotonServerActions.CHANGE_KINEMATIC, _isKinematic);
            }
        }
    }
}
