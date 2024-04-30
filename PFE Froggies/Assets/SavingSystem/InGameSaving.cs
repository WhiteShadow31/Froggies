using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSaving : MonoBehaviour
{
    public List<Transform> transformsToSave = new List<Transform>();

    private void Awake()
    {
        // Awake the saving system 
        if(!SavingManager.initialized)
            SavingManager.Initialize();
    }

    public void SaveTransform()
    {
        SavingManager.CreateSaveDirectory("MySave");
        SavingManager.SaveTransform("MySave", transformsToSave);
    }

    public void LoadSaving()
    {
        SavingManager.LoadTransform("MySave", transformsToSave);
    }
}
