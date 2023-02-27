using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private VRLogger _vrLogger;
    [SerializeField] private string _playersPrefabName;

    private GameObject _spawnedPlayerPrefab;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Vector3 playerPosition = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10));
        _spawnedPlayerPrefab = PhotonNetwork.Instantiate(_playersPrefabName, playerPosition, transform.rotation);
        _vrLogger.Log("Player "+ _spawnedPlayerPrefab.name+" spawned at position("+ playerPosition.x+","+playerPosition.y+","+playerPosition.z+")");
    }

    public override void OnLeftRoom()
    {
        string destroyedPlayerName = _spawnedPlayerPrefab.name;
        base.OnLeftRoom();        
        PhotonNetwork.Destroy(_spawnedPlayerPrefab);
        _vrLogger.Log("Player " + destroyedPlayerName + " is destroy");
    }
}
