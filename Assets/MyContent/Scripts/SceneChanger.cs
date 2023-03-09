using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _startSceneName;
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(_startSceneName);
    }

    public void ExitFromApplication()
    {
        Debug.LogWarning("Завершение работы приложения.");
        Application.Quit();
    }
}
