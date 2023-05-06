using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/**
### Скрипт для синхронизации RigidBody на сервере

Данный скрипт синхронизирует с сервером параметры RigidBody объекта:
- isKinematic;

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkVariables;
 */
[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PhotonView))]
public class RigidBodySycn : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private NetworkVariables _networkVariables;
    private bool _isKinematic;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody>();
        _isKinematic = _rigidbody.isKinematic;
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
            NetworkVariables.SendPropertyToServer(PlayersProperty.CHANGE_KINEMATIC, _isKinematic);
        }
    }

    private void ChangeKinematic(bool isKinematic)
    {
        _rigidbody.isKinematic = isKinematic;
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayersProperty.CHANGE_KINEMATIC))
        {
            if (!_photonView.IsMine)
            {
                if (targetPlayer == _photonView.Owner)
                {
                    ChangeKinematic((bool)changedProps[PlayersProperty.CHANGE_KINEMATIC]);
                }
            }
        }
    }
}
