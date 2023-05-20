using UnityEngine;

/// Тип руки.
public enum HandType
{
    None = 0, ///< Не назначен.
    Right = 1, ///< Правая рука.
    Left = 2 ///< Левая рука.
};

/**
Компонент, хранящий в себе свойства жеста.

Данный компонент, инкапсулирующий работу с информацией о загнутых пальцах в жесте.
Данный компонент используется для анимации рук на сервере. 
Каждый жест, используемый для анимации модели руки на сервере, имеет данный компонент.
@param gestureFingers GestureFingers данного жеста.
 */
public class GestureProperties : MonoBehaviour
{
    /** Тип руки данного жеста:
    - None;
    - Right;
    - Left;
     */
    public HandType Type;
    /// Загнут ли большой палец.
    public bool Thumb;
    /// Загнут ли указательный палец.
    public bool Index;
    /// Загнут ли средний палец.
    public bool Middle;
    /// Загнут ли безымянный палец.
    public bool Ring;
    /// Загнут ли мизинец.
    public bool Pinky;

    private bool[] _areFingersClosed;

    /// Конструктор по умолчанию. Ни один палец не загнут.
    public GestureProperties()
    {
        _areFingersClosed = new bool[5];
        Thumb = false;
        Index = false;
        Middle = false;
        Ring = false;
        Pinky = false;
        RefreshFingersArray();
    }

    /** 
     Сеттер загнутых пальцев из другого GestureFingers.
     @param [in] fingers Копируемый GestureProperties.
     */
    public void SetFromAnotherGestureProperties(GestureProperties fingers)
    {
        this.Type = fingers.Type;
        this.Thumb = fingers.Thumb;
        this.Index = fingers.Index;
        this.Middle = fingers.Middle;
        this.Ring = fingers.Ring;
        this.Pinky = fingers.Pinky;
        RefreshFingersArray();
    }

    /** 
     Сеттер загнутых пальцев из массива bool.
     @param [in] fingers Массив bool загнутых пальцев.
     */
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

    /**
     Геттер массива загнутых пальцев.
    @return bool[] Массив загнутых пальцев.
     */
    public bool[] GetFingersState()
    {
        RefreshFingersArray();
        return _areFingersClosed;
    }

    /** 
    Геттер HandType данного жеста.
    @return HandType:
    - None;
    - Right;
    - Left;
     */
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
