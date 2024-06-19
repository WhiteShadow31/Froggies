using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class InGameSaving : MonoBehaviour
{
    public static InGameSaving Instance;

    private void Awake()
    {
        Instance = this;

        // Awake the saving system 
        if(!Saver.initialized)
            Saver.Initialize();
    }

    private void Start()
    {
        if(Saver.isLoading)
        {
            LoadPlayers();
            //Saver.isLoading = false;
        }
    }

    private void Update()
    {
        //int indexActiveScene = SceneManager.GetActiveScene().buildIndex;
        
        //if(Input.GetKeyDown(KeyCode.Space))
        //    Saver.SaveActiveScene(0); // Save for the save 0

        //if(Input.GetKeyDown(KeyCode.Backspace))
        //    Saver.LoadSave(0); // Load the save 0
        
        //if (Input.GetKeyDown(KeyCode.E)) // Next scene
        //{
        //    int nbrScene = SceneManager.sceneCountInBuildSettings;

        //    if (indexActiveScene < nbrScene)
        //        SceneManager.LoadScene(indexActiveScene + 1);
        //}

        //if (Input.GetKeyDown(KeyCode.Q)) // Previous scene
        //{
        //    if (indexActiveScene > 0)
        //        SceneManager.LoadScene(indexActiveScene-1);
        //}


    }

    public void LoadSave()
    {
        Saver.LoadSave(Saver.saveIndex);
    }

    public void SetSaveIndex(int index)
    {
        Saver.saveIndex = index;
    }

    public void SavePlayers()
    {
        if(PlayerManager.Instance != null)
            Saver.SavePlayers(Saver.saveIndex, PlayerManager.Instance.playerEntities);
    }

    public void LoadPlayers()
    {
        if (PlayerManager.Instance != null)
        {
            Saver.LoadPlayers(Saver.saveIndex, PlayerManager.Instance.playerEntities);
        }
    }

}
