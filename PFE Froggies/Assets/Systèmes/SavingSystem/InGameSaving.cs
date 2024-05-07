using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSaving : MonoBehaviour
{
    public List<Transform> transformsToSave = new List<Transform>();

    private void Awake()
    {
        // Awake the saving system 
        if(!SavingManager.initialized)
            SavingManager.Initialize();
    }

    private void Start()
    {
        if (SavingManager.isLoadingSave) // SavingManager is loading a save
        {
            SavingManager.LoadTransform(SavingManager.loadedSaveName, transformsToSave); // Load the transform based on save name and object in scene

            // OU FAIRE

            // SavingManager.LoadTransform(SceneManager.GetActiveScene().name, transformsToSave); // Load the transform based on scene name and object in scene
        }

        SavingManager.isLoadingSave = false;
        SavingManager.loadedSaveName = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveTransform();
        }
    }

    public void SaveTransform()
    {
        SavingManager.CreateSaveDirectory("MySave");
        SavingManager.SaveTransform("MySave", transformsToSave);
    }
}
