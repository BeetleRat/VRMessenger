using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Realtime;

public class MicrophoneController : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    [SerializeField] private Image _microphoneOnImage;
    [SerializeField] private Image _microphoneOffImage;
    [SerializeField] private Recorder _microphone;

    private bool isMicrophoneActive;
    private float _currentMicrophoneVolume;
    private NetworkVariables _networkVariables;


    private void Start()
    {
        _networkVariables = _catcher.GetNetworkVariables();
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
        }
        isMicrophoneActive = true;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        _microphone.RecordingEnabled = isMicrophoneActive;
        _currentMicrophoneVolume = 1.0f;
    }

    private void OnDestroy()
    {
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
        }
    }

    public void SwitchMicrophoneActivity()
    {
        isMicrophoneActive = !isMicrophoneActive;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        _microphone.RecordingEnabled = isMicrophoneActive;
    }

    public void ChangeMicrophoneVolume(Slider slider)
    {
        _currentMicrophoneVolume = (float)slider.value;
        NetworkVariables.SendPropertyToServer(PlayersProperty.MICROPHONE_VOLUME, _currentMicrophoneVolume);
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayersProperty.UPDATE_STATUS))
        {
            NetworkVariables.SendPropertyToServer(PlayersProperty.MICROPHONE_VOLUME, _currentMicrophoneVolume);
        }
    }
}
