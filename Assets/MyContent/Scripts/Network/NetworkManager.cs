using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private VRLogger _vrLogger;
    [SerializeField] private string _roomName;
    [SerializeField] private byte _playersInRoom;
    [SerializeField] private bool _isRoomVisible;
    private void Start()
    {       
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        _vrLogger.Log("Conecting to server...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        _vrLogger.Log("Connected to master server.");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _playersInRoom;
        roomOptions.IsVisible = _isRoomVisible;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _vrLogger.Log("Some user is join to the room.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _vrLogger.Log(
           newPlayer.NickName == null
            ? "Some unknown user"
            : newPlayer.NickName 
            +"is join to the room.");
    }
}
