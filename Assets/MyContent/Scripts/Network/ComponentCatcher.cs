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
                Debug.LogWarning("�� ������� ������� VRLoggersManager");
            }
        }
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            if (_networkManager == null)
            {
                _vrLogger?.Log("�� ������� ������� NetworkManager");
                Debug.LogWarning("�� ������� ������� NetworkManager");
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
                _vrLogger?.Log("Catcher �� �������� NetworkManager");
                Debug.LogWarning("Catcher �� �������� NetworkManager");
            }
        }
        return _networkManager;
    }


}
