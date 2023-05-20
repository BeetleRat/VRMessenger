using System.Collections.Generic;
using UnityEngine;

/**
 Класс переходника на 3 входа.

Данный переходник необходим для подключения вольтметра (Voltmeter) в электрическую цепь.
Ведь вольтметр должен включаться параллельно.
@param leftInput Левый вход WireInput переходника.
@param rightInput Правый вход WireInput переходника.
@param parallelInput Вход WireInput находящийся по середине переходника. 
К данному входу должен подключаться вольтметр (Voltmeter).
@see WireInput; WireElement; ElectricalElement; ElectricityTransfer
 */
public class MultiClema : ElectricalElement, ElectricityTransfer
{
    [SerializeField] private WireInput _leftInput;
    [SerializeField] private WireInput _rightInput;
    [SerializeField] private WireInput _parallelInput;

    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для переходника параметров.
     */
    protected new void Awake()
    {
        base.Awake();
        _name = MULTY_CLEMA;
        _leftInput.SetParent(this);
        _rightInput.SetParent(this);
        _parallelInput.SetParent(this);
    }

    private void Update()
    {
        UpdateElectricityStatus();
    }

    /**
     Геттер провода, подключенного к левому входу переходника.
    @return WireElement, подключенный к левому входу переходника.
     */
    public WireElement GetLeftConnectedWire()
    {
        return _leftInput.GetConnectedWire()?.GetWire();
    }

    /**
     Геттер провода, подключенного к правому входу переходника.
    @return WireElement, подключенный к правому входу переходника.
     */
    public WireElement GetRightConnectedWire()
    {
        return _rightInput.GetConnectedWire()?.GetWire();
    }

    /**
     Геттер провода, подключенного к находящемуся по середине входу переходника.
    @return WireElement, подключенный к находящемуся по середине входу переходника.
     */
    public WireElement GetParallelConnectedWire()
    {
        return _parallelInput.GetConnectedWire()?.GetWire();
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
        if (_parallelInput.GetConnectedWire() != null && _parallelInput.GetConnectedWire().GetOutputType() == OutputType.Minuse)
        {
            wireElements.Add(_parallelInput.GetConnectedWire().GetWire());
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

        if (_parallelInput.GetConnectedWire() != null)
        {
            if (_parallelInput.GetConnectedWire().GetOutputType() == OutputType.None)
            {
                _parallelInput.GetConnectedWire().SetOutputTypeForWholeWire(OutputType.Minuse);
            }
        }
    }
}
