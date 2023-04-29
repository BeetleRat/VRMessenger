﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

/**
 ### Класс, хранящий настройки комнаты
 */
[System.Serializable]
public class RoomSettings
{
    /// Название комнаты.
    public string name;
    /// ID сцены, из которой будет создана комната.
    public int sceneID;
    /// Количество игроков в комнате.
    public byte playersInRoom;
    /// Будет ли комната видима.
    public bool isRoomVisible;
}

/// Коды состояний сервера
public enum NetworkCode
{
    NO_CODE = 619, ///< Отсутствие кода (необходимо, если действие должно произойти не зависимо от состояния сервера).
    CONNECT_TO_SERVER_IN_PROGRESS = 100, ///< В процессе подключения к серверу.
    CONNECT_TO_SERVER_COMPLETE = 200, ///< Подключение к серверу завершено.
    CONNECT_TO_LOBBY_IN_PROGRESS = 101, ///< В процессе подключения к лобби.
    CONNECT_TO_LOBBY_COMPLETE = 201, ///< Подключение к лобби завершено.
    CONNECT_TO_ROOM_IN_PROGRESS = 102, ///< В процессе подключения к комнате.
    CONNECT_TO_ROOM_COMPLETE = 202, ///< Подключение к комнате завершено.
    CONNECT_TO_ROOM_FAILD = 402, ///< Не удалось подключиться к комнате.
    PLAYER_ENTER_THE_ROOM = 203, ///< К комнате подключился новый игрок.
    DISCONNECT_FROM_SERVER_IN_PROGRESS = 104, ///< В процессе отключения от сервера.
    DISCONNECT_FROM_SERVER_COMPLETE = 204 ///< Отключение от сервера завершено.
}

/**
 ### Класс, отвечающий за взаимодействие с сервером Photon

Данный класс отвечает за:
 - Подключение к серверу;
 - Отключение от сервера;
 - Создание и подключение к лобби;
 - Создание и подключение к комнате;
 - Отключение от комнаты;
 - Отключение от лобби;
 - Отключение от сервера;
 - Спавн игрока на сервере.

@param vrLogger VRLoggersManager для вывода логов внутри игры.
@param sceneChanger SceneChanger для перехода между сценами.
@param playersPrefabName string название prefab-а игрока лежащего в Assets/Resources. 
Данный prefab будет заспавнен в комнате на сервере при подключении.
@param defaultRooms Список RoomSettings комнат, к которым будет производиться подключение.
@param autoStartTestRoom bool Параметр для отладки. 
Если true, то автоматически подключает в первую комнату при запуске приложения.
 */
public class NetworkManager : MonoBehaviourPunCallbacks
{
    /// Событие сервера.
    public event UnityAction<NetworkCode> NetworConnectionEvent;

    [SerializeField] private VRLoggersManager _vrLogger;
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private string _playersPrefabName;
    [SerializeField] private List<RoomSettings> _defaultRooms;
    [SerializeField] private bool _autoStartTestRoom;

    private bool _quitFromApplication;
    private bool _isConnectedToServer;
    private GameObject _spawnedPlayerPrefab;

    private void Start()
    {
        _quitFromApplication = false;
        _isConnectedToServer = false;
        _vrLogger.SetNetworkManager(this);
        if (_autoStartTestRoom)
        {
            ConnectToServer();
        }
    }

    /// Осуществить подключение к серверу.
    public void ConnectToServer()
    {
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_IN_PROGRESS);
        _vrLogger.Log("[" + this.name + "] Conecting to server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    /// Метод, выполняемый при подключении к серверу.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_COMPLETE);
        _isConnectedToServer = true;
        _vrLogger.Log("[" + this.name + "] Connected to master server.");
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_LOBBY_IN_PROGRESS);
        // PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    /**
    Создание/подключение к комнате.

