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
    public CanvasGroup FromLayer;
    public CanvasGroup ToLayer;
    public bool IsFromActive;
}

public class ChangeUILayer : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private List<ChangeLayers> _layers;
    [Range(1, 120)]
    [SerializeField] private float _fadeDuration;
    [Range(0.00001f, 0.1f)]
    [SerializeField] private float _fadeSmoothness;



    private Queue<ChangeLayers> _changeQueue;
    private float _currentFrame;

    private void Awake()
    {
        _changeQueue = new Queue<ChangeLayers>();
        _currentFrame = 0;
    }
    private void Start()
    {
        _networkManager.NetworConnectionEvent += OnNetworConnection;
        if (_layers.Count > 0)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].FromLayer.gameObject.SetActive(_layers[i].IsFromActive);
                _layers[i].ToLayer.alpha = 0;
                _layers[i].ToLayer.gameObject.SetActive(false);
            }
        }
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
                changeLayer.ToLayer.gameObject.SetActive(true);
                if (changeLayer.FromLayer.alpha == 0 && changeLayer.ToLayer.alpha == 1)
                {
                    changeLayer.FromLayer.gameObject.SetActive(false);
                    _changeQueue.Dequeue();
                }
                else
                {
                    changeLayer.FromLayer.alpha = Mathf.Max(0, changeLayer.FromLayer.alpha - _fadeSmoothness);
                    changeLayer.ToLayer.alpha = Mathf.Min(1, changeLayer.ToLayer.alpha + _fadeSmoothness);
                }
            }
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
