using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
/**
 Компонент, отображающий имя игрока в текстовом поле.

@param photonView Компонент PhotonView.
@param playerNameText Текстовое поле, в котором будет отображаться имя игрока.
 */
public class UIDisplayPlayerName : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private TMP_Text _playerNameText;

    private void Start()
    {
        _playerNameText.text = _photonView.Owner.NickName;
        if (_photonView.IsMine)
        {
            _playerNameText.GetComponentInParent<Canvas>().enabled = false;
        }
    }
}
