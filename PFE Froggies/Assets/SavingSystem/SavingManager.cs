using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SavingManager
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves
    public static string SaveDirectoryPath {  get { return _saveDirectoryPath; } }
    private static string[] _savedGames = null;

    public static bool initialized = false;

    public static string loadedSaveName = "";
    public static bool isLoadingSave = false;

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
        }
        //Debug.Log(_saveDirectoryPath);

        // Get all folders of saved games in the save directory
        _savedGames = Directory.GetDirectories(_saveDirectoryPath, "*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Create a directory to save the files of the level
    /// </summary>
    /// <param name="savedGameName"> Name of the save </param>
    public static void CreateSaveDirectory(string savedGameName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + savedGameName;

        // Look if saved game already exist
        if (Directory.Exists(savedDirectory))
        {
            // There is so overwrite by deleting
            Directory.Delete(savedDirectory, true);
        }
        // Create the directory
        Directory.CreateDirectory(savedDirectory);
    }
    /// <summary>
    /// Create a file with the scene name to access it
    /// </summary>
    /// <param name="savedGameName"></param>
    /// <param name="sceneName"></param>
    public static void SaveSceneName(string savedGameName, string sceneName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + savedGameName;

        string savedData = JsonUtility.ToJson(sceneName);

        File.WriteAllText(savedDirectory + "/" + sceneName, savedData); // Write the file with the data
    }
    /// <summary>
    /// Load the scene from its name and set it as loading a scene
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadSave(string saveName)
    {
        //string path = savedDirectory + "/" + saveName;
        //loadedSaveName = sceneName;
        //isLoadingSave = true;

        //
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Save the transform parameters of the objects inside the list
    /// </summary>
    /// <param name="savedGameName"> The directory where to create the files </param>
    public static void SaveTransform(string savedGameName, List<Transform> savedTransforms)
    {
        string savedDirectory = _saveDirectoryPath + "/" + savedGameName;

        for (int i = 0; i < savedTransforms.Count; i++)
        {
            if (savedTransforms[i] != null)
            {
                SaveTransform savedTransform = new SaveTransform(savedTransforms[i]);
                string savedData = JsonUtility.ToJson(savedTransform);

                File.WriteAllText(savedDirectory + "/" + savedTransforms[i].name, savedData); // Write the file with the data
            }
        }
    }
    /// <summary>
    /// Load the transform datas on the objects in the list
    /// </summary>
    /// <param name="savedGameName"> The directory where to load the files </param>
    public static void LoadTransform(string savedGameName, List<Transform> savedTransforms)
    {
        string savedDirectory = _saveDirectoryPath + "/" + savedGameName;

        // Load each transform
        for (int i = 0; i < savedTransforms.Count; i++)
        {
            // Not null
            if (savedTransforms[i] != null)
            {
                // Get path with object name
                string path = savedDirectory + "/" + savedTransforms[i].name;
                if (File.Exists(path))
                {
                    string loadData = File.ReadAllText(path);
                    SaveTransform savedTransform = JsonUtility.FromJson<SaveTransform>(loadData);

                    savedTransform.LoadOnTransform(savedTransforms[i]);
                }
            }
        }
    }

}
