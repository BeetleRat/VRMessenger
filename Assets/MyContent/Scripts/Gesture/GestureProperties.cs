using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.PoseDetection;

[System.Serializable]
public class GestureFingers
{
    public enum HandType { None = 0, Right = 1, Left = 2 };

    public HandType Type;
    public bool Thumb;
    public bool Index;
    public bool Middle;
    public bool Ring;
    public bool Pinky;

    private bool[] _areFingersClosed;

    public GestureFingers()
    {
        _areFingersClosed = new bool[5];
        Thumb = false;
        Index = false;
        Middle = false;
        Ring = false;
        Pinky = false;
        RefreshFingersArray();
    }

    public void SetFromAnotherGestureFingers(GestureFingers fingers)
    {
        this.Type = fingers.Type;
        this.Thumb = fingers.Thumb;
        this.Index = fingers.Index;
        this.Middle = fingers.Middle;
        this.Ring = fingers.Ring;
        this.Pinky = fingers.Pinky;
        RefreshFingersArray();
    }
    public void SetFromBoolArray(bool[] fingers)
    {
        if (fingers.Length >= 5)
        {
            this.Thumb = fingers[0];
            this.Index = fingers[1];
            this.Middle = fingers[2];
            this.Ring = fingers[3];
            this.Pinky = fingers[4];
            RefreshFingersArray();
        }

    }
    public bool[] GetFingersState()
    {
        RefreshFingersArray();
        return _areFingersClosed;
    }
    public HandType GetHandType()
    {
        return Type;
    }

    private void RefreshFingersArray()
    {
        _areFingersClosed[0] = Thumb;
        _areFingersClosed[1] = Index;
        _areFingersClosed[2] = Middle;
        _areFingersClosed[3] = Ring;
        _areFingersClosed[4] = Pinky;
    }
}

public class GestureProperties : MonoBehaviour
{
    [SerializeField] private GestureFingers _gestureFingers;

    public GestureFingers GetGestureFingers()
    {
        return _gestureFingers;
    }
}
