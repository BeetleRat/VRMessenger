using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasySingleton : MonoBehaviour
{
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
