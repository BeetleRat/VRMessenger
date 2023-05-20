using UnityEngine;

/**
Класс реостата

По данному классу элемент в электронной цепи определяется как Реостат.

Реостат - электрический аппарат для регулирования и ограничения тока или напряжения в электрической цепи, 
основная часть которого — проводящий элемент с переменным электрическим сопротивлением.

@param positionAxis Ось CoordinateAxis, по которой движется указатель реостата.
@param pointer Указатель реостата.
@param minDistance Минимальное положение указателя реостата. В этом положении сопротивление реостата будет минимальным.
@param maxDistance Максимальное положение указателя реостата. В этом положении сопротивление реостата будет максимальным.
@param step Шаг пересчета сопротивления. Когда указатель проходит данное расстояние, происходит пересчет сопротивления в соответствии с новым положением указателя.
Данный параметр позволяет не пересчитывать постоянно всю электронную цепь, при неизменном сопротивлении.
@param minValue Минимальное значение сопротивления. При нахождении указателя в позиции minDistance, реостат будет иметь данное сопротивление.
@param maxValue Максимальное значение сопротивления. При нахождении указателя в позиции maxDistance, реостат будет иметь данное сопротивление.
@see Resistor
 */
public class Rheostat : Resistor
{
    [Header("Pointer object")]
    [SerializeField] private CoordinateAxis _positionAxis;
    [SerializeField] private Transform _pointer;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _step;
    [Header("Measurand")]
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;

    private Vector3 _previousPosition;
    private float _distance;
    private float _valueRange;
    private float _currentDistance;

    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для реостата параметров.
     */
    protected new void Awake()
    {
        base.Awake();
        _distance = _maxDistance - _minDistance;
        _valueRange = _maxValue - _minValue;
        _currentDistance = _minDistance;
        ReaclculateCurrentDistance();
        _resistance = Mathf.Clamp(_minValue + (_currentDistance / _distance) * _valueRange, _minValue, _maxValue);
    }

    /**
     Метод вызываемый один раз за кадр.
    В данном методе в реальном времени определяется направление тока внутри реостата.
    А также происходит подсчет сопротивления в зависимости от положения указателя.
     */
    protected new void Update()
    {
        base.Update();
        ReaclculateCurrentDistance();

        float distance = Vector3.Distance(_pointer.localPosition, _previousPosition);
        if (distance > _step)
        {
            _resistance = Mathf.Clamp(_minValue + (_currentDistance / _distance) * _valueRange, _minValue, _maxValue);
            _previousPosition = new Vector3(_pointer.localPosition.x, _pointer.localPosition.y, _pointer.localPosition.z);
            _electricalCircuit?.RecalculateCircuit();
        }
    }

    private void ReaclculateCurrentDistance()
    {
        switch (_positionAxis)
        {
            case CoordinateAxis.X:
                _currentDistance = Mathf.Clamp(_pointer.localPosition.x, _minDistance, _maxDistance) - _minDistance;
                break;
            case CoordinateAxis.Y:
                _currentDistance = Mathf.Clamp(_pointer.localPosition.y, _minDistance, _maxDistance) - _minDistance;
                break;
            case CoordinateAxis.Z:
                _currentDistance = Mathf.Clamp(_pointer.localPosition.z, _minDistance, _maxDistance) - _minDistance;
                break;
        }
    }
}
