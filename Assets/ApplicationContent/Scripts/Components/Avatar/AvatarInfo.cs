using UnityEngine;

/**
 Класс, хранящий информацию об аватаре.

Данный класс необходим для поиска prefab-ов аватаров в папке Resources.
Так же данный класс используется для отображения аватаров в UI.

@param isActive Если аватар не активен, он не будет добавлятся в список аватаров и не будет отображаться.
Это нужно, если какой-то аватар еще не настроен, но необходимо вести работу с другими аватарами.
@param avatarImage Изображение отображаемое в UI.
@param avatarName Путь до аватара в папке Resources/Avatars. По данному пути NetworkManager будет искать данный аватар.
 */
public class AvatarInfo : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Sprite _avatarImage;
    [SerializeField] private string _avatarName;

    /**
     Геттер активности аватара.
    @return bool Активен ли данный аватар.
     */
    public bool IsAvatarActive()
    {
        return _isActive;
    }

    /**
     Геттер изображения аватара.
    @return Sprite Изображение аватара.
     */
    public Sprite GetAvatarImage()
    {
        return _avatarImage;
    }

    /**
      Геттер имени аватара.

     Имя аватара — это путь до prefab-а с аватаром в папке Resources/Avatars.
     @return string Путь до prefab-а с аватаром в папке Resources/Avatars.
      */
    public string GetAvatarName()
    {
        return _avatarName;
    }
}
