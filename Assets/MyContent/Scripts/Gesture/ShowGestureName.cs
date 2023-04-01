using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowGestureName : GestureDetector
{
    [SerializeField] private TMP_Text text;

    protected override void GestureSelected(ActiveStateSelector gesture)
    {
        text.text = gesture.gameObject.name;
        Debug.Log("gesture.gameObject.name - " + gesture.gameObject.name);
        
    }

    protected override void GestureUnselected(ActiveStateSelector gesture)
    {
        text.text = "";
        Debug.Log("Очистка текста");
    }
}
