using System.Collections.Generic;
using UnityEngine;

/**
Класс электрической цепи.

Данный класс должен быть прикреплен к источнику электрического тока PowerSupply.
Т.к. электрическая цепь начинается и заканчивается на источнике тока.

Данный класс просчитывает электрическую цепь из элементов, последовательно подключенных друг к другу, 
начиная с источника электрического тока PowerSupply. В цепи просчитывается общее сопротивление и напряжение.
Если цепь замыкается, то есть последовательное соединение из более чем 2-х ElectricalElement, 
начинается в PowerSupply и заканчивается в этом же PowerSupply, то происходит расчет силы тока в цепи, 
согласно закону Ома: I=U/R.
@see PowerSupply; ElectricalElement
 */
[RequireComponent(typeof(PowerSupply))]
public class ElectricalCircuit : MonoBehaviour
{
    private List<ElectricalElement> _activeElements;
    private Dictionary<ElectricalElement, List<ElectricalElement>> _elementGraph;
    private float _amperage;
    private float _voltage;
    private float _resistance;
    private bool _isClosed;
    private PowerSupply _startElement;

    private void Start()
    {
        _activeElements = new List<ElectricalElement>();
        _elementGraph = new Dictionary<ElectricalElement, List<ElectricalElement>>();
        _startElement = GetComponent<PowerSupply>();
        _startElement.SetElectricCircuit(this);
        _activeElements.Add(_startElement);
    }

    /**
     Геттер состояния замкнутости электрической цепи.
    @return bool Замкнута ли электрическая цепь.
     */
    public bool IsCircuitClosed()
    {
        return _isClosed;
    }

    /**
     Получить силу тока в электрической цепи.
    @return float Сила тока в электрической цепи.
     */
    public float GetAmperage()
    {
        if (_isClosed)
        {
            return _amperage;
        }
        return 0;
    }

    /**
     Получить напряжение в электрической цепи.
    @return float Напряжение в электрической цепи.
     */
    public float GetVoltage()
    {
        if (_isClosed)
        {
            return _voltage;
        }
        return 0;
    }

    /**
     Пересчитать электрическую цепь.

    Данный метод обходит все элементы ElectricalElement, последовательно подключенные друг к другу,
    начиная с minus выхода PowerSupply. Во всей цепи происходит подсчет общего сопротивления. 
    И если по окончании обхода цепи, метод вышел к стартовому элементу PowerSupply, 
    то цепь считается замкнутой и происходит перерасчет силы тока в цепи. 
    Если цепь не замкнута, то сила тока в цепи считается 0.
     */
    public void RecalculateCircuit()
    {
        _resistance = 0;
        _voltage = 0;
        _amperage = 0;

        foreach (KeyValuePair<ElectricalElement, List<ElectricalElement>> element in _elementGraph)
        {
            element.Key.SetElectricCircuit(null);
        }
        _elementGraph.Clear();
        CreateElementGraph(_startElement, _startElement.GetMinusConnectedWire());
        _activeElements.Clear();
        if (!_elementGraph.ContainsKey(_startElement))
        {
            _isClosed = false;
        }
        else
        {
            _isClosed = CalculateElectricalCircuit(_elementGraph[_startElement][0]);
            if (_isClosed)
            {
                _amperage = _voltage / _resistance;
            }
        }
    }

    private void CreateElementGraph(ElectricalElement from, ElectricalElement to)
    {
        if (to == null)
        {
            return;
        }

        if (!_elementGraph.ContainsKey(from))
        {
            _elementGraph.Add(from, new List<ElectricalElement>());
        }
        else
        {
            if (_elementGraph[from].Contains(to))
            {
                return;
            }
        }

        to.SetElectricCircuit(this);
        _elementGraph[from].Add(to);

        if (to.GetConnectedElectricalElements().Count > 0)
        {
            ElectricityTransfer electricityTransfer = to as ElectricityTransfer;
            if (electricityTransfer == null)
            {
                foreach (ElectricalElement element in to.GetConnectedElectricalElements())
                {
                    if (element != from)
                    {
                        CreateElementGraph(to, element);
                    }
                }
            }
            else
            {
                foreach (ElectricalElement element in electricityTransfer.GetMinusWireElements())
                {
                    if (element != from)
                    {
                        CreateElementGraph(to, element);
                    }
                }
            }
        }
    }

    private bool CalculateElectricalCircuit(ElectricalElement currentElement)
    {
        if (currentElement == null || !_elementGraph.ContainsKey(currentElement))
        {
            return false;
        }

        if (currentElement == _startElement)
        {
            PowerSupply powerSupply = currentElement as PowerSupply;
            if (!_activeElements.Contains(currentElement))
            {
                _voltage += powerSupply.GetVoltage();
                _activeElements.Add(currentElement);
            }
            return true;
        }

        bool isCircuitClose = false;
        foreach (ElectricalElement electricalElement in _elementGraph[currentElement])
        {
            isCircuitClose = isCircuitClose || CalculateElectricalCircuit(electricalElement);
        }

        ConfigureCircuitWithElement(currentElement, isCircuitClose);

        return isCircuitClose;
    }

    private void ConfigureCircuitWithElement(ElectricalElement element, bool isCircuitClose)
    {
        switch (element.GetName())
        {
            case ElectricalElement.RESISTOR_AND_RHEOSTAT:
                Resistor resistor = element as Resistor;
                if (isCircuitClose)
                {
                    _resistance += resistor.GetResistence();
                }
                break;
            case ElectricalElement.POWER_SUPPLY:
                PowerSupply powerSupply = element as PowerSupply;
                if (isCircuitClose)
                {
                    _voltage += powerSupply.GetVoltage();
                }
                break;
            case ElectricalElement.VOLTMETER:
                Voltmeter voltmeter = element as Voltmeter;

                break;
            case ElectricalElement.AMPERMETER:
                Milliammeter ampermeter = element as Milliammeter;

                break;
            case ElectricalElement.WIRE:
                WireElement wire = element as WireElement;

                break;
        }
    }
}
