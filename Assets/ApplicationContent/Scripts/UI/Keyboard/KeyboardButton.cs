using UnityEngine;
using TMPro;

/**
 Класс кнопки виртуальной клавиатуры.
@param buttonText Текстовое поле кнопки.
@see VirtualKeyboardController
 */
public class KeyboardButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;

    private string _text;

    private VirtualKeyboardController _keyboardController;

    private bool _isSpecialButton;

    private void Start()
    {

        _text = _buttonText.text;
        _isSpecialButton =
            _text == VirtualKeyboardController.BACKSPACE
            || _text == VirtualKeyboardController.SHIFT
            || _text == VirtualKeyboardController.CAPS_LOCK
            || _text == VirtualKeyboardController.TO_NUMBERS
            || _text == VirtualKeyboardController.BROWSE
            || _text == VirtualKeyboardController.ENTER
            || _text == VirtualKeyboardController.ESCAPE;
    }

    /**
     Метод устанавливающий контроллер клавиатуры для данной кнопки.

    Все кнопки на клавиатуре контролируются классом VirtualKeyboardController.
    Когда данный класс собирает кнопки, которые он контролирует, 
    он так же должен через данный метод установить себя, как контролирующий класс для данной кнопки.
    @param VirtualKeyboardController Контроллер клавиатуры данной кнопки.
     */
    public void SetKeyboardController(VirtualKeyboardController keyboardController)
    {
        _keyboardController = keyboardController;
    }

    /**
     Метод устанавливающий символ в заглавный или в строчный.
    Не производит действия на специальные кнопки.
    @param isUpperCase Если true, то установит символ в заглавный.
    Если false, то установит символ в строчный.
     */
    public void ChangeUpperCase(bool isUpperCase)
    {
        if (!_isSpecialButton)
        {
            if (isUpperCase)
            {
                _text = _text.ToUpper();
            }
            else
            {
                _text = _text.ToLower();
            }
            _buttonText.text = _text;
        }
    }

    /// Метод нажатия на кнопку
    public void OnClick()
    {
        _keyboardController?.ButtonAction(_text);
    }
}
