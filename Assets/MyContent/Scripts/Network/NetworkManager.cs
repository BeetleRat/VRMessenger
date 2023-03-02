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
    PLAYER_ENTER_THE_ROOM = 203

}
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance = null;

    public event UnityAction<NetworkCode> NetworConnectionEvent;

    [SerializeField] private string _playersPrefabName;
    [SerializeField] private List<RoomSettings> _defaultRooms;
    [SerializeField] private bool _autoStartTestRoom;

    private List<VRLogger> _vrLoggers;
    private GameObject _spawnedPlayerPrefab;

    private void Start()
    {
        SingleToneOnStart();
        RefreshVrLogger();
        if (_autoStartTestRoom)
        {
            ConnectToServer();
        }
    }

    public void ConnectToServer()
    {
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_IN_PROGRESS);
        PhotonNetwork.ConnectUsingSettings();
        NetworkLog("Conecting to server...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        RefreshVrLogger();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_COMPLETE);
        NetworkLog("Connected to master server.");
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
            NetworkLog("Room index " + roomIndex + " is not correct.");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        RefreshVrLogger();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_COMPLETE);
        NetworkLog("You are join to the room.");
        SpawnPlayerPrefab();
    }

    public override void OnLeftRoom()
    {
        string destroyedPlayerName = _spawnedPlayerPrefab.name;
        base.OnLeftRoom();
        PhotonNetwork.Destroy(_spawnedPlayerPrefab);
        NetworkLog("Player " + destroyedPlayerName + " is destroy");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_LOBBY_COMPLETE);
        NetworkLog("Some user is join to the lobby.");
        if (_autoStartTestRoom)
        {
            _autoStartTestRoom = false;
            if (_defaultRooms.Count > 0)
            {
                InitRoom(0);
            }
            else
            {
                NetworkLog("No rooms to connect.");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        NetworkLog(
           newPlayer.NickName == null || newPlayer.NickName == ""
            ? "Some unknown user"
            : newPlayer.NickName
            + "is join to the room.");
    }

    private void SingleToneOnStart()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void RefreshVrLogger()
    {
        if (_vrLoggers == null)
        {
            _vrLoggers = new List<VRLogger>();
        }
        List<string> oldLines = new List<string>();
        if (_vrLoggers.Count > 0)
        {
            oldLines.AddRange(_vrLoggers[0].GetLoggerLines());
            foreach (VRLogger vrLogge in _vrLoggers)
            {
                vrLogge.ClearLog();
            }
        }
        _vrLoggers.Clear();
        GameObject[] vrLoggerObjects = GameObject.FindGameObjectsWithTag("VRLogger");
        if (vrLoggerObjects != null)
        {
            Debug.Log("Найдено vrLoggerObjects: " + vrLoggerObjects.Length);
            foreach (GameObject vrLoggerObject in vrLoggerObjects)
            {
                if (vrLoggerObject.TryGetComponent(out VRLogger vrLogger))
                {
                    Debug.Log("Получен компонент " + _vrLoggers.Count + ": " + vrLogger);
                    vrLogger.SetLoggerLines(oldLines);
                    _vrLoggers.Add(vrLogger);
                }
            }
        }
        else
        {
            Debug.LogWarning("No VRLogger found in scene.");
        }
    }

    private void SpawnPlayerPrefab()
    {
        Vector3 playerPosition = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10));
        _spawnedPlayerPrefab = PhotonNetwork.Instantiate(_playersPrefabName, playerPosition, transform.rotation);
        NetworkLog("Player " + _spawnedPlayerPrefab.name + " spawned at position(" + playerPosition.x + "," + playerPosition.y + "," + playerPosition.z + ")");
    }

    private void NetworkLog(string log)
    {
        if (_vrLoggers != null)
        {
            foreach (VRLogger vrLogger in _vrLoggers)
            {
                vrLogger.Log(log);
            }
        }
        else
        {
            Debug.LogWarning("No vrLogger found in scene.");
            Debug.Log(log);
        }
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
