using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureAnimation : GestureDetector
{
    public UnityAction<bool[]> LeftGestChange;
    public UnityAction<bool[]> RightGestChange;
    protected override void GestureSelected(ActiveStateSelector gesture)
    {
        if (gesture.gameObject.TryGetComponent(out GestureProperties gestureProperties))
        {
            GestureFingers gestureFingers = gestureProperties.GetGestureFingers();
            switch (gestureFingers.GetHandType())
            {
                case GestureFingers.HandType.Left:
                    LeftGestChange?.Invoke(gestureFingers.GetFingersState());
                    break;
                case GestureFingers.HandType.Right:
                    RightGestChange?.Invoke(gestureFingers.GetFingersState());
                    break;
                default:
                    Debug.LogWarning("["+this.name+"] Жесту не назначен тип руки.");
                    break;
            }
        }
    }

    protected override void GestureUnselected(ActiveStateSelector gesture)
    {
        if (gesture.gameObject.TryGetComponent(out GestureProperties gestureProperties))
        {
            GestureFingers gestureFingers = gestureProperties.GetGestureFingers();
            switch (gestureFingers.GetHandType())
            {
                case GestureFingers.HandType.Left:
                    //LeftGestChange?.Invoke(new bool[5]);
                    break;
                case GestureFingers.HandType.Right:
                    //RightGestChange?.Invoke(new bool[5]);
                    break;
                default:
                    Debug.LogWarning("[" + this.name + "] Жесту не назначен тип руки.");
                    break;
            }
        }
    }
}
