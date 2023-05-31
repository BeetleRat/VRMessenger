using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Realtime;

/**
 Класс отвечающий за взаимодействие с локальным микрофоном

@attention Для корректной работы данный класс требует, что бы в сцене присутствовали скрипты:
- NetworkVariables;

@param catcher ComponentCatcher находящийся в данной сцене.
@param microphoneOnImage Изображение включенного микрофона.
@param microphoneOffImage Изображение выключенного микрофона.
@param microphone Recorder микрофона из Photon.Voice.Unity.
@see ComponentCatcher; NetworkVariables; 
 */
public class MicrophoneController : MonoBehaviour
{
    [SerializeField] private ComponentCatcher _catcher;
    [SerializeField] private Image _microphoneOnImage;
    [SerializeField] private Image _microphoneOffImage;
    private Recorder _microphone;

    private bool isMicrophoneActive;
    private float _currentMicrophoneVolume;
    private NetworkVariables _networkVariables;

    private void Awake()
    {
        _currentMicrophoneVolume = 1.0f;
    }

    private void Start()
    {
        _microphone = _catcher.GetRecorder();
        _networkVariables = _catcher.GetNetworkVariables();
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
        }
        isMicrophoneActive = true;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        if (_microphone != null)
        {
            _microphone.RecordingEnabled = isMicrophoneActive;
        }
    }

    private void OnDestroy()
    {
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
        }
    }

    /// Вкл/выкл микрофон.
    public void SwitchMicrophoneActivity()
    {
        isMicrophoneActive = !isMicrophoneActive;
        _microphoneOnImage.enabled = isMicrophoneActive;
        _microphoneOffImage.enabled = !_microphoneOnImage.enabled;
        if (_microphone != null)
        {
            _microphone.RecordingEnabled = isMicrophoneActive;
        }
    }

    /** 
    Изменить громкость микрофона через UI компонент Slider.
    @param [in] slider Slider, в соответствии со значением которого устанавливается текущая громкость микрофона.
     */
    public void ChangeMicrophoneVolume(Slider slider)
    {
        _currentMicrophoneVolume = (float)slider.value;
        NetworkVariables.SendPropertyToServer(PhotonServerActions.MICROPHONE_VOLUME, _currentMicrophoneVolume);
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PhotonServerActions.UPDATE_STATUS))
        {
            NetworkVariables.SendPropertyToServer(PhotonServerActions.MICROPHONE_VOLUME, _currentMicrophoneVolume);
        }
    }
}
