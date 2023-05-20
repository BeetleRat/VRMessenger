using UnityEngine;


/**
 Класс, проигрывающий указанный AudioSource при изменении положения/поворота объекта

@param soundOfMoving AudioSource воспроизводимый при изменении положения объекта
@param soundOfRotation AudioSource воспроизводимый при изменении поворота объекта
@param positionSoundDistance Расстояние, через которое будет проигрываться звук. 
Каждый раз, когда объект проходит данное расстояние, относительно предыдущего положения воспроизведения звука или начального положения, воспроизводится звук soundOfMoving.
@param rotationSoundAngle Угол, при повороте на который воспроизводится звук.
Каждый раз, когда объект совершает поворот, относительно предыдущего положения воспроизведения звука или начального положения, воспроизводится звук rotationSoundAngle.
 */
public class PlayAudioWhenObjectMove : MonoBehaviour
{
    [SerializeField] private AudioSource _soundOfMoving;
    [SerializeField] private AudioSource _soundOfRotation;

    [SerializeField] float _positionSoundDistance;
    [SerializeField] float _rotationSoundAngle;

    private Vector3 _previousPosition;
    private Vector3 _previousRotation;

    private void Start()
    {
        _previousPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        _previousRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (_soundOfMoving)
        {
            float distance = Vector3.Distance(transform.localPosition, _previousPosition);
            if (distance > _positionSoundDistance)
            {
                _soundOfMoving.PlayOneShot(_soundOfMoving.clip);
                _previousPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }
        }

        if (_soundOfRotation)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            if (Mathf.Abs(_previousRotation.x - currentRotation.x) > _rotationSoundAngle)
            {
                _soundOfRotation.PlayOneShot(_soundOfRotation.clip);
                _previousRotation.x = currentRotation.x;
            }
            else
            {
                if (Mathf.Abs(_previousRotation.y - currentRotation.y) > _rotationSoundAngle)
                {
                    _soundOfRotation.PlayOneShot(_soundOfRotation.clip);
                    _previousRotation.y = currentRotation.y;
                }
                else
                {
                    if (Mathf.Abs(_previousRotation.z - currentRotation.z) > _rotationSoundAngle)
                    {
                        _soundOfRotation.PlayOneShot(_soundOfRotation.clip);
                        _previousRotation.z = currentRotation.z;
                    }
                }
            }
        }
    }
}
