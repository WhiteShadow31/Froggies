using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSaving : MonoBehaviour
{
    public static InGameSaving Instance;

    private void Awake()
    {
        Instance = this;

        // Awake the saving system 
        if(!SavingManager.initialized)
            SavingManager.Initialize();
    }

    private void Start()
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(2);
        Saver.SaveSceneName(0, scene);
        Saver.LoadSave(0);
    }
}
