using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 ### Класс пункта списка комнат

В приложении есть список комнат, к которым мы можем подключаться. 
Список состоит из ряда объектов данного класса. 
Данный класс хранит информацию о комнате, к которой мы можем подключиться через данный пункт списка.

@param buttonText Текстовое поле, в которое будет вписано название пункта списка.
 */
public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;

    private RoomInfo _roomInfo;
    private UIRoomListController _roomConnector;

    /**
     Настройка пункта меню.

    После создания пункта списка, в него нужно занести информацию, которую он будет в себе хранить.
    Так же через данный метод передается UIConnectToRoom, отвечающий за работу с пунктами списка.
    @param [in] roomInfo Информация о комнате, к которой можно подключиться через данный пункт списка.
    @param [in] roomConnector UIRoomListController управляющий данным пунктом списка.
     */
    public void SetUp(RoomInfo roomInfo, UIRoomListController roomConnector)
    {
        _roomInfo = roomInfo;
        _buttonText.text = "     " + roomInfo.Name;
        _roomConnector = roomConnector;
    }

    /**
     Геттер Информации о комнате, к которой можно подключиться через данный пункт списка.
    @return RoomInfo Информации о комнате, к которой можно подключиться через данный пункт списка.
     */
    public RoomInfo GetRoomInfo()
    {
        return _roomInfo;
    }

    /// Метод нажатия на пункт списка.
    public void OnClick()
    {
        _roomConnector?.ChangeCurrentLoadRoom(gameObject);
    }
}
