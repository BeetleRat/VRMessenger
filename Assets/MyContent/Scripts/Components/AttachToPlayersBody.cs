using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayersBody : MonoBehaviour
{
    [SerializeField] private Transform _bodyPart;


    private void Start()
    {
        gameObject.transform.parent = _bodyPart;
    }
}
