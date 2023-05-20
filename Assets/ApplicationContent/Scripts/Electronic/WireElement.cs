using UnityEngine;

/**
 Класс провода

@param output1 Первый конец провода.
@param output1 Второй конец провода.
@see WireOutput; ElectricalElement; OutputType
 */
public class WireElement : ElectricalElement
{
    [SerializeField] private WireOutput _output1;
    [SerializeField] private WireOutput _output2;

    protected new void Awake()
    {
        base.Awake();
        _name = WIRE;
        _output1.SetWire(this);
        _output2.SetWire(this);
    }

    /**
     Обновить направление тока в проводе.

    Данный метод вызывается одним из концов провода WireOutput.
    Метод устанавливает второму концу провода направление, 
    противоположное направлению в конце провода, вызвавшего данный метод.
    @param [in] outputType OutputType конца провода, который вызвал данный метод.
    @param [in] sender WireOutput вызвавший данный метод.
     */
    public void UpdeteWireType(OutputType outputType, WireOutput sender)
    {
        if (outputType == OutputType.None)
        {
            _output1.SetOutputTypeToOneOutput(OutputType.None);
            _output2.SetOutputTypeToOneOutput(OutputType.None);
        }
        else
        {
            if (sender == _output1)
            {
                if (outputType == OutputType.Pluse)
                {
                    _output2.SetOutputTypeToOneOutput(OutputType.Minuse);
                }
                else
                {
                    _output2.SetOutputTypeToOneOutput(OutputType.Pluse);
                }
            }
            else
            {
                if (outputType == OutputType.Pluse)
                {
                    _output1.SetOutputTypeToOneOutput(OutputType.Minuse);
                }
                else
                {
                    _output1.SetOutputTypeToOneOutput(OutputType.Pluse);
                }
            }
        }
    }
}
