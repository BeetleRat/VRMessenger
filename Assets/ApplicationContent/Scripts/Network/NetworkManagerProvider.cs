using UnityEngine;

/**
Класс, обеспечивающий взаимодействие UI с NetworkManager.

Взаимодействуя с ComponentCatcher, данный класс предоставляет элементам UI взаимодействовать с NetworkManager.

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- NetworkManager;

@param catcher ComponentCatcher, который используется для получения доступа к NetworkManager.
@see ComponentCatcher; NetworkManager
 */
public class NetworkManagerProvider : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;

    private NetworkManager _networkManager;

    private void Start()
    {
        if (_catcher != null)
        {
            _networkManager = _catcher.GetNetworkManager();
        }
    }

    /// Подключиться к серверу.
    public void ConnectToServer()
    {
        _networkManager?.ConnectToServer();
    }

    /// Покинуть комнату.
    public void LeaveRoom()
    {
        _networkManager?.LeaveRoom();
    }

    /**
    Создание/подключение к комнате нулевой комнате.

    Если комнаты не существует - она будет создана. 
    Иначе произойдет подключение к существующей комнате.
    @param [in] roomIndex Индекс комнаты в defaultRooms, к которой мы хотим подключиться.
     */
    public void InitDefaultRoom(int roomIndex)
    {
        _networkManager?.InitRoom(roomIndex, 0);
    }

    /// Отключиться от сервера.
    public void DisconnectedFromServer()
    {
        _networkManager?.DisconnectedFromServer();
    }

    /**
    Выйти из приложения.
    @note Данный метод завершает только собранное приложение. 
    В режиме отладки в консоль будет выведен Warning о завершении, 
    но отладка продолжится
     */
    public void ExitFromApplication()
    {
        _networkManager?.DisconnectedFromServer(true);
    }
}
