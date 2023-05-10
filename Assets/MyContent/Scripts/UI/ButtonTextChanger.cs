using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/**
Компонент меняющий текст в текстовом поле при наведении курсора на кнопку

@param textField TextMeshPro, в котором будет меняться текст.
@param text string текст, который появиться в textField при наведении на кнопку.
 */
public class ButtonTextChanger : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text _textField;
    [TextArea(2,4)]
    [SerializeField] private string _text;

    /// Реализация метода интерфейса. Метод вызываемый при наведении курсора на кнопку.
    public void OnPointerEnter(PointerEventData eventData)
    {
        _textField.text = _text;
    }
}
