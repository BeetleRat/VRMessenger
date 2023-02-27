using System.Collections.Generic;
using UnityEngine;
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

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _serversUI;
    [SerializeField] private List<RoomSettings> _defaultRooms;
    [SerializeField] private VRLogger _vrLogger;
    [SerializeField] private byte _playersInRoom;

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        _vrLogger.Log("Conecting to server...");
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        _vrLogger.Log("Connected to master server.");
        PhotonNetwork.JoinLobby();
    }

    public void InitRoom(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < _defaultRooms.Count)
        {
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _vrLogger.Log("Some user is join to the room.");
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
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
