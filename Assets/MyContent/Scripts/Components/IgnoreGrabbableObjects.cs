using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
