using System.Collections.Generic;
using UnityEngine;


/**
 Класс возвращающий объект в стартовое положение, если объект упал на пол.

Полом является объект имеющий компонент Floor.
@attention Объект или его потомки должны иметь коллайдер.
@param returnSpeed Скорость, с которой объект вернется в исходное положение.
@param bindObject Объекты BackFromFloor, которые должны вернуться на свои места вместе с данным объектом.
Это нужно, если у нас сложный объект, в котором несколько независимых/частично зависимых частей.
И если одна из таких частей упала на пол - значит необходимо вернуть весь объект в исходное положение.
А значит необходимо вернуть в исходное положение все связанные с этим объектом части.
 */
[RequireComponent(typeof(Rigidbody))]
public class BackFromFloor : MonoBehaviour
{
    [SerializeField] private float _returnSpeed = 1f;
    [SerializeField] private List<BackFromFloor> _bindObjects;

    private Vector3 _startPosition;
    private Vector3 _startRotation;
    private Rigidbody _rigidBody;
    private List<Collider> _colliders;

    private bool _isBackToStartPosition;

    private void Start()
    {
        _isBackToStartPosition = false;
        _startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _startRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        _rigidBody = GetComponent<Rigidbody>();
        _colliders = new List<Collider>();
        _colliders.AddRange(GetComponentsInChildren<Collider>());
        if (TryGetComponent(out Collider collider))
        {
            _colliders.Add(collider);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor))
        {
            BackToStartPosition();
        }
    }

    /// Метод для возвращения объекта в исходное положение
    public void BackToStartPosition()
    {
        if (!_isBackToStartPosition)
        {
            _isBackToStartPosition = true;
            foreach (BackFromFloor bindObject in _bindObjects)
            {
                bindObject.BackToStartPosition();
            }
            SetColliderState(false);
            _rigidBody.isKinematic = true;
            transform.LeanMove(_startPosition, _returnSpeed).setOnComplete(() =>
            {
                _isBackToStartPosition = false;
                SetColliderState(true);
                _rigidBody.isKinematic = false;
            });
            transform.LeanRotate(_startRotation, _returnSpeed);
        }
    }

    private void SetColliderState(bool isActive)
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = isActive;
        }
    }
}
