using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;
using Photon.Realtime;

/**
Класс взаимодействия с компонентами воспроизводящими звук с микрофона на сервере

Данный класс дополняет класс MicrophoneController. Он обрабатывает запросы микрофона пришедшие на сервер.

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- ComponentCatcher;
- NetworkVariables;
@see ComponentCatcher; NetworkVariables 
 */
[RequireComponent(typeof(Speaker)), RequireComponent(typeof(AudioSource))]
public class MicrophoneNetworkSettings : MonoBehaviour
{
    [SerializeField] private PhotonView _myPhotonView;

    private Speaker _speaker;
    private AudioSource _audioSource;
    private NetworkVariables _networkVariables;

    private void Start()
    {
        ComponentCatcher catcher = FindObjectOfType<ComponentCatcher>();
        _networkVariables = catcher?.GetNetworkVariables();
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
        }
        _speaker = GetComponent<Speaker>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
        }
    }

    private void ChangeVolume(float newVolume)
    {
        _audioSource.volume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayersProperty.MICROPHONE_VOLUME))
        {
            if (targetPlayer == _myPhotonView.Owner)
            {
                ChangeVolume((float)changedProps[PlayersProperty.MICROPHONE_VOLUME]);
            }
        }
    }
}
