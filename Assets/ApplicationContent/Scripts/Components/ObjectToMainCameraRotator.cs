using UnityEngine;

/**
 Компонент разварачивающий объект лицом к активной камере

@param camera Камера, к которой будет разворачиваться объект.
 */
public class ObjectToMainCameraRotator : MonoBehaviour
{
    [Header("[Optional]")]
    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (_camera == null)
        {
            Camera[] sceneCameras = new Camera[10];
            Camera.GetAllCameras(sceneCameras);
            foreach (Camera camera in sceneCameras)
            {
                if (camera.enabled)
                {
                    _camera = camera;
                    break;
                }
            }
            if (_camera == null)
            {
                Debug.LogWarning("[" + this.name + "] Scene does not contain a camera");
                return;
            }
        }

        transform.LookAt(_camera.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
