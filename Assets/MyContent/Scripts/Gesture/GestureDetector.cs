using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public abstract class GestureDetector : MonoBehaviour
{
    [SerializeField] protected List<ActiveStateSelector> _gestures;

    private void Start()
    {
        foreach (ActiveStateSelector gesture in _gestures)
        {
            gesture.WhenSelected += () => GestureSelected(gesture);
            gesture.WhenUnselected += () => GestureUnselected(gesture);
        }
    }

    protected abstract void GestureSelected(ActiveStateSelector gesture);
    protected abstract void GestureUnselected(ActiveStateSelector gesture);
}
