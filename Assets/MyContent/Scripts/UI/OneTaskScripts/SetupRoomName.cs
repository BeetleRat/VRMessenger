using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

/**
Скрипт для установки имени текущей комнаты в указанное текстовое поле

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkManager;

@param roomNameText Текстовое поле в которое будет записано "Имя комнаты: " + имя_текущей_комнаты.
@see NetworkManager; ComponentCatcher
 */
public class SetupRoomName : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameText;

    private NetworkManager _networkManager;

    private void Start()
    {
        ComponentCatcher catcher = FindObjectOfType<ComponentCatcher>();
        _networkManager = catcher?.GetNetworkManager();
        if (_networkManager)
        {
            _networkManager.NetworConnectionEvent += OnNetworConnection;
        }

        _roomNameText.text = "Имя комнаты: Вы не находитесь в комнате";
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
    }

    private void OnNetworConnection(NetworkCode code)
    {
        switch (code)
        {
            case NetworkCode.CONNECT_TO_ROOM_COMPLETE:
                if (PhotonNetwork.CurrentRoom != null || PhotonNetwork.CurrentRoom.Name == "")
                {
                    _roomNameText.text = "Имя комнаты: " + PhotonNetwork.CurrentRoom?.Name;
                }
                else
                {
                    _roomNameText.text = "Имя комнаты: Вы не находитесь в комнате";
                }
                break;
            case NetworkCode.CONNECT_TO_LOBBY_COMPLETE:
                _roomNameText.text = "Имя комнаты: Вы находитесь в лобби";
                break;
        }
    }
}
