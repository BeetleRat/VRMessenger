using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerProvider : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    private NetworkManager _networkManager;
    private void Start()
    {
        _networkManager = _catcher.GetNetworkManager();
    }

    public void ConnectToServer()
    {
        _networkManager?.ConnectToServer();
    }
    public void LeaveRoom()
    {
        _networkManager?.LeaveRoom();
    }

    public void InitRoom(int roomIndex)
    {
        _networkManager?.InitRoom(roomIndex);
    }

    public void DisconnectedFromServer()
    {
        _networkManager?.DisconnectedFromServer();
    }

    public void ExitFromApplication()
    {
        _networkManager?.DisconnectedFromServer(true);
    }
}
