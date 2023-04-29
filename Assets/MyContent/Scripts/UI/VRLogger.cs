using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 ### Скрипт для вывода логов внутри игры

@param logField TextMeshPro, в который будут выводиться логи.
@param maxLine Максимальное число строк в логах.
 */
public class VRLogger : MonoBehaviour
{
    [SerializeField] private TMP_Text _logField;
    [Range(1, 1000)]
    [SerializeField] private int _maxLine;

    private List<string> _lines;

    private void Awake()
    {
        _logField.text = "";
        _lines = new List<string>();
    }

    /**
     Метод для вывода лога.
    @param text string текст выводимого лога.
     */
    public void Log(string text)
    {
        if (_lines.Count >= _maxLine)
        {
            _lines.RemoveAt(0);
        }
        _lines.Add(text);
        FillLog();
    }

    /// Геттер текста логов.
    public string GetTextFromLogger()
    {
        return _logField.text;
    }

    /// Геттер текста логов в виде списка строк.
    public List<string> GetLoggerLines()
    {
        return _lines;
    }

    /** 
     Сеттер множества строк логов, через список строк логов.
     @param lines Список строк логов.
     */
    public void SetLoggerLines(List<string> lines)
    {
        this._lines = lines;
    }

    /// Метод для очистки логов.
    public void ClearLog()
    {
        _lines.Clear();
        FillLog();
    }

    private void FillLog()
    {
        _logField.text = "";
        foreach (string line in _lines)
        {
            _logField.text += line + "\n";
        }
    }
}
