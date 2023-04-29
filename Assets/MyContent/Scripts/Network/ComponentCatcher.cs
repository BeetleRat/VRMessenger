using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
### Класс, отлавливающий определенные компоненты в текущей сцене

@attention Данный класс не должен быть Singletone. 
Для каждой отдельной сцены должен быть свой экземпляр данного класса, если он в ней нужен.

@attention В сцене достаточно одного экземпляра такого класса.

Данный класс находит в сцене указанные компоненты. 
Это компоненты, запрашиваемые другими скриптами в ходе работы приложения. 
Так же компоненты пришедшие из других сцен.

Логика работы следующая. При старте сцены ComponentCatcher отлавливает все важные компоненты. 
В дальнейшем, если какому-то скрипту понадобиться один из этих компонентов он будет обращаться к ComponentCatcher.

Локальные компоненты запрашивают данный класс через свои свойства. 
А компоненты спавнемые сервером будут искать ComponentCatcher в сцене.

ComponentCatcher безусловно отлавливает компоненты:
 - VRLoggersManager;
 - NetworkManager;
 - ControllerEvents;
@param catchGestureAnimation Отлавливать ли в текущей сцене GestureAnimation.
@param catchNetworkVariables Отлавливать ли в текущей сцене NetworkVariables.
 */
public class ComponentCatcher : MonoBehaviour
{
    /// Словарь типов классов
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

    [Header("Catchable Components")]
    [SerializeField] private bool _catchGestureAnimation;
    [SerializeField] private bool _catchNetworkVariables;

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

        if (_catchGestureAnimation && _gestureAnimator == null)
        {
            _gestureAnimator = FindObjectOfType<GestureAnimation>();
            CheckComponentState(_gestureAnimator);
        }

        if (_catchNetworkVariables && _networkVariables == null)
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

    /**
     Геттер NetworkManager
    @return NetworkManager, если он был найден в сцене.
     */
    public NetworkManager GetNetworkManager()
    {
        TryToGetComponent(ref _networkManager);

        return _networkManager;
    }

    /**
     Геттер ControllerEvents
    @return ControllerEvents, если он был найден в сцене.
     */
    public ControllerEvents GetControllerEvents()
    {
        TryToGetComponent(ref _controllerEvents);

        return _controllerEvents;
    }

    /**
     Геттер GestureAnimation
    @return GestureAnimation, если он был найден в сцене.
     */
    public GestureAnimation GetGestureAnimator()
    {
        TryToGetComponent(ref _gestureAnimator);

        return _gestureAnimator;
    }

    /**
     Геттер NetworkVariables
    @return NetworkVariables, если он был найден в сцене.
     */
    public NetworkVariables GetNetworkVariables()
    {
        TryToGetComponent(ref _networkVariables);

        return _networkVariables;
    }

    /**
     Геттер VRLoggersManager
    @return VRLoggersManager, если он был найден в сцене.
     */
    public VRLoggersManager GetVRLoggersManager()
    {
        TryToGetComponent(ref _vrLogger);

        return _vrLogger;
    }

}
