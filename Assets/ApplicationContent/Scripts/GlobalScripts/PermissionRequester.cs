using UnityEngine;
using UnityEngine.Android;

/**
Компонент, запрашивающий разрешения шлема виртуальной реальности.

Данный компонент стоит располагать в стартовой сцене. 
В свойствах данного компонента указывается какие разрешения необходимо запросить.
При включении данного компонента, он запросит все указанные разрешения и отключиться.
@param microphone Запрашивать ли разрешение пользоваться микрофоном.
 */
public class PermissionRequester : MonoBehaviour
{
    [Header("Request permissions")]
    [SerializeField] private bool _microphone;

    private void OnEnable()
    {
        if (_microphone && !Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        enabled = false;
    }
}
