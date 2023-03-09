using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCatcher : MonoBehaviour
{
    private VRLoggersManager _vrLogger;
    private NetworkManager _networkManager;
    private void Start()
    {
        RefreshComponents();
    }
    private void RefreshComponents()
    {
        if (_vrLogger == null)
        {
            _vrLogger = FindObjectOfType<VRLoggersManager>();
            if (_vrLogger == null)
            {
                Debug.LogWarning("Не удалось поймать VRLoggersManager");
            }
        }
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            if (_networkManager == null)
            {
                _vrLogger?.Log("Не удалось поймать NetworkManager");
                Debug.LogWarning("Не удалось поймать NetworkManager");
            }
        }
    }
    public NetworkManager GetNetworkManager()
    {
        if (_networkManager == null)
        {
            RefreshComponents();
            if (_networkManager == null)
            {
                _vrLogger?.Log("Catcher не содержит NetworkManager");
                Debug.LogWarning("Catcher не содержит NetworkManager");
            }
        }
        return _networkManager;
    }


}
