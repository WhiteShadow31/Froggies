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
        if(!Saver.initialized)
            Saver.Initialize();
    }

    private void Start()
    {
        if(Saver.isLoading)
        {


            Saver.isLoading = false;
        }

        //Saver.SaveActiveScene(0);
    }
}
