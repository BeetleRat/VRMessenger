using UnityEngine;
using TMPro;
using Photon.Pun;

/**
 Класс, изменяющий имя игрока в сети на значение, записанное в поле ввода.

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkManager;

@param inputText Поле ввода, в котором записывается новое имя игрока в сети.
@param catcher ComponentCatcher данной сцены.
@see ComponentCatcher; NetworkManager
 */
public class ChangeNetworkName : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputText;
    [SerializeField] private ComponentCatcher _catcher;

    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = _catcher.GetNetworkManager();
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent += OnNetworConnection;
        }
        _inputText.enabled = false;
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
    }

    /// Метод изменяющий имя игрока в сети на то, что записано в inputText.
    public void OnUsernameInputFieldChanged()
    {
        PhotonNetwork.NickName = _inputText.text;
    }

    private void OnNetworConnection(NetworkCode code)
    {
        switch (code)
        {
            case NetworkCode.CONNECT_TO_LOBBY_COMPLETE:
                _inputText.enabled = true;
                _inputText.text = PhotonNetwork.NickName;
                break;
            case NetworkCode.DISCONNECT_FROM_SERVER_COMPLETE:
                _inputText.enabled = false;                
                break;
        }
    }
}
