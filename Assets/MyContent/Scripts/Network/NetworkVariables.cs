using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class PlayersProperty
{
    public const string CONTROLLER_TYPE = "ControllerType";
    public const string GESTURE_FINGERS = "GestureFingers";
    public const string UPDATE_STATUS = "UpdateStatus";
}

public class NetworkVariables : MonoBehaviourPunCallbacks
{
    public UnityAction<Player, Hashtable> OnNetworkVariablesUpdate;

    public static void SendPropertyToServer<T>(string propertyName, T property)
    {
        Hashtable hash = new Hashtable();
        hash.Add(propertyName, property);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        OnNetworkVariablesUpdate?.Invoke(targetPlayer, changedProps);
    }
}
