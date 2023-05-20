using System.Collections.Generic;
using UnityEngine;

/**
Класс миллиамперметра.

По данному классу элемент в электронной цепи определяется как Амперметр.

Миллиамперметр выводит значение силы тока в замкнутой электронной цепи, к которой он подключен, 
на шкалу ScaleWithPointer данного миллиамперметра.
@note Что бы миллиамперметр работал, он должен присутствовать в замкнутой электронной цепи ElectricalCircuit.
@param pluse WireInput миллиамперметра, из которого должен выходить ток.
Если в данный вход будет входить ток, то миллиамперметра будет показывать отрицательные значения.
@param minus WireInput миллиамперметра, в который должен входить ток.
Если из данного входа будет выходить ток, то миллиамперметра будет показывать отрицательные значения.
@param pointer Шкала измерений данного миллиамперметра ScaleWithPointer.
@see ScaleWithPointer; WireInput; WireElement; ElectricalElement; ElectricityTransfer; ElectricalCircuit
*/
public class Milliammeter : ElectricalElement, ElectricityTransfer
{
    [SerializeField] private WireInput _pluse;
    [SerializeField] private WireInput _minus;
    [SerializeField] private ScaleWithPointer _pointer;

    private int _multiplier;
    /// Множитель для перевода миллиампер в амперы
    private const int _valueScaler = 1000;


    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для вольтметра миллиамперметра.
     */
    protected new void Awake()
    {
        base.Awake();
        _name = AMPERMETER;
        _pluse.SetParent(this);
        _minus.SetParent(this);
        _multiplier = 0;
    }

    private void Update()
    {
        if (_electricalCircuit != null && _electricalCircuit.IsCircuitClosed())
        {
            _pointer.SetCurrentValue(_electricalCircuit.GetAmperage() * _valueScaler * _multiplier);
        }

        UpdateElectricityStatus();
    }

    /**
     Установить множитель показаний миллиамперметра.

    Если ток течет от minus к pluse, то множитель равен (1).
    Если ток течет в обратном направлении, множитель равен (-1).
    @param [in] multiplier Множитель.
     */
    public void SetMultiplier(int multiplier)
    {
        _multiplier = multiplier;
    }

    /// Реализация метода интерфейса.
    public List<WireElement> GetMinusWireElements()
    {
        UpdateElectricityStatus();
        List<WireElement> wireElements = new List<WireElement>();
        if (_minus.GetConnectedWire() != null && _minus.GetConnectedWire().GetOutputType() == OutputType.Minuse)
        {
            wireElements.Add(_minus.GetConnectedWire().GetWire());
        }
        if (_pluse.GetConnectedWire() != null && _pluse.GetConnectedWire().GetOutputType() == OutputType.Minuse)
        {
            wireElements.Add(_pluse.GetConnectedWire().GetWire());
        }

        return wireElements;
    }

    private void UpdateElectricityStatus()
    {
        if (_minus.GetConnectedWire() == null)
        {
            if (_pluse.GetConnectedWire() != null && _pluse.GetConnectedWire().GetOutputType() == OutputType.Minuse)
            {
                _pluse.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.None);
            }
        }
        else
        {
            if (_pluse.GetConnectedWire() == null)
            {
                if (_minus.GetConnectedWire().GetOutputType() == OutputType.Minuse)
                {
                    _minus.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.None);
                }
            }
            else
            {
                if (_minus.GetConnectedWire().GetOutputType() == OutputType.Pluse)
                {
                    _multiplier = 1;
                    _pluse.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.Minuse);
                }
                else
                {
                    if (_pluse.GetConnectedWire().GetOutputType() == OutputType.Pluse)
                    {
                        _multiplier = -1;
                        _minus.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.Minuse);
                    }
                }
            }
        }
    }
}
