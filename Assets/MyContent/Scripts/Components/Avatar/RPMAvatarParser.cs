using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Класс для замены меша и материала оригинальной модели на указанную
[System.Serializable]
public class MeshSwapper
{
    /// Нужно ли использовать данную модель.
    public bool IsActive;
    /// SkinnedMeshRenderer оригинальной модели. В данной модели меш и материал будут заменены.
    public SkinnedMeshRenderer Original;
    /** 
     SkinnedMeshRenderer, из которого будут взяты меш и материалы.
    @attention Данная модель должна присутствовать в сцене. 
    То есть перед ее использованием ее необходимо заспавнить при помощи Instantiate.
     */
    public SkinnedMeshRenderer NewMesh;

    /// Метод заметы меша и материалов в оригинальной модели на меш и материалы из указанной.
    public void SwapMesh()
    {
        Original.sharedMesh = NewMesh.sharedMesh;
        Original.materials = NewMesh.materials;
    }
}

/// Пол скелета модели
public enum Gender
{
    Male, ///< Мужской
    Female ///< Женский
};


/**
 Компонент для парсинга модели Ready Player Me в аватар.

Данный компонент берет модель из RPMAvatarInfo и прикрепляет выбранные ее части к скелету, 
на котором используется данный компонент.
@param skeletonGender Пол скелета, с которым работает данный компонент.
@param rpmAvatarInfo RPMAvatarInfo аватара, в котором работает данный компонент. 
Из данного RPMAvatarInfo будет взята модель, прикрепляемая к скелету.
@param eyeLeft Использовать ли в аватаре модель левого глаза.
@param eyeRight Использовать ли в аватаре модель правого глаза.
@param head Использовать ли в аватаре модель головы.
@param teeth Использовать ли в аватаре модель зубов.
@param body Использовать ли в аватаре модель тела.
@param outfitBottom Использовать ли в аватаре модель штанов.
@param outfitFootwear Использовать ли в аватаре модель ботинок.
@param outfitTop Использовать ли в аватаре модель верхней одежды.
@param hair Использовать ли в аватаре модель волос.
@param beard Использовать ли в аватаре модель бороды.
@param glasses Использовать ли в аватаре модель очков.
@see RPMAvatarInfo
 */
public class RPMAvatarParser : MonoBehaviour
{
    [SerializeField] private Gender _skeletonGender;
    [SerializeField] private RPMAvatarInfo _rpmAvatarInfo;

    [Header("Active avatar parts")]
    [SerializeField] private bool _eyeLeft;
    [SerializeField] private bool _eyeRight;
    [SerializeField] private bool _head;
    [SerializeField] private bool _teeth;
    [SerializeField] private bool _body;
    [SerializeField] private bool _outfitBottom;
    [SerializeField] private bool _outfitFootwear;
    [SerializeField] private bool _outfitTop;
    [SerializeField] private bool _hair;
    [SerializeField] private bool _beard;
    [SerializeField] private bool _glasses;

    private MeshSwapper _eyeLeftMS;
    private MeshSwapper _eyeRightMS;
    private MeshSwapper _headMS;
    private MeshSwapper _teethMS;
    private MeshSwapper _bodyMS;
    private MeshSwapper _outfitBottomMS;
    private MeshSwapper _outfitFootwearMS;
    private MeshSwapper _outfitTopMS;
    private MeshSwapper _hairMS;
    private MeshSwapper _beardMS;
    private MeshSwapper _glassesMS;

    private void Start()
    {
        _eyeLeftMS = new MeshSwapper();
        _eyeLeftMS.IsActive = _eyeLeft;
        _eyeRightMS = new MeshSwapper();
        _eyeRightMS.IsActive = _eyeRight;
        _headMS = new MeshSwapper();
        _headMS.IsActive = _head;
        _teethMS = new MeshSwapper();
        _teethMS.IsActive = _teeth;
        _bodyMS = new MeshSwapper();
        _bodyMS.IsActive = _body;
        _outfitBottomMS = new MeshSwapper();
        _outfitBottomMS.IsActive = _outfitBottom;
        _outfitFootwearMS = new MeshSwapper();
        _outfitFootwearMS.IsActive = _outfitFootwear;
        _outfitTopMS = new MeshSwapper();
        _outfitTopMS.IsActive = _outfitTop;
        _hairMS = new MeshSwapper();
        _hairMS.IsActive = _hair;
        _beardMS = new MeshSwapper();
        _beardMS.IsActive = _beard;
        _glassesMS = new MeshSwapper();
        _glassesMS.IsActive = _glasses;

        SkinnedMeshRenderer[] children = GetComponentsInChildren<SkinnedMeshRenderer>();
        GameObject tempObject = Instantiate(_rpmAvatarInfo.GetRedyPlayerMeAvatar());
        SkinnedMeshRenderer[] rpmChildrens = tempObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < children.Length; i++)
        {
            switch (children[i].name)
            {
                case "Renderer_EyeLeft":
                    SetMeshes("Renderer_EyeLeft", ref _eyeLeftMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_EyeRight":
                    SetMeshes("Renderer_EyeRight", ref _eyeRightMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Head":
                    SetMeshes("Renderer_Head", ref _headMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Teeth":
                    SetMeshes("Renderer_Teeth", ref _teethMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Body":
                    SetMeshes("Renderer_Body", ref _bodyMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Outfit_Bottom":
                    SetMeshes("Renderer_Outfit_Bottom", ref _outfitBottomMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Outfit_Footwear":
                    SetMeshes("Renderer_Outfit_Footwear", ref _outfitFootwearMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Outfit_Top":
                    SetMeshes("Renderer_Outfit_Top", ref _outfitTopMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Hair":
                    SetMeshes("Renderer_Hair", ref _hairMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Beard":
                    SetMeshes("Renderer_Beard", ref _beardMS, ref children[i], rpmChildrens);
                    break;
                case "Renderer_Glasses":
                    SetMeshes("Renderer_Glasses", ref _glassesMS, ref children[i], rpmChildrens);
                    break;
            }
        }
        Destroy(tempObject);
    }

    private void SetMeshes(string name, ref MeshSwapper meshSwapper, ref SkinnedMeshRenderer child, SkinnedMeshRenderer[] rpmChildrens)
    {
        if (meshSwapper.IsActive)
        {
            meshSwapper.Original = child;
            foreach (SkinnedMeshRenderer rpmChildren in rpmChildrens)
            {
                if (rpmChildren.name == name)
                {
                    meshSwapper.NewMesh = rpmChildren;
                    break;
                }
            }
            if (meshSwapper.NewMesh == null)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                meshSwapper.SwapMesh();
            }
        }
        else
        {
            child.gameObject.SetActive(false);
        }
    }

    /**
     Геттер пола скелета.
    @return Gender используемого скелета.
     */
    public Gender GetSkeletonGender()
    {
        return _skeletonGender;
    }
}
