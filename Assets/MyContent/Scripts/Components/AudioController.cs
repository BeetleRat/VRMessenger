using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 ### Класс, манипулирующий звуками сцены

@param controlledSources Массив AudioSource сцены, над которыми будут производиться манипуляции.
 */
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _controlledSources;

    private bool _isMute;

    private void Start()
    {
        _isMute = false;
        SetMuteToAll();
    }

    /**
     Вкл/выкл конкретный аудио источник.
    @param [in] index Индекс переключаемого источника
     */
    public void SwitchMute(int index)
    {
        if (index >= 0 && index < _controlledSources.Length)
        {
            _controlledSources[index].mute = !_controlledSources[index].mute;
        }
    }

    /// Вкл/выкл все контролируемые аудио источники.
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
