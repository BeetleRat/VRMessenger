using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _controlledSources;

    private bool _isMute;
    private void Start()
    {
        _isMute = false;
        SetMuteToAll();
    }

    public void SwitchMute(int index)
    {
        if (index >= 0 && index < _controlledSources.Length)
        {
            _controlledSources[index].mute = !_controlledSources[index].mute;
        }
    }

    public void SwitchMuteToAll()
    {
        _isMute = !_isMute;
        SetMuteToAll();
    }

    private void SetMuteToAll()
    {
        foreach (AudioSource audioSource in _controlledSources)
        {
            audioSource.mute = _isMute;
        }
    }
}
