using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRLogger : MonoBehaviour
{
    [SerializeField] private TMP_Text _logField;
    [Range(1, 20)]
    [SerializeField] private int _maxLine;
    private List<string> _lines;

    private void Start()
    {
        _logField.text = "";
        _lines = new List<string>();
    }

    public void Log(string text)
    {
        if(_lines.Count >= _maxLine)
        {
            _lines.RemoveAt(0);
        }
        _lines.Add(text);
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
