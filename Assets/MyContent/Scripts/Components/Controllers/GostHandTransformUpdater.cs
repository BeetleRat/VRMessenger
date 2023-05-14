using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/**
Класс, отвечающий за синхронизацию положения рук на сервере

Данный класс синхронизирует руки из prefab-а LeftHandSynthetic/RightHandSynthetic.

Класс ориентируется на название костей в руках. А именно он ищет в сцене и в переданном объекте объекты с названиями:
- b_l_wrist;
- b_r_wrist;
- b_l_index1;
- b_r_index1;
- b_l_index2;
- b_r_index2;
- b_l_index3;
- b_r_index3;
- b_l_middle1;
- b_r_middle1;
- b_l_middle2;
- b_r_middle2;
- b_l_middle3;
- b_r_middle3;
- b_l_pinky0;
- b_r_pinky0;
- b_l_pinky1;
- b_r_pinky1;
- b_l_pinky2;
- b_r_pinky2;
- b_l_pinky3;
- b_r_pinky3;
- b_l_ring1;
- b_r_ring1;
- b_l_ring2;
- b_r_ring2;
- b_l_ring3;
- b_r_ring3;
- b_l_thumb0;
- b_r_thumb0;
- b_l_thumb1;
- b_r_thumb1;
- b_l_thumb2;
- b_r_thumb2;
- b_l_thumb3;
- b_r_thumb3;
@param handType Тип руки:
    - None;
    - Right;
    - Left;
*/
[RequireComponent(typeof(PhotonView))]
public class GostHandTransformUpdater : ControllerModel
{
    [SerializeField] private HandType _handType;

    private string _prefix;

    private Transform _wrist;

    private Transform _index1;
    private Transform _index2;
    private Transform _index3;

    private Transform _middle1;
    private Transform _middle2;
    private Transform _middle3;

    private Transform _pinky0;
    private Transform _pinky1;
    private Transform _pinky2;
    private Transform _pinky3;

    private Transform _ring1;
    private Transform _ring2;
    private Transform _ring3;

    private Transform _thumb0;
    private Transform _thumb1;
    private Transform _thumb2;
    private Transform _thumb3;

    private Transform _serverWrist;

    private Transform _serverIndex1;
    private Transform _serverIndex2;
    private Transform _serverIndex3;

    private Transform _serverMiddle1;
    private Transform _serverMiddle2;
    private Transform _serverMiddle3;

    private Transform _serverPinky0;
    private Transform _serverPinky1;
    private Transform _serverPinky2;
    private Transform _serverPinky3;

    private Transform _serverRing1;
    private Transform _serverRing2;
    private Transform _serverRing3;

    private Transform _serverThumb0;
    private Transform _serverThumb1;
    private Transform _serverThumb2;
    private Transform _serverThumb3;

    private void Start()
    {
        _prefix = "";
        switch (_handType)
        {
            case HandType.Left:
                _prefix = "b_l";
                break;
            case HandType.Right:
                _prefix = "b_r";
                break;
        }
        if (_prefix != "")
        {
            CreateHand();
        }
    }

    private void Update()
    {
        if (_myPhotonView.IsMine)
        {
            MapPosition(_serverWrist, _wrist);

            MapPosition(_serverThumb0, _thumb0);
            MapPosition(_serverThumb1, _thumb1);
            MapPosition(_serverThumb2, _thumb2);
            MapPosition(_serverThumb3, _thumb3);

            MapPosition(_serverIndex1, _index1);
            MapPosition(_serverIndex2, _index2);
            MapPosition(_serverIndex3, _index3);

            MapPosition(_serverMiddle1, _middle1);
            MapPosition(_serverMiddle2, _middle2);
            MapPosition(_serverMiddle3, _middle3);

            MapPosition(_serverRing1, _ring1);
            MapPosition(_serverRing2, _ring2);
            MapPosition(_serverRing3, _ring3);

            MapPosition(_serverPinky0, _pinky0);
            MapPosition(_serverPinky1, _pinky1);
            MapPosition(_serverPinky2, _pinky2);
            MapPosition(_serverPinky3, _pinky3);
        }
    }

    private void CreateHand()
    {
        ParseServerHand();
        FindLocalHand();
    }

    private void FindLocalHand()
    {
        _wrist = GameObject.Find(_prefix + "_wrist").transform;

        _index1 = _wrist.Find(_prefix + "_index1").transform;
        _index2 = _index1.Find(_prefix + "_index2").transform;
        _index3 = _index2.Find(_prefix + "_index3").transform;

        _middle1 = _wrist.Find(_prefix + "_middle1").transform;
        _middle2 = _middle1.Find(_prefix + "_middle2").transform;
        _middle3 = _middle2.Find(_prefix + "_middle3").transform;

        _pinky0 = _wrist.Find(_prefix + "_pinky0").transform;
        _pinky1 = _pinky0.Find(_prefix + "_pinky1").transform;
        _pinky2 = _pinky1.Find(_prefix + "_pinky2").transform;
        _pinky3 = _pinky2.Find(_prefix + "_pinky3").transform;

        _ring1 = _wrist.Find(_prefix + "_ring1").transform;
        _ring2 = _ring1.Find(_prefix + "_ring2").transform;
        _ring3 = _ring2.Find(_prefix + "_ring3").transform;

        _thumb0 = _wrist.Find(_prefix + "_thumb0").transform;
        _thumb1 = _thumb0.Find(_prefix + "_thumb1").transform;
        _thumb2 = _thumb1.Find(_prefix + "_thumb2").transform;
        _thumb3 = _thumb2.Find(_prefix + "_thumb3").transform;
    }

    private void ParseServerHand()
    {
        _serverWrist = _objectModel.transform.Find(_prefix + "_wrist");

        _serverIndex1 = _serverWrist.Find(_prefix + "_index1").transform;
        _serverIndex2 = _serverIndex1.Find(_prefix + "_index2").transform;
        _serverIndex3 = _serverIndex2.Find(_prefix + "_index3").transform;

        _serverMiddle1 = _serverWrist.Find(_prefix + "_middle1").transform;
        _serverMiddle2 = _serverMiddle1.Find(_prefix + "_middle2").transform;
        _serverMiddle3 = _serverMiddle2.Find(_prefix + "_middle3").transform;

        _serverPinky0 = _serverWrist.Find(_prefix + "_pinky0").transform;
        _serverPinky1 = _serverPinky0.Find(_prefix + "_pinky1").transform;
        _serverPinky2 = _serverPinky1.Find(_prefix + "_pinky2").transform;
        _serverPinky3 = _serverPinky2.Find(_prefix + "_pinky3").transform;

        _serverRing1 = _serverWrist.Find(_prefix + "_ring1").transform;
        _serverRing2 = _serverRing1.Find(_prefix + "_ring2").transform;
        _serverRing3 = _serverRing2.Find(_prefix + "_ring3").transform;

        _serverThumb0 = _serverWrist.Find(_prefix + "_thumb0").transform;
        _serverThumb1 = _serverThumb0.Find(_prefix + "_thumb1").transform;
        _serverThumb2 = _serverThumb1.Find(_prefix + "_thumb2").transform;
        _serverThumb3 = _serverThumb2.Find(_prefix + "_thumb3").transform;
    }

    private void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
