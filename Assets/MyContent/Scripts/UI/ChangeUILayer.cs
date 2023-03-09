using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Codes
{
    CONNECT_TO_SERVER_IN_PROGRESS,
    CONNECT_TO_SERVER_COMPLETE,

}

[System.Serializable]
public class ChangeLayers
{
    public NetworkCode Code;
    public CanvasGroup Layer;
}

public class ChangeUILayer : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    [SerializeField] private CanvasGroup _startLayer;
    [SerializeField] private List<ChangeLayers> _layers;
    [Range(1, 120)]
    [SerializeField] private float _fadeDuration;
    [Range(0.00001f, 0.1f)]
    [SerializeField] private float _fadeSmoothness;


    private NetworkManager _networkManager;
    private Queue<ChangeLayers> _changeQueue;
    private float _currentFrame;
    private CanvasGroup _currentLayer;
    

    private void Awake()
    {
        _changeQueue = new Queue<ChangeLayers>();
        _currentFrame = 0;        
    }
    private void Start()
    {
        _networkManager = _catcher.GetNetworkManager();
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent += OnNetworConnection;
        }
        
        if (_layers.Count > 0)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].Layer.alpha = 0;
                _layers[i].Layer.gameObject.SetActive(false);
            }
        }
        _startLayer.gameObject.SetActive(true);
        _startLayer.alpha = 1;
        _currentLayer = _startLayer;
    }

    private void Update()
    {
        _currentFrame += Time.deltaTime * 10;
        if (_currentFrame / _fadeDuration > 1)
        {
            _currentFrame = 0;
            if (_changeQueue.Count > 0)
            {
                ChangeLayers changeLayer = _changeQueue.Peek();
                if (changeLayer.Layer == _currentLayer)
                {
                    _changeQueue.Dequeue();
                }
                else
                {
                    changeLayer.Layer.gameObject.SetActive(true);
                    if (_currentLayer.alpha == 0 && changeLayer.Layer.alpha == 1)
                    {
                        _currentLayer.gameObject.SetActive(false);
                        _currentLayer = changeLayer.Layer;
                        _changeQueue.Dequeue();
                    }
                    else
                    {
                        _currentLayer.alpha = Mathf.Max(0, _currentLayer.alpha - _fadeSmoothness);
                        changeLayer.Layer.alpha = Mathf.Min(1, changeLayer.Layer.alpha + _fadeSmoothness);
                    }
                }
            }
        }

    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
    }

    private void OnNetworConnection(NetworkCode code)
    {
        foreach (ChangeLayers layer in _layers)
        {
            if (layer.Code == code)
            {
                _changeQueue.Enqueue(layer);
                break;
            }
        }
    }
}
