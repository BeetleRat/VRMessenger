using UnityEngine;

/**
Компонент обеспечивающий паттерн Singleton для того объекта, к которому он применен

Паттерн Singleton - порождающий паттерн, который гарантирует, 
что для определенного класса будет создан только один объект, 
а также предоставит к этому объекту точку доступа.
 */
public class EasySingleton : MonoBehaviour
{
    /// Единственный статичный экземпляр данного объекта
    public static EasySingleton Instance = null;

    private void Start()
    {
        SingleToneOnStart();
    }

    private void SingleToneOnStart()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
