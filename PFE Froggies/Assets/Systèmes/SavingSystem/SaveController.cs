using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveController
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves
    private static string[] _savedGames = null;

    static string saveName;
    static string savePath;

    static bool _initialized = false;
    public static bool Initialized { get { return _initialized; } }

    static bool _isLoading = false;
    static string _loadingSaveName = "";

    // ------------------------------ CREATE A SAVE DIRECTORY 

    /// <summary>
    /// Initialize the SaveController and Save directory
    /// </summary>
    public static void Initialize()
    {
        // Is there a save directory
        if (!Directory.Exists(_saveDirectoryPath))
        {
            // There is none so create it
            Directory.CreateDirectory(_saveDirectoryPath);
        }
        //Debug.Log(_saveDirectoryPath);

        // Get all folders of saved games in the save directory
        _savedGames = Directory.GetDirectories(_saveDirectoryPath, "*", SearchOption.TopDirectoryOnly);

        _initialized = true;
    }

    // ------------------------------ CREATE A DIRECTORY FOR A SAVE (Inside saves)

    /// <summary>
    /// Create a directory for a save inside the directory Saves
    /// </summary>
    /// <param name="saveName"></param>
    public static string CreateSaveDirectory(string saveName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + saveName;

        // Look if saved game already exist
        if (Directory.Exists(savedDirectory))
        {
            // There is so overwrite by deleting
            Directory.Delete(savedDirectory, true);
        }
        // Create the directory
        Directory.CreateDirectory(savedDirectory);

        return savedDirectory;
    }

    public static void LoadSaveDirectory(string saveName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + saveName;

        // Look if saved game exist
        if (Directory.Exists(savedDirectory))
        {
            
        }
    }

    // ------------------------------ SAVE SCENE NAME

    /// <summary>
    /// Create a file with the scene name to access it
    /// </summary>
    /// <param name="saveName"></param>
    /// <param name="sceneName"></param>
    public static void SaveSceneName(string saveName, string sceneName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + saveName;

        string savedData = JsonUtility.ToJson(sceneName);

        File.WriteAllText(savedDirectory + "/" + sceneName, savedData); // Write the file with the data
    }
    /// <summary>
    /// Create a file with the scene name to access it
    /// </summary>
    /// <param name="saveName"></param>
    /// <param name="sceneName"></param>
    public static void SaveSceneName(string saveName, Scene scene)
    {
        SaveSceneName(saveName, scene.name);
    }
}
