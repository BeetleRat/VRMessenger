using UnityEngine;
using UnityEngine.UI;

/**
Скрипт для воспроизведения gif анимации в UI

@param frameRate Количество кадров в секунду (30 оптимально).
@param frames Массив кадров gif анимации.
 */
[RequireComponent(typeof(RawImage))]
public class GifAnimation : MonoBehaviour
{
    [Range(1, 120)]
    [SerializeField] private float _frameRate;
    [SerializeField] private Texture2D[] _frames;

    private RawImage _image;
    private float _index;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
        _index = 0;
    }

    private void Update()
    {
        _index += Time.deltaTime * _frameRate;
        _index = _index % _frames.Length;
        _image.texture = _frames[(int)_index];
    }
}
