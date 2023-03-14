using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class RoomSettings
{
    public string name;
    public int sceneID;
    public byte playersInRoom;
    public bool isRoomVisible;
}

public enum NetworkCode
{
    CONNECT_TO_SERVER_IN_PROGRESS = 100,
    CONNECT_TO_SERVER_COMPLETE = 200,
    CONNECT_TO_LOBBY_IN_PROGRESS = 101,
    CONNECT_TO_LOBBY_COMPLETE = 201,
    CONNECT_TO_ROOM_IN_PROGRESS = 102,
    CONNECT_TO_ROOM_COMPLETE = 202,
    CONNECT_TO_ROOM_FAILD = 402,
    PLAYER_ENTER_THE_ROOM = 203,
    DISCONNECT_FROM_SERVER_IN_PROGRESS = 104,
    DISCONNECT_FROM_SERVER_COMPLETE = 204

}
public class NetworkManager : MonoBehaviourPunCallbacks
{
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

    public void ConnectToServer()
    {
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_IN_PROGRESS);
        _vrLogger.Log("[" + this.name + "] Conecting to server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_COMPLETE);
        _isConnectedToServer = true;
        _vrLogger.Log("[" + this.name + "] Connected to master server.");
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_LOBBY_IN_PROGRESS);
        PhotonNetwork.JoinLobby();
    }

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

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_COMPLETE);
        _vrLogger.Log("[" + this.name + "] You are join to the room.");
        SpawnPlayerPrefab();
    }

    public override void OnLeftRoom()
    {
        string destroyedPlayerName = _spawnedPlayerPrefab.name;
        base.OnLeftRoom();
        PhotonNetwork.Destroy(_spawnedPlayerPrefab);
        _vrLogger.Log("[" + this.name + "] Player " + destroyedPlayerName + " is destroy");
        _sceneChanger.LoadStartScene();
    }

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _vrLogger.Log("[" + this.name + "] "+
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

    //private IEnumerator<int> FadeAndChangeScene(int roomIndex)
    //{
    //    bool isFaded = true;
    //    //Fader.instance.FadeIn(() => isFaded = true);
    //    while (!isFaded)
    //    {
    //        yield return -1;
    //    }
    //    NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_IN_PROGRESS);
    //    RoomSettings defaultRoom = _defaultRooms[roomIndex];

    //    PhotonNetwork.LoadLevel(defaultRoom.sceneID);

    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = defaultRoom.playersInRoom;
    //    roomOptions.IsVisible = defaultRoom.isRoomVisible;
    //    roomOptions.IsOpen = true;
    //    PhotonNetwork.JoinOrCreateRoom(defaultRoom.name, roomOptions, TypedLobby.Default);
    //    //Fader.instance.FadeOut(() => isFaded = false);
    //}
}
