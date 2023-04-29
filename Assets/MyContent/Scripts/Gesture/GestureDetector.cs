using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

/**
 ### Абстрактный суперкласс для распознавания жестов

Данный класс распознает жесты указанные в его списке распознаваемых жестов.
 */
public abstract class GestureDetector : MonoBehaviour
{
    /// Список жестов, распознаваемых данной системой.
    [SerializeField] protected List<ActiveStateSelector> _gestures;

    private void Start()
    {
        foreach (ActiveStateSelector gesture in _gestures)
        {
            gesture.WhenSelected += () => GestureSelected(gesture);
            gesture.WhenUnselected += () => GestureUnselected(gesture);
        }
    }

    /**
    Метод, вызываемый при распознавании жеста.
    @param [in] gesture Распознанный жест.
     */
    protected abstract void GestureSelected(ActiveStateSelector gesture);

    /**
    Метод, вызываемый по окончании распознавания жеста.
    @param [in] gesture Распознанный жест.
     */
    protected abstract void GestureUnselected(ActiveStateSelector gesture);
}
