using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
### Скрипт позволяющий коллайдеру игроки игнорировать коллайдеры интерактивных объектов

При соприкосновении с коллайдером другого объекта, данный скрипт проверяет, 
является ли объект, с которым мы соприкоснулись, объектом, который можно взять. 
И если является, то данный скрипт начинает игнорировать данный коллайдер.
Это позволяет игроку имеющему коллайдер свободно держать в руках интерактивные объекты, не сталкиваясь с ними. 
 */
[RequireComponent(typeof(Collider))]
public class IgnoreGrabbableObjects : MonoBehaviour
{    
    private Collider _myCollider;

    private void Start()
    {
        _myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Oculus.Interaction.Grabbable grabbableObject))
        {
            Physics.IgnoreCollision(collision.collider, _myCollider);
        }
    }
}
