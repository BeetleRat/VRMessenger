using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;

public class MicrophoneController : MonoBehaviour
{
    [SerializeField] private Image _microphoneOnImage;
    [SerializeField] private Image _microphoneOffImage;
    [SerializeField] private Recorder _microphone;

    private bool isMicrophoneActive;
    private void Start()
    {

        isMicrophoneActive = true;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        _microphone.RecordingEnabled = isMicrophoneActive;
    }

    public void SwitchMicrophoneActivity()
    {
        isMicrophoneActive = !isMicrophoneActive;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        _microphone.RecordingEnabled = isMicrophoneActive;
    }

    public void ChangeMicrophoneVolume(int newVolume)
    {
       
    }
}
