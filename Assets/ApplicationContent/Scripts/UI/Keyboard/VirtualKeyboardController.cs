using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 Класс контроллер виртуальной клавиатуры.

Данный класс контролирует виртуальную клавиатуру.
В качестве параметров ему передаются объекты разных раскладок клавиатур.
Данный класс собирает все клавиши с данных раскладок и обеспечивает их работу.

Когда текстовому полю ввода нужно вызвать клавиатуру, это текстовое поле вызывает метод CreateVirtualKeyboard(TMP_InputField)
и передает себя в качестве параметра данного метода. В дальнейшем, до закрытия клавиатуры клавишей Esc, 
все кнопки будут вводить символы в указанное текстовое поле.
@param enKeyboard Объект английской раскладки клавиатуры. Данный объект должен содержать компоненты KeyboardButton.
@param ruKeyboard Объект русской раскладки клавиатуры. Данный объект должен содержать компоненты KeyboardButton.
@param numberKeyboard Объект раскладки клавиатуры, содержащей спецсимволы. Данный объект должен содержать компоненты KeyboardButton.
@see InterfaceHider; KeyboardButton
 */
[RequireComponent(typeof(InterfaceHider))]
public class VirtualKeyboardController : MonoBehaviour
{
    /// Константа имени специальной кнопки
    public const string BACKSPACE = "←";
    /// Константа имени специальной кнопки
    public const string SHIFT = "↑";
    /// Константа имени специальной кнопки
    public const string CAPS_LOCK = "Caps\nLock";
    /// Константа имени специальной кнопки
    public const string TO_NUMBERS = "123";
    /// Константа имени специальной кнопки
    public const string TO_LETTERS = "abc";
    /// Константа имени специальной кнопки
    public const string BROWSE = "(#)";
    /// Константа имени специальной кнопки
    public const string ENTER = "Enter";
    /// Константа имени специальной кнопки
    public const string ESCAPE = "Esc";

    [SerializeField] private Transform _enKeyboard;
    [SerializeField] private Transform _ruKeyboard;
    [SerializeField] private Transform _numberKeyboard;

    private List<KeyboardButton> _buttons;

    private TMP_InputField _inputText;
    private InterfaceHider _hider;

    private bool _isShift;
    private bool _isCapsLock;
    private bool _isEnKeyboard;
    private bool _isNumberKey;

    private void Start()
    {
        _isShift = false;
        _isCapsLock = false;
        _isEnKeyboard = true;
        _isNumberKey = false;

        _buttons = new List<KeyboardButton>();

        FiilButtonsArray(_enKeyboard);
        FiilButtonsArray(_ruKeyboard);
        FiilButtonsArray(_numberKeyboard);

        _hider = GetComponent<InterfaceHider>();

        ChangeLetterKeyboard();
        transform.localScale = Vector3.zero;
    }

    /**
     Метод, показывающий виртуальную клавиатуру.

    Данный метод вызывается текстовым полем ввода.
    Входным параметром является поле ввода, в которое будет вводить текст данная клавиатура.
    @param inputText TMP_InputField, в которое будет вводить текст данная клавиатура.
     */
    public void CreateVirtualKeyboard(TMP_InputField inputText)
    {
        _hider.ShowInterface();
        _inputText = inputText;
    }

    /**
     Метод, закрывающий виртуальную клавиатуру.

    Данный метод скроет виртуальную клавиатуру 
    и отвяжет ее от текстового поля, в которое осуществлялся ввод.
     */
    public void CloseVirtualKeyboard()
    {
        _inputText = null;
        _hider.HideInterface();
    }

    /**
     Метод, вызываемый кнопкой клавиатуры. 
    
    В качестве входного параметра, кнопка вызвавшая данный метод, передает свой текст.

    Если данный метод вызывается обычной кнопкой, 
    то произойдет ввод текста кнопки в текстовое поле.
    Если данный метод вызывается специальной кнопкой,
    то произойдет действие соответствующие специальной кнопке.
    @param buttonName Имя кнопки, вызывающей данный метод.
     */
    public void ButtonAction(string buttonName)
    {
        switch (buttonName)
        {
            case BACKSPACE:
                Backspace();
                break;
            case SHIFT:
                _isShift = !_isShift;
                ChangeUpperCase(_isShift);
                break;
            case CAPS_LOCK:
                _isCapsLock = !_isCapsLock;
                ChangeUpperCase(_isCapsLock);
                break;
            case TO_NUMBERS:
            case TO_LETTERS:
                ChangeNumberKey();
                break;
            case BROWSE:
                _isEnKeyboard = !_isEnKeyboard;
                ChangeLetterKeyboard();
                break;
            case ENTER:
                EnterText("\n");
                break;
            case ESCAPE:
                CloseVirtualKeyboard();
                break;
            default:
                EnterText(buttonName);
                break;
        }
    }

    private void FiilButtonsArray(Transform keyboard)
    {
        foreach (KeyboardButton button in keyboard.gameObject.GetComponentsInChildren<KeyboardButton>())
        {
            button.SetKeyboardController(this);
            _buttons.Add(button);
        }

    }

    private void ChangeUpperCase(bool isUpperCase)
    {
        foreach (KeyboardButton button in _buttons)
        {
            button.ChangeUpperCase(isUpperCase);
        }
    }

    private void ChangeNumberKey()
    {
        _isNumberKey = !_isNumberKey;
        if (_isNumberKey)
        {
            _enKeyboard.gameObject.SetActive(false);
            _ruKeyboard.gameObject.SetActive(false);
            _numberKeyboard.gameObject.SetActive(true);
        }
        else
        {
            ChangeLetterKeyboard();
        }
    }

    private void ChangeLetterKeyboard()
    {
        if (_isEnKeyboard)
        {
            _enKeyboard.gameObject.SetActive(true);
            _ruKeyboard.gameObject.SetActive(false);
            _numberKeyboard.gameObject.SetActive(false);
        }
        else
        {
            _enKeyboard.gameObject.SetActive(false);
            _ruKeyboard.gameObject.SetActive(true);
            _numberKeyboard.gameObject.SetActive(false);
        }
    }

    private void Backspace()
    {
        if (_inputText != null && _inputText.text.Length > 0)
        {
            _inputText.text = _inputText.text.Substring(0, _inputText.text.Length - 1);
        }
    }

    private void EnterText(string text)
    {
        if (_inputText != null)
        {
            _inputText.text = _inputText.text + text;
            if (_isShift)
            {
                _isShift = !_isShift;
                ChangeUpperCase(_isShift);
            }
        }
    }
}
