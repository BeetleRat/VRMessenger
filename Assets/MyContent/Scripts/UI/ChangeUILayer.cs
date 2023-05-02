using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Класс связывающий код состояния сервера и отображаемый в этом состоянии слой.
[System.Serializable]
public class ChangeLayers
{
    /// Код состояния сервера.
    public NetworkCode Code;
    /// Отображаемый Canvas при поступлении данного кода от сервера.
    public CanvasGroup Layer;
}

/**
 ### Класс отвечающий за плавную смену слоев на UI экране

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- NetworkManager;

@param catcher ComponentCatcher находящийся на данной сцене;
@param startLayer Canvas отображаемый изначально;
@param layers Список ChangeLayers, которые будут меняться в соответствии с указанными кодами;
@param fadeDuration Продолжительность затухания слоя;
@param fadeSmoothness Плавность затухания слоя;
 */
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

    /**
     Метод для произвольной смены слоя, не зависимо от состояния сервера.
    @param layer Canvas, который мы хотим отобразить. Данный Canvas должен быть указан в layers.
     */
    public void ChangeLayer(CanvasGroup layer)
    {
        foreach (ChangeLayers changeLayer in _layers)
        {
            if (changeLayer.Layer == layer)
            {
                _changeQueue.Enqueue(changeLayer);
                break;
            }
        }
    }
}
