using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InterfaceHider : MonoBehaviour
{
    [SerializeField] private float _hideDuration;

    private Vector3 _currentScale;

    private bool _isHide;
    private void Awake()
    {
        _isHide = false;
        _currentScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //HideInterface();
    }
    public void HideInterface()
    {
        if (!_isHide)
        {
            _isHide = true;
            transform.LeanScale(Vector3.zero, _hideDuration).setEaseInBack();
            //.setOnComplete(() => gameObject.SetActive(false));

        }
    }
    public void ShowInterface()
    {
        if (_isHide)
        {
            _isHide = false;
            //gameObject.SetActive(true);
            transform.LeanScale(_currentScale, _hideDuration);
        }
    }
}
