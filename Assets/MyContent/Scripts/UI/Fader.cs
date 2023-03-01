using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private const string FADER_PATH = "Fader";
    [SerializeField] private Animator _faderAnimator;
    private static Fader _instance;
    public static Fader instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<Fader>(FADER_PATH);
                _instance = Instantiate(prefab);
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public bool IsFading { get; private set; }
    private Action _fadeInCallback;
    private Action _fadeOutCallback;

    private void Awake()
    {
        IsFading = false;
    }

    public void FadeIn(Action fadeInCallback)
    {
        if (IsFading)
        {
            return;
        }
        IsFading = true;
        this._fadeInCallback = fadeInCallback;
        _faderAnimator.SetBool("isFaded", true);
    }
    public void FadeOut(Action fadeOutCallback)
    {
        if (IsFading)
        {
            return;
        }
        IsFading = true;
        this._fadeOutCallback = fadeOutCallback;
        _faderAnimator.SetBool("isFaded", false);
    }

    private void Handle_FadeInAnimationOver()
    {
        _fadeInCallback?.Invoke();
        _fadeInCallback = null;
        IsFading = false;
    }

    private void Handle_FadeOutAnimationOver()
    {
        _fadeOutCallback?.Invoke();
        _fadeOutCallback = null;
        IsFading = false;
    }
}
