using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
Класс пункта списка аватаров

В приложении есть список возможных аватаров. 
Список состоит из ряда объектов данного класса. 
Данный класс хранит информацию об аватаре, который будет отображаться в виртуальной комнате.
Данный элемент так же отвечает за отображение аватара в списке AvatarList.

@param avatarImage Image, в котором будет выведено изображение аватара.
@param selectCheckboxImage Image выбранного checkbox-а.
@param unselectCheckboxImage Image невыбранного checkbox-а.
@param selectFrameImage Image рамки вокруг изображения аватара.
 */
public class AvatarListItem : MonoBehaviour
{
    [SerializeField] private Image _avatarImage;
    [SerializeField] private Image _selectCheckboxImage;
    [SerializeField] private Image _unselectCheckboxImage;
    [SerializeField] private Image _selectFrameImage;

    private AvatarList _controllerList;

    private bool _isSelected;
    private string _avatarName;

    private void Update()
    {
        _selectCheckboxImage.enabled = _isSelected;
        _selectFrameImage.enabled = _isSelected;
        _unselectCheckboxImage.enabled = !_isSelected;
    }

    /**
     Сеттер изображения аватара.
    @param image Sprite изображения аватара.
     */
    public void SetAvatarImage(Sprite image)
    {
        _avatarImage.sprite = image;
    }

    /**
     Сеттер названия аватара.
    @param name Название аватара.
     */
    public void SetAvatarName(string name)
    {
        _avatarName = name;
    }

    /**
     Сеттер состояния элемента списка.
    @param isSelected Выбран ли элемент.
     */
    public void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
    }

    /**
     Сеттер списка управляющего данным элементом.
    @param controllerList AvatarList управляющий данным элементом.
     */
    public void SetControllerList(AvatarList controllerList)
    {
        _controllerList = controllerList;
    }

    /**
     Геттер имени аватара.
    @return string имя аватара, загружаемое NetworkManager-ом.
     */
    public string GetAvatarName()
    {
        return _avatarName;
    }

    /**
     Геттер состояния элемента.
    @return bool выбран ли данный элемент.
     */
    public bool IsSelected()
    {
        return _isSelected;
    }

    /// Метод выбирающий данный элемент, если он не выбран.

    public void OnClick()
    {
        if (!_isSelected)
        {
            _controllerList?.ChangeCurrentElement(this);
        }
    }

}
