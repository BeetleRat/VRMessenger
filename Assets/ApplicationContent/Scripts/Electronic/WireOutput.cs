using UnityEngine;

/// Направление тока на данном выходе провода
public enum OutputType
{
    None, ///< Ток не идет по проводу
    Pluse, ///< Ток выходит из данного конца провода
    Minuse ///< Ток входит в данный конец провода
};

/**
 Компонент конца провода

Данный компонент крепиться на концы провода WireElement. WireElement управляет данным компонентом.
Данный компонент прикрепляется к WireInput.
@see WireInput; WireElement; OutputType
 */
public class WireOutput : MonoBehaviour
{
    private WireInput _connectedObject;
    private WireElement _wire;
    private OutputType _outputType;

    /**
     Установить объект, к которому подключен данный конец провода.
    @param [in] connectedObject WireInput к которому подключен данный конец провода.
     */
    public void SetConnectedObject(WireInput connectedObject)
    {
        _connectedObject = connectedObject;
    }

    /**
    Установить провод, одним из концов которого является данный объект.
    @param [in] wire WireElement, одним из концов которого является данный объект.
     */
    public void SetWire(WireElement wire)
    {
        _wire = wire;
    }

    /**
     Установить течение тока для всего провода.
     @param [in] outputType OutputType данного конца провода.
    Второй конец провода будет установлен в противоположное направление.
     */
    public void SetOutputTypeForWholeWire(OutputType outputType)
    {
        _outputType = outputType;
        _wire.UpdeteWireType(outputType, this);
    }

    /**
     Установить течение тока только для данного конца провода.
     @param [in] outputType OutputType данного конца провода.
    Второй конец провода останется неизменным.
     */
    public void SetOutputTypeToOneOutput(OutputType outputType)
    {
        _outputType = outputType;
    }

    /** 
     Получить WireInput к которому прикреплен данный объект
    @return WireInput к которому прикреплен данный объект либо null, 
    если данный объект не прикреплен к WireInput.
     */
    public WireInput GetConnectedObject()
    {
        return _connectedObject ? _connectedObject : null;
    }

    /** 
     Получить провод, концом которого является данный объект
    @return WireElement, концом которого является данный объект.
     */
    public WireElement GetWire()
    {
        return _wire;
    }

    /** 
     Получить направление тока на данном конце провода.
    @return OutputType, данного конца провода.
     */
    public OutputType GetOutputType()
    {
        return _outputType;
    }
}
