using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// Класс, хранящий константы действий, совершаемых на сервере.
public class PhotonServerActions
{
    /// Изменить тип контроллера.
    public const string CONTROLLER_TYPE = "ControllerType";
    /// Изменить анимацию отображения руки в соответствии с жестом.
    public const string GESTURE_FINGERS = "GestureFingers";
    /// Обновить состояние всех переменных.
    public const string UPDATE_STATUS = "UpdateStatus";
    /// Изменить громкость микрофона.
    public const string MICROPHONE_VOLUME = "MicrophoneVolume";
    /// Изменить kinematic объекта
    public const string CHANGE_KINEMATIC = "ChangeKinematic";
    /// Изменить видимость объекта InterfaceHider
    public const string CHANGE_HIDER = "ChangeHider";
}

/**
 Класс работы с переменными сервера

Данный класс предоставляет возможность отправлять переменные на сервер 
и обрабатывать переменные пришедшие на сервер. 
 */
public class NetworkVariables : MonoBehaviourPunCallbacks
{
    /// Событие пришествия на сервер новой переменной.
    public UnityAction<Player, Hashtable> OnNetworkVariablesUpdate;

    /**
    Метод отправки на сервер переменной.
    @param [in] propertyName string имя переменной из класса PhotonServerActions.
    @param [in] property Переменная, отправляемая на сервер.
     */
    public static void SendPropertyToServer<T>(string propertyName, T property)
    {
        Hashtable hash = new Hashtable();
        hash.Add(propertyName, property);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /**
    Метод, вызываемый при пришествии новых переменных на сервер.
    @param [in] targetPlayer Игрок, приславший переменные.
    @param [in] changedProps Хеш таблица Photon, содержащая переменные, присланные на сервер.
     */
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        OnNetworkVariablesUpdate?.Invoke(targetPlayer, changedProps);
    }
}
