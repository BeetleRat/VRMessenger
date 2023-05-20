using UnityEngine;

/**
 Компонент якорь для отображения контроллеров на сервере

К данному компоненту привязывается отображение соответствующего контроллера на сервере. 
То есть его необходимо расположить в месте с контроллером локального игрока.
@param handType HandType контроллера, который будет привязан к этому компоненту.
@param catcher ComponentCatcher находящийся в данной сцене.
 */
public class HandView : MonoBehaviour
{
    [SerializeField] private HandType _handType;
    [SerializeField] private ComponentCatcher _catcher;

    /**
     Геттер HandType.
    @return HandType
    - None;
    - Right;
    - Left;
     */
    public HandType GetHandType()
    {
        return _handType;
    }

    /**
     Геттер ControllerEvents.
    @return ControllerEvents данного игрока.
     */
    public ControllerEvents GetControllerSwitcher()
    {
        if (_catcher == null)
        {
            return null;
        }

        return _catcher.GetControllerEvents();
    }
}
