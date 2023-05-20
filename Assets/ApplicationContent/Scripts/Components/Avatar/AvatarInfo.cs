using UnityEngine;

/**
 Класс, хранящий информацию об аватаре.

Данный класс необходим для поиска prefab-ов аватаров в папке Resources.
Так же данный класс используется для отображения аватаров в UI.

@param isActive Если аватар не активен, он не будет добавлятся в список аватаров и не будет отображаться.
Это нужно, если какой-то аватар еще не настроен, но необходимо вести работу с другими аватарами.
@param avatarImage Изображение отображаемое в UI.
@param avatarName Путь до аватара в папке с ресурсами. По данному пути NetworkManager будет искать данный аватар.
 */
public class AvatarInfo : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Sprite _avatarImage;
    [SerializeField] private string _avatarName;

    public bool IsAvatarActive()
    {
        return _isActive;
    }
    public Sprite GetAvatarImage()
    {
        return _avatarImage;
    }
    public string GetAvatarName()
    {
        return _avatarName;
    }
}
