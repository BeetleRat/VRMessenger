using UnityEngine;

/**
Компонент, обозначающий данный объект местом для крепления провода

Концы провода содержат компонент WireOutput. Именно он и прикрепляется к данному объекту.

Объекты, содержащие данный компонент, используются для проведения тока, идущего по проводу WireElement
к ElectricalElement и наоборот.

@param plugPosition Позиция, в которую будет прикреплен провод.
@param unplugPosition Позиция, в которой окажется провод, при откреплении. 
Данную позицию нужно выбрать так, чтобы при попадании в нее коллайдер провода не соприкасался с коллайдером данного объекта.
@param breakingDistance Расстояние, на которое должен отклониться провод от plugPosition, чтобы считать, что он откреплен.
@param connectedAudio Опциональный параметр. AudioSource воспроизводимый при прикреплении провода.
@param disconnectedAudio Опциональный параметр. AudioSource воспроизводимый при откреплении провода.
@see WireOutput; ElectricalElement
 */
public class WireInput : MonoBehaviour
{
    [SerializeField] private Transform _plugPosition;
    [SerializeField] private Transform _unplugPosition;
    [SerializeField] private float _breakingDistance = 0.02f;
    [Header("[Optional]")]
    [SerializeField] private AudioSource _connectedAudio;
    [Header("[Optional]")]
    [SerializeField] private AudioSource _disconnectedAudio;

    private bool _isConnected = false;
    private WireOutput _connectedWire;
    private ElectricalElement _parent;

    private void Update()
    {
        if (_connectedWire != null)
        {
            if (Vector3.Distance(_connectedWire.transform.position, _plugPosition.position) > _breakingDistance)
            {
                RemoveWireFromInput();
            }
            else
            {
                _connectedWire.transform.position = new Vector3(_plugPosition.position.x, _plugPosition.position.y, _plugPosition.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isConnected)
        {
            if (other.gameObject.TryGetComponent(out WireOutput wire))
            {
                ConnectWire(wire);
            }
        }
    }

    /// Открепить провод от объекта
    public void RemoveWireFromInput()
    {
        if (_isConnected && _connectedWire)
        {
            DisconnectWire(_connectedWire);
        }
    }

    /** 
     Получить WireOutput прикрепленный к данному объекту
    @return WireOutput прикрепленный к данному объекту либо null, 
    если к данному объекту не прикреплен провод.
     */
    public WireOutput GetConnectedWire()
    {
        return _connectedWire ? _connectedWire : null;
    }

    private void ConnectWire(WireOutput wire)
    {
        _isConnected = true;
        _connectedWire = wire;
        if (_connectedWire.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = _isConnected;
            rigidbody.useGravity = !_isConnected;
        }
        wire.SetConnectedObject(this);
        if (_connectedAudio != null && _connectedAudio.enabled)
        {
            _connectedAudio?.PlayOneShot(_connectedAudio?.clip);
        }
        _parent?.AddConnectedElement(wire.GetWire());
        wire.GetWire().AddConnectedElement(_parent);
        _connectedWire.transform.position = new Vector3(_plugPosition.position.x, _plugPosition.position.y, _plugPosition.position.z);
    }

    private void DisconnectWire(WireOutput wire)
    {
        _isConnected = false;
        wire.SetConnectedObject(null);
        _connectedWire.transform.position = new Vector3(_unplugPosition.position.x, _unplugPosition.position.y, _unplugPosition.position.z);
        if (wire.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = _isConnected;
            rigidbody.useGravity = !_isConnected;
        }
        _connectedWire = null;
        if (_disconnectedAudio != null && _disconnectedAudio.enabled)
        {
            _disconnectedAudio?.PlayOneShot(_disconnectedAudio?.clip);
        }
        if (wire.GetOutputType() == OutputType.Minuse)
        {
            wire.SetOutputTypeForWholeWire(OutputType.None);
        }
        wire.GetWire().RemoveConnectedElement(_parent);
        _parent?.RemoveConnectedElement(wire.GetWire());
    }

    /**
     Сеттер ElectricalElement, который использует данное место крепления провода.

    @param [in] electricalElement ElectricalElement, который использует данное место крепления провода
     */
    public void SetParent(ElectricalElement electricalElement)
    {
        _parent = electricalElement;
    }
}
