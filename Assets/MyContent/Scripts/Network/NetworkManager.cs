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
    public event UnityAction<NetworkCode> NetworConnectionEvent;
    [SerializeField] private List<RoomSettings> _defaultRooms;
    [SerializeField] private VRLogger _vrLogger;
    [SerializeField] private byte _playersInRoom;

    public void ConnectToServer()
    {
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_IN_PROGRESS);
        PhotonNetwork.ConnectUsingSettings();
        _vrLogger.Log("Conecting to server...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_SERVER_COMPLETE);
        _vrLogger.Log("Connected to master server.");
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
            _vrLogger.Log("Room index " + roomIndex + " is not correct.");
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_ROOM_COMPLETE);
        _vrLogger.Log("Some user is join to the room.");
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        NetworConnectionEvent?.Invoke(NetworkCode.CONNECT_TO_LOBBY_COMPLETE);
        _vrLogger.Log("Some user is join to the lobby.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _vrLogger.Log(
           newPlayer.NickName == null || newPlayer.NickName == ""
            ? "Some unknown user"
            : newPlayer.NickName
            + "is join to the room.");
    }
}
