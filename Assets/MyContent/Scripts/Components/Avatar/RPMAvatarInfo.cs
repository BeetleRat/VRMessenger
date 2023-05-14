using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 Класс, хранящий информацию об аватаре, использующим модель Ready Player Me.

@param redyPlayerMeAvatar Prefab аватара, созданный через Ready Player Me.
@param gender Пол модели. В зависимости от данного параметра будет выбран скелет, 
к которому будет прикреплена модель.
@see AvatarInfo
 */
public class RPMAvatarInfo : AvatarInfo
{
    [SerializeField] private GameObject _redyPlayerMeAvatar;
    [SerializeField] private Gender _gender;

    private void Awake()  
    {
        RPMAvatarParser[] avatarSkeleton = GetComponentsInChildren<RPMAvatarParser>();
        foreach(RPMAvatarParser skeleton in avatarSkeleton)
        {
            if(skeleton.GetSkeletonGender()!= _gender)
            {
                skeleton.gameObject.SetActive(false);
            }
        }
    }

    /**
     Геттер модели Ready Player Me.
    @return Prefab аватара, созданный через Redy Player Me
     */
    public GameObject GetRedyPlayerMeAvatar()
    {
        return _redyPlayerMeAvatar;
    }
}
