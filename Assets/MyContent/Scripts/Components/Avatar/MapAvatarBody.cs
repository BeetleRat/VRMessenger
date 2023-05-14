using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Класс для установки положения и поворота кости скелета, в положение цели.
[System.Serializable]
public class MapRigTransform
{
    /// Положение, в которое будет установлена указанная кость.
    public Transform Target;
    /// Кость скелета, которая будет установлена в указанное положение.
    public Transform Rig;

    /// Отступ положения кости от цели.
    public Vector3 TrackingPositionOffset;
    /// Отступ поворота кости от цели
    public Vector3 TrackingRotationOffset;

    /// Установить кость в указанное положение с учетом отступов.
    public void MapRig()
    {
        Rig.position = Target.TransformPoint(TrackingPositionOffset);
        Rig.rotation = Target.rotation * Quaternion.Euler(TrackingRotationOffset);
    }
}

/**
 Класс, отвечающий за синхронизацию положения тела аватара, с управляющими элементами.

Данный класс связывает отображаемую модель аватара и положение шлема, контроллеров и рук.
Без данного класса модель заспавнится в случайном месте и будет тянуться к скелету.

@param head MapRigTransform для контроллера головы и кости головы.
@param rightHand MapRigTransform для контроллера правой руки и кости правой руки.
@param leftHand MapRigTransform для контроллера левой руки и кости левой руки.
@param bodyOffset Отступ отображаемого тела от центральной камеры шлема.
@param turningSmoothness Плавность поворота тела вслед за головой.
@see MapRigTransform
*/
public class MapAvatarBody : MonoBehaviour
{
    [SerializeField] private MapRigTransform _head;
    [SerializeField] private MapRigTransform _rightHand;
    [SerializeField] private MapRigTransform _leftHand;

    [SerializeField] private float _turningSmoothness;
    [SerializeField] private Vector3 _bodyOffset;

    private void LateUpdate()
    {
        transform.position = _head.Rig.position + _bodyOffset;

        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(_head.Rig.forward, Vector3.up).normalized, Time.deltaTime * _turningSmoothness);

        _head.MapRig();
        _rightHand.MapRig();
        _leftHand.MapRig();
    }
}
