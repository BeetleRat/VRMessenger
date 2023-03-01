using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextChanger : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private string _text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textField.text = _text;
    }
}