    Если комнаты не существует - она будет создана. 
    Иначе произойдет подключение к существующей комнате.
    @param [in] roomIndex Индекс комнаты в defaultRooms, к которой мы хотим подключиться.
     */
    public void InitRoom(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < _defaultRooms.Count)
        {
            //StartCoroutine(FadeAndChangeScene(roomIndex));
            NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_IN_PROGRESS);
            RoomSettings defaultRoom = _defaultRooms[roomIndex];

            PhotonNetwork.LoadLevel(defaultRoom.sceneID);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = defaultRoom.playersInRoom;
            roomOptions.IsVisible = defaultRoom.isRoomVisible;
            roomOptions.IsOpen = true;
            PhotonNetwork.JoinOrCreateRoom(defaultRoom.name, roomOptions, TypedLobby.Default);
        }
        else
        {
            _vrLogger.Log("[" + this.name + "] Room index " + roomIndex + " is not correct.");
        }
    }

    /// Покинуть текущую комнату
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /**
    Отключиться от сервера.
    @param [in] quitFromApplication Если true, то после отключения от сервера произойдет выход из приложения.
     */
    public void DisconnectedFromServer(bool quitFromApplication = false)
    {
        _vrLogger.Log("[" + this.name + "] Disconnecting from server.");
        NetworConnectionEvent?.Invoke(NetworkCode.DISCONNECT_FROM_SERVER_IN_PROGRESS);
        _quitFromApplication = quitFromApplication;
        if (quitFromApplication && !_isConnectedToServer)
        {
            _sceneChanger.ExitFromApplication();
        }
        PhotonNetwork.Disconnect();
    }

    /// Метод, выполняемый при подключении к комнате.
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_COMPLETE);
        _vrLogger.Log("[" + this.name + "] You are join to the room.");
        SpawnPlayerPrefab();
        NetworkVariables.SendPropertyToServer(PlayersProperty.UPDATE_STATUS, "Update");
    }

    /// Метод, выполняемый при отключении от комнаты.
    public override void OnLeftRoom()
    {
        string destroyedPlayerName = "#destroyed#";
        if (_spawnedPlayerPrefab != null)
        {
            destroyedPlayerName = _spawnedPlayerPrefab.name;
        }
        base.OnLeftRoom();
        PhotonNetwork.Destroy(_spawnedPlayerPrefab);
        _vrLogger.Log("[" + this.name + "] Player " + destroyedPlayerName + " is destroy");
        _sceneChanger.LoadStartScene();
    }

    /** 
    Метод, выполняемый при отключении от сервера.
    @param cause Причина отключения от сервера.
     */
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        _isConnectedToServer = false;
        NetworConnectionEvent?.Invoke(NetworkCode.DISCONNECT_FROM_SERVER_COMPLETE);
        _vrLogger.Log("[" + this.name + "] You was disconnected from server.");
        if (_quitFromApplication)
        {
            _sceneChanger.ExitFromApplication();
        }
    }

    /// Метод, выполняемый при подключении к лобби.
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_LOBBY_COMPLETE);
        _vrLogger.Log("[" + this.name + "] Some user is join to the lobby.");
        if (_autoStartTestRoom)
        {
            _autoStartTestRoom = false;
            if (_defaultRooms.Count > 0)
            {
                InitRoom(0);
            }
            else
            {
                _vrLogger.Log("[" + this.name + "] No rooms to connect.");
            }
        }
    }

    /** 
    Метод выполняемый, когда другой игрок подключился к комнате.
    @param newPlayer Данные подключившегося игрока.
     */
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _vrLogger.Log("[" + this.name + "] " +
           newPlayer.NickName == null || newPlayer.NickName == ""
            ? "Some unknown user"
            : newPlayer.NickName
            + "is join to the room.");
    }

    private void SpawnPlayerPrefab()
    {
        Vector3 playerPosition = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10));
        _spawnedPlayerPrefab = PhotonNetwork.Instantiate(_playersPrefabName, playerPosition, transform.rotation);
        _vrLogger.Log("[" + this.name + "] Player " + _spawnedPlayerPrefab.name + " spawned at position(" + playerPosition.x + "," + playerPosition.y + "," + playerPosition.z + ")");
    }
}
