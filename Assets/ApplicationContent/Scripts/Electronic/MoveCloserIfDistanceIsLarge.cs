using UnityEngine;
using Oculus.Interaction;

/**
 Компонент для синхронизации относительного положения концов провода

Данный класс используется для разрешения ситуаций, когда оба конца провода isKinematic.
Это может произойти, когда один конец провода у нас в руках, а другой прикреплен к клемме.
В таком случае необходимо открепить второй конец провода от клеммы и притянуть его поближе к первому концу.

Данный класс так же поддерживает определенное расстояние maxDistance между данным объектом и объектом dependentObject.
Это необходимо для избегания глитчей провода. Если текущее расстояние между объектами больше указанного, 
и данный объект игрок держит в руке, то dependentObject будет притягиваться к данному объекту со скоростью speed.

@note Данный компонент стоит применять на оба объекта, которые предполагается притягивать.

@note Притяжение к данному объекту будет действовать только если игрок взял этот объект в руки.

@param dependentObject Объект, который будет притягиваться к данному объекту.
@param maxDistance Расстояние, превышая которое, dependentObject начинает притягиваться к данному объекту.
@param speed Скорость, с которой dependentObject притягивается к данному объекту
@see WireOutput; WireInput
 */
[RequireComponent(typeof(Grabbable))]
public class MoveCloserIfDistanceIsLarge : MonoBehaviour
{
    [SerializeField] Transform _dependentObject;
    [SerializeField] private float _maxDistance = -1f;
    [SerializeField] private float _speed = 1f;

    private bool _isGrab;
    private Grabbable _grabbable;

    private void Start()
    {
        if (_maxDistance < 0f)
        {
            _maxDistance = Vector3.Distance(transform.position, _dependentObject.position);
        }
        _grabbable = GetComponent<Grabbable>();
        _grabbable.WhenPointerEventRaised += OnObjectGrabChange;
        _isGrab = false;
    }

    private void Update()
    {
        if (_isGrab)
        {
            if (Vector3.Distance(transform.position, _dependentObject.position) > _maxDistance)
            {
                if (_dependentObject.TryGetComponent(out WireOutput output))
                {
                    output.GetConnectedObject()?.RemoveWireFromInput();
                }
                _dependentObject.position = Vector3.MoveTowards(_dependentObject.position, transform.position, _speed * Time.deltaTime);
            }
        }
    }

    private void OnDestroy()
    {
        _grabbable.WhenPointerEventRaised -= OnObjectGrabChange;
    }

    private void OnObjectGrabChange(PointerEvent grabEvent)
    {
        switch (grabEvent.Type)
        {
            case PointerEventType.Select:
                if (!_isGrab)
                {
                    _isGrab = true;
                }
                break;
            case PointerEventType.Unselect:
                if (_isGrab)
                {
                    _isGrab = false;
                }
                break;
        }
    }
}
