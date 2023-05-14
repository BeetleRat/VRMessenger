using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/**
Класс, контролирующий все элементы списка доступных аватаров.

Данный класс ищет в папке ресурсов аватары AvatarInfo и из них составляет список аватаров и отображает его в указанном UI.

Так же данный класс связывается с NetworkManager для указания ему выбранного в текущий момент аватара.


@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkManager;

@param catcher ComponentCatcher данной сцены.
@param avatarsListContent UI поле, в которое будет отображен список аватаров.
@param avatarListItemPrefab Prefab отображаемого элемента списка. Данный prefab должен содержать компонент AvatarListItem.
@see ComponentCatcher; NetworkManager; AvatarListItem; AvatarInfo
 */
public class AvatarList : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    [SerializeField] private Transform _avatarsListContent;
    [SerializeField] private GameObject _avatarListItemPrefab;
    [Tooltip("All avatars should be stored in a subfolder of the Resources folder.")]
    [SerializeField] private string _avatarsFolder;


    private List<AvatarListItem> _avatarListItems;
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = _catcher.GetNetworkManager();
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent += OnNetworConnection;
        }
        UpdateAvatarList();
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.NetworConnectionEvent -= OnNetworConnection;
        }
    }

    /**
     Метод изменяющий выбранный в данный момент элемент списка.

    Данный метод делает не выбранными все элементы списка, кроме переданного ему во входном параметре.
    Так же в данном методе устанавливается новое имя спавнемого prefab-а игрока(аватара).
    @param newSelectedElement AvatarListItem выбранный в данный момент.
     */
    public void ChangeCurrentElement(AvatarListItem newSelectedElement)
    {
        foreach (AvatarListItem item in _avatarListItems)
        {
            if (item == newSelectedElement)
            {
                item.SetSelected(true);
                _networkManager?.SetPlayersPrefabName(item.GetAvatarName());
            }
            else
            {
                item.SetSelected(false);
            }
        }
    }

    private void UpdateAvatarList()
    {
        foreach (Transform trans in _avatarsListContent)
        {
            Destroy(trans.gameObject);
        }

        if (_avatarListItems == null)
        {
            _avatarListItems = new List<AvatarListItem>();
        }
        _avatarListItems.Clear();

        AvatarInfo[] avatars = Resources.LoadAll<AvatarInfo>(_avatarsFolder);
        foreach (AvatarInfo avatar in avatars)
        {
            if (avatar.IsAvatarActive())
            {
                AvatarListItem item = Instantiate(_avatarListItemPrefab, _avatarsListContent).GetComponent<AvatarListItem>();
                item.SetControllerList(this);
                item.SetSelected(false);
                item.SetAvatarImage(avatar.GetAvatarImage());
                item.SetAvatarName(avatar.GetAvatarName());
                _avatarListItems.Add(item);
            }
        }

        if (_avatarListItems.Count > 0)
        {
            _avatarListItems[0].SetSelected(true);
        }
    }

    private void OnNetworConnection(NetworkCode code)
    {
        switch (code)
        {
            case NetworkCode.CONNECT_TO_LOBBY_COMPLETE:
                foreach (AvatarListItem item in _avatarListItems)
                {
                    if (item.IsSelected())
                    {
                        _networkManager?.SetPlayersPrefabName(item.GetAvatarName());
                        break;
                    }
                }
                break;
        }
    }
}
