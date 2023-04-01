using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class HandsAnimaionUpdater : ModelAnimator
{
    private GestureAnimation _gestureAnimation;

    private GestureFingers _fingers;

    private NetworkVariables _networkVariables;

    private void Start()
    {
        ComponentCatcher catcher = FindAnyObjectByType<ComponentCatcher>();

        _gestureAnimation = catcher?.GetGestureAnimator();
        if (_gestureAnimation)
        {
            _fingers = new GestureFingers();
            if (_controllerType == OVRInput.Controller.LTouch || _controllerType == OVRInput.Controller.LHand)
            {
                _gestureAnimation.LeftGestChange += ChangeHandPose;
                _fingers.Type = GestureFingers.HandType.Left;
            }
            if (_controllerType == OVRInput.Controller.RTouch || _controllerType == OVRInput.Controller.RHand)
            {
                _gestureAnimation.RightGestChange += ChangeHandPose;
                _fingers.Type = GestureFingers.HandType.Right;
            }
        }

        _networkVariables = catcher?.GetNetworkVariables();
        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate += OnPlayerPropertiesUpdate;
        }
    }

    private void Update()
    {
        UpdateAnimation(_controller.Animator);
    }

    private void UpdateAnimation(Animator animator)
    {
        animator.SetBool("isThumbClosed", _fingers.Thumb);
        animator.SetBool("isIndexClosed", _fingers.Index);
        animator.SetBool("isMiddleClosed", _fingers.Middle);
        animator.SetBool("isRingClosed", _fingers.Ring);
        animator.SetBool("isPinkyClosed", _fingers.Pinky);
    }

    private void OnDestroy()
    {
        if (_gestureAnimation)
        {
            if (_controllerType == OVRInput.Controller.LTouch || _controllerType == OVRInput.Controller.LHand)
            {
                _gestureAnimation.LeftGestChange -= ChangeHandPose;
            }
            if (_controllerType == OVRInput.Controller.RTouch || _controllerType == OVRInput.Controller.RHand)
            {
                _gestureAnimation.RightGestChange -= ChangeHandPose;
            }
        }

        if (_networkVariables)
        {
            _networkVariables.OnNetworkVariablesUpdate -= OnPlayerPropertiesUpdate;
        }
    }

    private void ChangeHandPose(bool[] fingers)
    {
        if (_myPhotonView.IsMine)
        {
            ChangeLocalHandPose(fingers);
            NetworkVariables.SendPropertyToServer(PlayersProperty.GESTURE_FINGERS, fingers);
        }
    }

    private void ChangeLocalHandPose(bool[] fingers)
    {
        _fingers.SetFromBoolArray(fingers);
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayersProperty.GESTURE_FINGERS))
        {
            if (!_myPhotonView.IsMine)
            {
                if (targetPlayer == _myPhotonView.Owner)
                {
                    ChangeLocalHandPose((bool[])changedProps[PlayersProperty.GESTURE_FINGERS]);
                }
            }
        }
    }
}
