using System.Collections.Generic;
using UnityEngine;

/**
 Класс электрического тока в цепи.
 */
[System.Serializable]
public class Electricity
{
    /// Сила тока.
    public float Amperage;
    /// Напряжение.
    public float Voltage;

    /**
     Скопировать параметры другого класса электричества.
    @param [in] electricity Класс электрического тока параметры которого необходимо скопировать.
     */
    public void CopyElectricity(Electricity electricity)
    {
        Amperage = electricity.Amperage;
        Voltage = electricity.Voltage;
    }
}

/**
 Класс источника электрического тока.

Источник тока постоянно посылает сгенерированный им ток на minus выход.
@param generatedElectricity Electricity порождаемое данным источником тока.
@param pluse WireInput источника тока, в который должен входить ток.
@param minus WireInput источника тока, из которого выходит ток.
@see Electricity; WireInput; ElectricalElement; ElectricityTransfer; WireElement;
 */
public class PowerSupply : ElectricalElement, ElectricityTransfer
{
    [SerializeField] private Electricity _generatedElectricity;
    [SerializeField] private WireInput _pluse;
    [SerializeField] private WireInput _minus;

    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для источника электрического тока параметров.
     */
    protected new void Awake()
    {
        base.Awake();
        _name = POWER_SUPPLY;
        _pluse.SetParent(this);
        _minus.SetParent(this);
    }

    private void Update()
    {
        UpdateElectricityStatus();
    }

    /**
     Получить силу тока, генерируемую данным источником тока.
    @return float Сила тока, генерируемая данным источником тока.
     */
    public float GetAmperage()
    {
        return _generatedElectricity.Amperage;
    }

    /**
     Получить напряжение, генерируемое данным источником тока.
    @return float Напряжение, генерируемое данным источником тока.
     */
    public float GetVoltage()
    {
        return _generatedElectricity.Voltage;
    }

    /**
     Получить провод, подключенный к minus выходу источника электрического тока.
    @return WireElement, подключенный к minus выходу источника электрического тока.
     */
    public WireElement GetMinusConnectedWire()
    {
        return _minus.GetConnectedWire()?.GetWire();
    }

    /// Реализация метода интерфейса.
    public List<WireElement> GetMinusWireElements()
    {
        List<WireElement> wireElements = new List<WireElement>();
        if (_minus.GetConnectedWire() != null)
        {
            wireElements.Add(_minus.GetConnectedWire().GetWire());
        }

        return wireElements;
    }

    private void UpdateElectricityStatus()
    {
        _minus.GetConnectedWire()?.SetOutputTypeForWholeWire(OutputType.Minuse);
    }
}
