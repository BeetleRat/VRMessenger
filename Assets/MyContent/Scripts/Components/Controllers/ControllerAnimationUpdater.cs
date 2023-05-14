using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Класс обеспечивающий проигрывание анимации контроллеров Oculus на сервере

Данный класс обновляет анимацию моделей контроллера на сервере, в зависимости от нажатых на локальном контроллере кнопок/стиков/триггеров.
@bug При возвращении из комнаты в лобби возникает NullPointerException, связанный с данным классом.
 */
public class ControllerAnimationUpdater : ControllerModel
{
    /// Аниматор, который используется для анимации модели контроллера.
    [SerializeField] private Animator _animator;

    private void Update()
    {
        UpdateAnimation(_animator);
    }

    private void UpdateAnimation(Animator animator)
    {
        if (animator != null && _myPhotonView.IsMine)
        {
            animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, _controllerType) ? 1.0f : 0.0f);
            animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, _controllerType) ? 1.0f : 0.0f);
            animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, _controllerType) ? 1.0f : 0.0f);

            animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controllerType).x);
            animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controllerType).y);

            animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _controllerType));
            animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controllerType));
        }
    }

}
