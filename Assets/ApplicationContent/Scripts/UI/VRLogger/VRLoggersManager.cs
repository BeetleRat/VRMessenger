using System.Collections.Generic;
using UnityEngine;

/**
Скрипт, отвечающий за вывод логов во все VRLogger на сцене

@attention Для корректной работы vrLogger-а необходимо перед началом работы установить ему NetworkManager, 
воспользовавшись методом SetNetworkManager(NetworkManager networkManager).
@see NetworkManager; VRLogger
 */
public class VRLoggersManager : MonoBehaviour
{
    private List<VRLogger> _vrLoggers;
    private NetworkManager _networkManager;

    private void Awake()
    {
        RefreshVrLogger();
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
    }

    private void RefreshVrLogger()
    {
        if (_vrLoggers == null)
        {
            _vrLoggers = new List<VRLogger>();
        }
        List<string> oldLines = new List<string>();
        if (_vrLoggers.Count > 0)
        {
            oldLines.AddRange(_vrLoggers[0].GetLoggerLines());
            foreach (VRLogger vrLogge in _vrLoggers)
            {
                vrLogge.ClearLog();
            }
        }
        _vrLoggers.Clear();
        VRLogger[] vrLoggers = FindObjectsByType<VRLogger>(FindObjectsSortMode.None);
        if (vrLoggers.Length > 0)
        {
            foreach (VRLogger vrLogger in vrLoggers)
            {
                vrLogger.SetLoggerLines(oldLines);
                _vrLoggers.Add(vrLogger);

            }
        }
        else
        {
            Debug.LogWarning("No VRLogger found in scene.");
        }
    }

    /**
     Метод для вывода лога.
    @param [in] log string текст выводимого лога.
     */
    public void Log(string log)
    {
        if (_vrLoggers != null)
        {
            foreach (VRLogger vrLogger in _vrLoggers)
            {
                vrLogger.Log(log);
            }
        }
        else
        {
            Debug.LogWarning("No vrLogger found in scene.");
            Debug.Log(log);
        }
    }

    /**
     Сеттер NetworkManager-а. 
    @attention Для корректной работы vrLogger-а необходимо перед началом работы установить ему NetworkManager.
    @param [in] networkManager NetworkManager.
     */
    public void SetNetworkManager(NetworkManager networkManager)
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
        this._networkManager = networkManager;
        networkManager.NetworConnectionEvent += OnNetworConnection;
    }

    private void OnNetworConnection(NetworkCode code)
    {
        // Если произошло завершение какого-то подключения
        // коды завершений 2**
        if (((int)code) / 100 == 2)
        {
            RefreshVrLogger();
        }
    }
}
