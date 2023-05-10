using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

/**
Скрипт, прикрепляющий объект к якорю весящему на игроке

Данный класс используется для того, чтобы объекты, которые прикрепляются к игроку не приходилось долго искать на самом игроке. 
Идея в следующем: 
1. В нужную(-ые) части тела игрока мы прикрепляем EmptyObject якорь;
2. Создаем нужный нам объект, где нам будет угодно;
3. Добавляем на объект данный скрипт и в его полях указываем якорь, к которому объект будет прикреплен при запуске приложения;
4. *Если объект прикрепляется к рукам, то необходимо указать отдельный якорь для контроллеров и для рук*

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ControllerEvents;

@param catcher ComponentCatcher данной сцены.
@param controllerBodyPart Якорь, к которому прикрепляется объект. 
Если объект прикрепляется к рукам, то в данный параметр указывается якорь для прикрепления к контроллерам
@param handBodyPart (*Не обязательно*) Если объект прикрепляется к рукам, то в данный параметр указывается якорь для прикрепления к рукам
@see ComponentCatcher; ControllerEvents
 */
public class AttachToPlayersBody : MonoBehaviour
{
    [SerializeField] private Transform _controllerBodyPart;
    [SerializeField, Optional] private Transform _handBodyPart;

    [SerializeField] private ComponentCatcher _catcher;

    private Vector3 _currentPosition;
    private Vector3 _currentRotation;

    private ControllerEvents _controllerEvents;

    private void Start()
    {
        _controllerEvents = _catcher.GetControllerEvents();
        if (_controllerEvents != null)
        {
            _controllerEvents.ControllerTypeChange += OnAttachChange;           
        }

        _currentPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        _currentRotation = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        gameObject.transform.parent = _controllerBodyPart;

        OnAttachChange(!OVRPlugin.GetHandTrackingEnabled());

        if (_handBodyPart == null)
        {
            _handBodyPart = _controllerBodyPart;
        }
    }

    private void OnDestroy()
    {
        if (_controllerEvents != null)
        {
            _controllerEvents.ControllerTypeChange -= OnAttachChange;
        }
    }

    private void OnAttachChange(bool isAttachToController)
    {
        if (isAttachToController)
        {
            gameObject.transform.parent = _controllerBodyPart;
            RestoreLocalTransform();
        }
        else
        {
            gameObject.transform.parent = _handBodyPart;
            RestoreLocalTransform();
        }
    }

    private void RestoreLocalTransform()
    {
        transform.localPosition = new Vector3(_currentPosition.x, _currentPosition.y, _currentPosition.z);
        transform.localRotation = Quaternion.Euler(new Vector3(_currentRotation.x, _currentRotation.y, _currentRotation.z));
    }
}
