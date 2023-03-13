using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Кривой багфикс двигания плеером самого себя через взятый в руки объект
[RequireComponent(typeof(Collider))]
public class IgnorPlayerCollider : MonoBehaviour
{
    
    private Collider _objectCollider;
    private Vector3 _startPosition;
    private Vector3 _startRotation;
    [Range(-10000000,-10)]
    [SerializeField] private int _respawnDepth;

    private void Start()
    {
        _startPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        _startRotation = new Vector3(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
        _objectCollider = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SimpleCapsuleWithStickMovement capsuele))
        {
            _objectCollider.isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out SimpleCapsuleWithStickMovement capsuele))
        {
            _objectCollider.isTrigger = false;
        }
    }
    private void Update()
    {
        if (gameObject.transform.position.y < _respawnDepth && _startPosition.y> _respawnDepth)
        {
            _objectCollider.isTrigger = false;
            transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z);
            transform.rotation = Quaternion.Euler(_startRotation);
        }
    }
}
