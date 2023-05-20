using UnityEngine;

/// Ось Эйлеровых координат
public enum CoordinateAxis
{
    X, ///< Ось X
    Y, ///< Ось Y
    Z ///< Ось Z
}

/**
Класс шкалы с указателем.

Приборы на подобии амперметра имеют шкалу измерений.
Текущее значение указывается указателем. 
Шкала представляет собой полукруг. А указатель стрелку, идущую из центра круга.
Что бы изменить значение показателя, нам нужно изменить поворот указателя.
Данный компонент управляет поворотом указателя, в зависимости от текущего значения измерений.
@param rotationAxis Ось поворота указателя CoordinateAxis.
@param pointer Объект указателя.
@param minAngle Минимальное значение угла поворота. 
В данном значении указатель должен указывать на минимальное значение шкалы.
@param maxAngle Максимальное значение угла поворота. 
В данном значении указатель должен указывать на максимальное значение шкалы.
@param speed Скорость изменения поворота указателя.
@param minValue Минимальное значение шкалы. На данное значение указывает указатель при повороте на minAngle.
@param maxValue Максимальное значение шкалы. На данное значение указывает указатель при повороте на maxAngle.
@see CoordinateAxis
 */
public class ScaleWithPointer : MonoBehaviour
{
    [Header("Pointer object")]
    [SerializeField] private CoordinateAxis _rotationAxis;
    [SerializeField] private Transform _pointer;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _speed;
    [Header("Measurand")]
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;

    private float _currentValue;
    private float _angleDistance;
    private float _valueRange;
    private bool isPointerMoving;
    private float _lastValue;

    private void Start()
    {
        _angleDistance = _maxAngle - _minAngle;
        _valueRange = _maxValue - _minValue;
        isPointerMoving = false;
        _lastValue = 0;
    }

    private void Update()
    {
        if (!isPointerMoving)
        {
            SetPointer(_currentValue);
        }
    }

    /**
     Установить текущее значение шкалы.
    @param [in] value Текущее значение шкалы.
     */
    public void SetCurrentValue(float value)
    {
        _currentValue = value;
    }

    private void SetPointer(float value)
    {
        value = Mathf.Clamp(value, _minValue, _maxValue);
        if (value != _lastValue)
        {
            _lastValue = value;
            isPointerMoving = true;
            float rotationAngle = (value / _valueRange) * _angleDistance;
            switch (_rotationAxis)
            {
                case CoordinateAxis.X:
                    _pointer.LeanRotateX(rotationAngle, _speed).setOnComplete(() => isPointerMoving = false);
                    break;
                case CoordinateAxis.Y:
                    _pointer.LeanRotateY(rotationAngle, _speed).setOnComplete(() => isPointerMoving = false);
                    break;
                case CoordinateAxis.Z:
                    _pointer.LeanRotateZ(rotationAngle, _speed).setOnComplete(() => isPointerMoving = false);
                    break;
            }
        }
    }
}
