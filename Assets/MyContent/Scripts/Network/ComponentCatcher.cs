using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
class CatchableComponents
{

}

public class ComponentCatcher : MonoBehaviour
{
    // ������� ����� �������
    private static readonly Dictionary<System.Type, string> typeToString =
       new Dictionary<System.Type, string>
       {
            { typeof(string), "string" },            
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(short), "short" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(sbyte), "sbyte" },
            { typeof(float), "float" },
            { typeof(ushort), "ushort" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(void), "void" },
            { typeof(VRLoggersManager), "VRLoggersManager" },
            { typeof(NetworkManager), "NetworkManager" },
            { typeof(ControllerEvents), "ControllerEvents" },

            { typeof(object), "object" }
       };

    [SerializeField] CatchableComponents _catchableComponents;
    private VRLoggersManager _vrLogger;
    private NetworkManager _networkManager;
    private ControllerEvents _controllerEvents;


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
                Debug.LogWarning("[" + this.name + "] �� ������� ������� VRLoggersManager");
            }
        }
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            if (_networkManager == null)
            {
                _vrLogger?.Log("[" + this.name + "] �� ������� ������� NetworkManager");
                Debug.LogWarning("[" + this.name + "] �� ������� ������� NetworkManager");
            }
        }
        if (_controllerEvents == null)
        {
            _controllerEvents= FindObjectOfType<ControllerEvents>();
            if (_controllerEvents == null)
            {
                _vrLogger?.Log("[" + this.name + "] �� ������� ������� ControllerEvents");
                Debug.LogWarning("[" + this.name + "] �� ������� ������� ControllerEvents");
            }
        }

    }

    private void TryToGetComponent<T>(ref T fild)
    {
        if (fild == null)
        {
            RefreshComponents();
            if (fild == null)
            {
                _vrLogger?.Log("[" + this.name + "] Catcher �� �������� " + typeToString[typeof(T)]);
                Debug.LogWarning("[" + this.name + "] Catcher �� �������� " + typeToString[typeof(T)]);
            }
        }
    }

    public NetworkManager GetNetworkManager()
    {
        TryToGetComponent(ref _networkManager);

        return _networkManager;
    }

    public ControllerEvents GetControllerEvents()
    {
        TryToGetComponent(ref _controllerEvents);

        return _controllerEvents;
    }

    public VRLoggersManager GetVRLoggersManager()
    {
        TryToGetComponent(ref _vrLogger);

        return _vrLogger;
    }

}
