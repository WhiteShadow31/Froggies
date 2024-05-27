using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves
    public static string SaveDirectoryPath { get { return _saveDirectoryPath; } }

    public static bool initialized = false;

    public static bool isLoading = false;

    // ==================== INITIALIZE THE SAVING ====================
    /// <summary>
    /// Initialize the SavingManager and directories
    /// </summary>
    public static void Initialize()
    {
        // Is there a save directory
        if (!Directory.Exists(_saveDirectoryPath))
        {
            // There is none so create it
            Directory.CreateDirectory(_saveDirectoryPath);

            // Create the 3 saves directories
            for(int i = 0; i < 3 ; i++)
            {
                CreateSaveDirectory("/Save " + (i+1));
            }
        }
        initialized = true;
    }

    // ==================== CREATE A SAVE DIRECTORY ====================
    /// <summary>
    /// Create a directory to save the files of the level
    /// </summary>
    /// <param name="savedGameName"> Name of the save </param>
    public static void CreateSaveDirectory(int index)
    {
        string savedDirectory = _saveDirectoryPath + "/Save" + index;

        // Look if saved game already exist
        if (Directory.Exists(savedDirectory))
        {
            // There is so overwrite by deleting
            Directory.Delete(savedDirectory, true);
        }
        // Create the directory
        Directory.CreateDirectory(savedDirectory);
    }

    
}
