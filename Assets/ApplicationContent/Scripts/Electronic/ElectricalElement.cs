using System.Collections.Generic;
using UnityEngine;

/**
 Интерфейс элементов, которые в реальном времени определяют направление тока внутри себя.

WireElement не определяет направление в реальном времени.
@see WireElement
 */
public interface ElectricityTransfer
{
    /**
     Получить провода, в которые утекает ток.
    @return Список WireElement, в которые утекает ток.
     */
    List<WireElement> GetMinusWireElements();
}

/**
 Суперкласс всех элементов электрической схемы ElectricalCircuit
@see ElectricalCircuit; ElectricityTransfer;
 */
public class ElectricalElement : MonoBehaviour
{
    /// Имя элемента Источник питания
    public const string POWER_SUPPLY = "PowerSupply";
    /// Имя элемента Вольтметр
    public const string VOLTMETER = "Voltmeter";
    /// Имя элемента Амперметр
    public const string AMPERMETER = "Ampermeter";
    /// Имя элементов Резистор и Реостат
    public const string RESISTOR_AND_RHEOSTAT = "Resistor";
    /// Имя элемента Мультиклемма
    public const string MULTY_CLEMA = "MultiClema";
    /// Имя элемента Провод
    public const string WIRE = "WireElement";

    /// Электрическая цепь, в которой находится данный элемент
    protected ElectricalCircuit _electricalCircuit;
    /// Элементы, которые подключены к данному элементу
    private List<ElectricalElement> _connectedElements;
    /// Имя данного элемента
    protected string _name;

    protected void Awake()
    {
        _connectedElements = new List<ElectricalElement>();
        _electricalCircuit = null;
    }

    /**
     Геттер имени данного элемента.
    @return string Имя данного элемента.
     */
    public string GetName()
    {
        return _name;
    }

    /**
     Геттер элементов, подключенных к данному элементу
    @return List<ElectricalElement> подключенных к данному элементу.
     */
    public List<ElectricalElement> GetConnectedElectricalElements()
    {
        return _connectedElements;
    }

    /**
     Сеттер электрической цепи, в которой находится данный элемент.
    @param [in] electricalCircuit ElectricalCircuit , в которой находится данный элемент.
     */
    public void SetElectricCircuit(ElectricalCircuit electricalCircuit)
    {
        _electricalCircuit = electricalCircuit;
    }

    /**
     Добавление элемента, подключенного к данному элементу.
    @param [in] electricalElement Добавляемый ElectricalElement.
     */
    public void AddConnectedElement(ElectricalElement electricalElement)
    {
        if (!_connectedElements.Contains(electricalElement))
        {
            _connectedElements.Add(electricalElement);
            _electricalCircuit?.RecalculateCircuit();
        }
    }

    /**
     Удаление элемента из списка элементов, подключенных к данному элементу.
    @param [in] electricalElement Удаляемый ElectricalElement.
     */
    public void RemoveConnectedElement(ElectricalElement electricalElement)
    {
        if (_connectedElements.Contains(electricalElement))
        {
            _connectedElements.Remove(electricalElement);
            _electricalCircuit?.RecalculateCircuit();
        }
    }

    /// Удалить ссылку на электрическую цепь, в которой находится данный элемент.
    public void RemoveEctricalCircuitLink()
    {
        _electricalCircuit = null;
    }

    /**
     Геттер электрической цепи, в которой находится данный элемент.
    @return ElectricalCircuit , в которой находится данный элемент.
     */
    public ElectricalCircuit GetelEctricalCircuit()
    {
        return _electricalCircuit ? _electricalCircuit : null;
    }
}
