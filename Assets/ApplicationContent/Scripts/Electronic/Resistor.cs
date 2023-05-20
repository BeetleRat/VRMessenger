using System.Collections.Generic;
using UnityEngine;

/**
Класс резистора

По данному классу элемент в электронной цепи определяется как Резистор.

Резистор - пассивный элемент электрических цепей, обладающий определённым или переменным значением электрического сопротивления.

@param leftInput Условно левый вход WireInput резистора.
@param rightInput Условно правый вход WireInput резистора.
@param resistance Сопротивление резистора
@see WireInput; WireElement; ElectricalElement; ElectricityTransfer
 */
public class Resistor : ElectricalElement, ElectricityTransfer
{
    [SerializeField] private WireInput _leftInput;
    [SerializeField] private WireInput _rightInput;

    [SerializeField] protected float _resistance;

    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для резистора параметров.
     */
    protected new void Awake()
    {
        base.Awake();
        _name = RESISTOR_AND_RHEOSTAT;
        _leftInput.SetParent(this);
        _rightInput.SetParent(this);
    }

    /**
     Метод вызываемый один раз за кадр.
    В данном методе в реальном времени определяется направление тока внутри резистора.
     */
    protected void Update()
    {
        UpdateElectricityStatus();
    }

    /**
     Получить сопротивление резистора.
    @return float Сопротивление резистора.
     */
    public float GetResistence()
    {
        return _resistance;
    }

    /// Реализация метода интерфейса.
    public List<WireElement> GetMinusWireElements()
    {
        UpdateElectricityStatus();
        List<WireElement> wireElements = new List<WireElement>();
        if (_leftInput.GetConnectedWire() != null && _leftInput.GetConnectedWire().GetOutputType() == OutputType.Minuse)
        {
            wireElements.Add(_leftInput.GetConnectedWire().GetWire());
        }
        if (_rightInput.GetConnectedWire() != null && _rightInput.GetConnectedWire().GetOutputType() == OutputType.Minuse)
        {
            wireElements.Add(_rightInput.GetConnectedWire().GetWire());
        }

        return wireElements;
    }

    private void UpdateElectricityStatus()
    {
        if (_leftInput.GetConnectedWire() == null)
        {
            if (_rightInput.GetConnectedWire() != null && _rightInput.GetConnectedWire().GetOutputType() == OutputType.Minuse)
            {
                _rightInput.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.None);
            }
        }
        else
        {
            if (_rightInput.GetConnectedWire() == null)
            {
                if (_leftInput.GetConnectedWire().GetOutputType() == OutputType.Minuse)
                {
                    _leftInput.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.None);
                }
            }
            else
            {
                if (_leftInput.GetConnectedWire().GetOutputType() == OutputType.Pluse)
                {
                    _rightInput.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.Minuse);
                }
                else
                {
                    if (_rightInput.GetConnectedWire().GetOutputType() == OutputType.Pluse)
                    {
                        _leftInput.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.Minuse);
                    }
                }
            }
        }
    }
}
