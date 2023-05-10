using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
Скрипт, отвечающий за смену сцен в приложении

Данный класс инкапсулирует логику переключения между сценами у локального игрока
@param startSceneName Имя стартовой сцены. На данную сцену возвращаемся из всех других сцен.
*/
public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _startSceneName;

    /**
    Загрузить указанную сцену
    @param [in] sceneName Имя загружаемой сцены
    */
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /**
    Загрузить стартовую сцену.
    */
    public void LoadStartScene()
    {
        SceneManager.LoadScene(_startSceneName);
    }

    /**
    Метод завершающий работу приложения.

    @note Данный метод завершает только собранное приложение. 
    В режиме отладки в консоль будет выведен Warning о завершении, 
    но отладка продолжится
    */
    public void ExitFromApplication()
    {
        Debug.LogWarning("Завершение работы приложения.");
        Application.Quit();
    }
}
