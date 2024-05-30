using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField ,Scene] int _firstSceneToLoad;
    int _saveIndexToLoad;


    public void SetSaveIndexToLoad(int index)
    {
        _saveIndexToLoad = index;
    }

    public void LoadSave()
    {
        // Load save, if there's no save then load first game scene

        SceneManager.LoadScene(_firstSceneToLoad);
    }
}