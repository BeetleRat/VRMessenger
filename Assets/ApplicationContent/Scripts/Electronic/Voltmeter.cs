using System.Collections.Generic;
using UnityEngine;

/**
Класс вольтметра.

По данному классу элемент в электронной цепи определяется как Вольтметр.

Логика работы.

К вольтметру должны быть подключены провода WireElement.
Данные провода одним концом должны быть подключены к входам вольтметра,
другим концом к parallelInput MultiClema.

\image html VoltmetrSchema.png width=500
<div style = "text-align: center;" >
    Схема подключения вольтметра
</div>
Вольтметр будет подсчитывать сопротивление в цепи следующим образом.
Он начнет обходить провод по направлению тока, и суммировать сопротивление на резисторах (Resistor),
до тех пор, пока не найдет первую MultiClema.Дальше он пойдет по проводу подключенному к leftInput MultiClema.
Он так же суммирует сопративление на резисторах (Resistor), до тех пор, пока не найдет вторую MultiClema.
Теперь он пойдет по проводу подключенному к parallelInput MultiClema, т.к.вольтметр должен крепиться к этому входу MultiClema.
Вольтметр продолжает суммировать сопративление, пока не найдет себя. После этого он подсчитывает напряжение по закону Ома: U= A\*R.
Полученное напряжение выводится на шкалу ScaleWithPointer вольтметра.
@note Что бы вольтметр работал, он должен присутствовать в замкнутой электронной цепи ElectricalCircuit.
@param pluse WireInput вольтметра, из которого должен выходить ток.
Если в данный вход будет входить ток, то вольтметр будет показывать отрицательные значения.
@param minus WireInput вольтметра, в который должен входить ток.
Если из данного входа будет выходить ток, то вольтметр будет показывать отрицательные значения.
@param pointer Шкала измерений данного вольтметра ScaleWithPointer.
@see WireInput; ScaleWithPointer; ElectricalElement; ElectricityTransfer; WireElement; MultiClema; Resistor; ElectricalCircuit
*/
public class Voltmeter : ElectricalElement, ElectricityTransfer
{
    [SerializeField] private WireInput _pluse;
    [SerializeField] private WireInput _minus;
    [SerializeField] private ScaleWithPointer _pointer;
    private int _multiplier;
    private float _circuitCectionResistance;

    /**
     Эта функция всегда вызывается до начала любых функций, а также сразу после инициализации prefab-а.
    В данной функции происходит инициализация параметров электрического элемента и специфических для вольтметра параметров.
     */
    protected new void Awake()
    {
        base.Awake();
        _name = VOLTMETER;
        _pluse.SetParent(this);
        _minus.SetParent(this);
        _multiplier = 0;
    }

    private void Update()
    {
        if (_electricalCircuit != null && _electricalCircuit.IsCircuitClosed())
        {
            _circuitCectionResistance = 0;
            FindFirstMultiClema(this, _pluse.GetConnectedWire().GetWire());
        }
        else
        {
            _pointer.SetCurrentValue(0);
        }
        UpdateElectricityStatus();
    }

    private void FindFirstMultiClema(ElectricalElement from, ElectricalElement to)
    {
        if (to == null)
        {
            return;
        }
        MultiClema multiClema = to as MultiClema;
        if (multiClema != null)
        {
            FindLastMultiClema(multiClema, multiClema.GetLeftConnectedWire());
        }
        else
        {
            Resistor resistor = to as Resistor;
            if (resistor != null)
            {
                _circuitCectionResistance += resistor.GetResistence();
            }
            foreach (ElectricalElement electricalElement in to.GetConnectedElectricalElements())
            {
                if (electricalElement != from)
                {
                    FindFirstMultiClema(to, electricalElement);
                    break;
                }
            }
        }
    }

    private void FindLastMultiClema(ElectricalElement from, ElectricalElement to)
    {
        if (to == null)
        {
            return;
        }
        MultiClema multiClema = to as MultiClema;
        if (multiClema != null)
        {
            FindMyself(multiClema, multiClema.GetParallelConnectedWire());
        }
        else
        {
            Resistor resistor = to as Resistor;
            if (resistor != null)
            {
                _circuitCectionResistance += resistor.GetResistence();
            }
            foreach (ElectricalElement electricalElement in to.GetConnectedElectricalElements())
            {
                if (electricalElement != from)
                {
                    FindLastMultiClema(to, electricalElement);
                    break;
                }
            }
        }
    }

    private void FindMyself(ElectricalElement from, ElectricalElement to)
    {
        if (to == null)
        {
            return;
        }
        if (to == this)
        {
            _pointer.SetCurrentValue(_electricalCircuit.GetAmperage() * _circuitCectionResistance * _multiplier);
        }
        else
        {
            Resistor resistor = to as Resistor;
            if (resistor != null)
            {
                _circuitCectionResistance += resistor.GetResistence();
            }
            foreach (ElectricalElement electricalElement in to.GetConnectedElectricalElements())
            {
                if (electricalElement != from)
                {
                    FindMyself(to, electricalElement);
                    break;
                }
            }
        }
    }

    /**
     Установить множитель показаний вольтметра.
    
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
