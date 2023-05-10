using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
Класс, вызывающий события смены жестов левой и правой руки

Данный класс используется для анимации моделей рук на сервере. 

Принцип работы.

Жесты, используемые для анимации моделей рук, имеют компонент GestureProperties. 
В данном компоненте указано, какие пальцы загнуты у данного жеста. 
Данный класс отлавливает жесты с компонентом GestureProperties, переводит его в массив bool и отправляет его скрипту HandsAnimaionUpdater.
@see GestureDetector
 */
public class GestureAnimation : GestureDetector
{
    /// Событие изменение жеста левой руки. Используется в HandsAnimaionUpdater.
    public UnityAction<bool[]> LeftGestChange;
    /// Событие изменение жеста правой руки. Используется в HandsAnimaionUpdater.
    public UnityAction<bool[]> RightGestChange;

    /**
    Метод, вызываемый при распознавании жеста системой GestureDetector.
    @param [in] gesture Распознанный жест.
     */
    protected override void GestureSelected(ActiveStateSelector gesture)
    {
        if (gesture.gameObject.TryGetComponent(out GestureProperties gestureProperties))
        {

            switch (gestureProperties.GetHandType())
            {
                case HandType.Left:
                    LeftGestChange?.Invoke(gestureProperties.GetFingersState());
                    break;
                case HandType.Right:
                    RightGestChange?.Invoke(gestureProperties.GetFingersState());
                    break;
                default:
                    Debug.LogWarning("[" + this.name + "] Жесту не назначен тип руки.");
                    break;
            }
        }
    }

    /**
    Метод, вызываемый по окончании распознавания жеста системой GestureDetector.
    @param [in] gesture Распознанный жест.
     */
    protected override void GestureUnselected(ActiveStateSelector gesture)
    {
        if (gesture.gameObject.TryGetComponent(out GestureProperties gestureProperties))
        {
            switch (gestureProperties.GetHandType())
            {
                case HandType.Left:
                    //LeftGestChange?.Invoke(new bool[5]);
                    break;
                case HandType.Right:
                    //RightGestChange?.Invoke(new bool[5]);
                    break;
                default:
                    Debug.LogWarning("[" + this.name + "] Жесту не назначен тип руки.");
                    break;
            }
        }
    }
}
