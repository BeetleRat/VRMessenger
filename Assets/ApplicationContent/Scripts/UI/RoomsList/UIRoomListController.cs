using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
Класс, контролирующий все элементы списка комнат, к которым можно подключиться.

Данный класс хранит информацию о всех существующий на данный момент комнатах.
Он отображает эту информацию через элементы списка комнат, к которым можно подключиться.

Так же данный класс связывается с NetworkManager для получения информации о текущих комнатах.
И отправляет запрос на подключение к выбранной комнате.

Данному классу, через метод SetRoomType(), передается тип комнаты. 
После чего контроллер выведет на экран список из элементов RoomListItem, для существующий комнат данного типа.
При выборе одного из элементов списка, можно подключиться к указанной в нем комнате, при помощи метода ConnectToSelectedRoom().
Так же данный класс создает новые комнаты с уникальным именем для комнат данного типа: имя_комнаты + ID_в_списке, через метод CreateNewRoom().

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- NetworkManager;

@param catcher ComponentCatcher данной сцены.
@param roomListContent UI слой, в который будут отображаться элементы списка.
@param roomListItemPrefab Prefab элемента списка. Prefab должен содержать компонент RoomListItem.
@param connectToRoomButton Кнопка подключения к комнате, которая сейчас выбрана в списке.
@param defaultButtonColor Цвет не выбранного элемента списка.
@param selectedButtonColor Цвет выбранного элемента списка.
@see ComponentCatcher; NetworkManager; RoomListItem
 */
public class UIRoomListController : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    [SerializeField] private Transform _roomListContent;
    [SerializeField] private GameObject _roomListItemPrefab;
    [SerializeField] private GameObject _connectToRoomButton;
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _selectedButtonColor;

    private NetworkManager _networkManager;

    private int _roomType;
    private List<RoomInfo> _rooms;
    private List<RoomListItem> _roomButtons;
    private RoomListItem _currentLoadRoom;

    private void Start()
    {
        _networkManager = _catcher.GetNetworkManager();
        if (_networkManager)
        {
            _networkManager.RoomListUpdate += OnRoomListUpdate;
        }
        _connectToRoomButton.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (_networkManager)
        {
            _networkManager.RoomListUpdate -= OnRoomListUpdate;
        }
    }

    /**
     Метод установки типа комнаты.

    @param [in] roomType Индекс типа комнаты в поле defaultRooms компонента NetworkManager.
     */
    public void SetRoomType(int roomType)
    {
        _roomType = roomType;
        UpdateUI();
    }

    /**
     Метод изменения текущего выбранного компонента списка.

    Данный метод вызовет элемент списка, когда на него кликнут и в качестве входного параметра он передаст себя.
    @param [in] selectedButton Новый выбранный элемент списка.
     */
    public void ChangeCurrentLoadRoom(GameObject selectedButton)
    {
        foreach (Transform trans in _roomListContent)
        {
            Image buttonImage = trans.gameObject.GetComponent<Image>();
            if (buttonImage)
            {
                if (trans.gameObject == selectedButton)
                {
                    buttonImage.color = _selectedButtonColor;
                    _currentLoadRoom = selectedButton.GetComponent<RoomListItem>();
                    _connectToRoomButton.SetActive(true);
                }
                else
                {
                    buttonImage.color = _defaultButtonColor;
                }
            }
        }
    }

    /**
     Метод, делающий все элементы списка не выбранными.
     */
    public void ClearCurrentLoadRoom()
    {
        foreach (Transform trans in _roomListContent)
        {
            Image buttonImage = trans.gameObject.GetComponent<Image>();
            if (buttonImage)
            {
                buttonImage.color = _defaultButtonColor;
            }
        }
        _currentLoadRoom = null;
        _connectToRoomButton.SetActive(false);
    }

    /// Метод, осуществляющий попытку подключения к выбранной в текущей момент комнате.
    public void ConnectToSelectedRoom()
    {
        if (_currentLoadRoom)
        {
            _networkManager.JoinRoom(_currentLoadRoom.GetRoomInfo());
        }
    }

    /// Метод, создающий новую комнату с ID равным количеству уже существующих комнат данного типа.
    public void CreateNewRoom()
    {
        _networkManager.InitRoom(_roomType, _rooms.Count);
    }

    private void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _rooms = roomList;
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform trans in _roomListContent)
        {
            Destroy(trans.gameObject);
        }

        if (_roomButtons == null)
        {
            _roomButtons = new List<RoomListItem>();
        }
        _roomButtons.Clear();
        _currentLoadRoom = null;
        _connectToRoomButton.SetActive(false);

        if (_rooms != null)
        {
            foreach (RoomInfo room in _rooms)
            {
                if ((int)room.CustomProperties[RoomSettings.ROOM_INDEX] == _roomType)
                {
                    RoomListItem item = Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>();
                    if (item)
                    {
                        item.SetUp(room, this);
                        _roomButtons.Add(item);

                        item.gameObject.GetComponent<Image>().color = _defaultButtonColor;
                    }
                }
            }
        }
    }
}
