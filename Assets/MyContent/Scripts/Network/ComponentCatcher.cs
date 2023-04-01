using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
class CatchableComponents
{
    public bool GesureAnimator;
    public bool NetworkVariables;
}

public class ComponentCatcher : MonoBehaviour
{
    // Словарь типов классов
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
            { typeof(GestureAnimation), "GestureAnimation" },
            { typeof(NetworkVariables), "NetworkVariables" },

            { typeof(object), "object" }
       };

    [SerializeField] CatchableComponents _catchableComponents;
    private VRLoggersManager _vrLogger;
    private NetworkManager _networkManager;
    private ControllerEvents _controllerEvents;
    private GestureAnimation _gestureAnimator;
    private NetworkVariables _networkVariables;


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
                Debug.LogWarning("[" + this.name + "] Не удалось поймать VRLoggersManager");
            }
        }
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            CheckComponentState(_networkManager);
        }
        if (_controllerEvents == null)
        {
            _controllerEvents = FindObjectOfType<ControllerEvents>();
            CheckComponentState(_controllerEvents);
        }

        if (_catchableComponents.GesureAnimator && _gestureAnimator == null)
        {
            _gestureAnimator = FindObjectOfType<GestureAnimation>();
            CheckComponentState(_gestureAnimator);
        }

        if (_catchableComponents.GesureAnimator && _networkVariables == null)
        {
            _networkVariables = FindObjectOfType<NetworkVariables>();
            CheckComponentState(_networkVariables);
        }

    }

    private void CheckComponentState<T>(T component)
    {
        if (component == null)
        {
            _vrLogger?.Log("[" + this.name + "] Не удалось поймать " + typeToString[typeof(T)]);
            Debug.LogWarning("[" + this.name + "] Не удалось поймать " + typeToString[typeof(T)]);
        }
    }

    private void TryToGetComponent<T>(ref T fild)
    {
        if (fild == null)
        {
            RefreshComponents();
            if (fild == null)
            {
                _vrLogger?.Log("[" + this.name + "] Catcher не содержит " + typeToString[typeof(T)]);
                Debug.LogWarning("[" + this.name + "] Catcher не содержит " + typeToString[typeof(T)]);
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

    public GestureAnimation GetGestureAnimator()
    {
        TryToGetComponent(ref _gestureAnimator);

        return _gestureAnimator;
    }

    public NetworkVariables GetNetworkVariables()
    {
        TryToGetComponent(ref _networkVariables);

        return _networkVariables;
    }

    public VRLoggersManager GetVRLoggersManager()
    {
        TryToGetComponent(ref _vrLogger);

        return _vrLogger;
    }

}
