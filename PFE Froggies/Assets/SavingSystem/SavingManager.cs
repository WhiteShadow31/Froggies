using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SavingManager
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves
    public static string SaveDirectoryPath {  get { return _saveDirectoryPath; } }
    private static string[] savedGames = null;

    public static bool initialized = false;

    public static string loadedSaveName = "";
    public static bool isLoadingSave = false;

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
        }

        for (int i = 0; i < 3; i++)
        {
            string sName = "Save " + (i + 1);

            if(!Directory.Exists(_saveDirectoryPath + "/" + sName))
            {
                CreateSaveDirectory(sName);
                SaveSceneName(sName, SceneManager.GetSceneByBuildIndex(1).name);
            }
        }

        // Get all folders of saved games in the save directory
        savedGames = Directory.GetDirectories(_saveDirectoryPath, "*", SearchOption.TopDirectoryOnly);

        initialized = true;
    }

    // ==================== CREATE A SAVE DIRECTORY ====================
    /// <summary>
    /// Create a directory to save the files of the level
    /// </summary>
    /// <param name="savedGameName"> Name of the save </param>
    public static void CreateSaveDirectory(string saveName)
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
    }

    // ==================== SAVE THE SCENE NAME INSIDE THE SAVE DIRECTORY CHOOSED ====================
    /// <summary>
    /// Create a file with the scene name to access it
    /// </summary>
    /// <param name="savedGameName"></param>
    /// <param name="sceneName"></param>
    public static void SaveSceneName(string saveName, Scene scene)
    {
        string path = _saveDirectoryPath + "/" + saveName;

        // Look if saved game exist
        if (Directory.Exists(path))
        {
            string sceneName = scene.name;
            string savedData = JsonUtility.ToJson(sceneName);

            File.WriteAllText(path + "/Scene", savedData);
        }
    }
    public static void SaveSceneName(string saveName, string sceneName)
    {
        string savedDirectory = _saveDirectoryPath + "/" + saveName;

        string savedData = JsonUtility.ToJson(sceneName);

        File.WriteAllText(savedDirectory + "/Scene", savedData); // Write the file with the data with name Scene
    }

    // ==================== LOAD THE SAVING ====================
    /// <summary>
    /// Load the scene from its name and set it as loading a scene
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadSave(string saveName)
    {
        // 
        string path = _saveDirectoryPath + "/" + saveName;

        // Look if saved game exist
        if (Directory.Exists(path))
        {
            string sceneName = JsonUtility.FromJson<string>(path + "/Scene"); ;

            loadedSaveName = saveName;
            isLoadingSave = true;

            SceneManager.LoadScene(sceneName);
        }
    }

    public static void EndLoadSave()
    {
        loadedSaveName = "";
        isLoadingSave = false;
    }


    // ==================== SAVE LIST OF TRANSFORM ====================
    /// <summary>
    /// Save the transform parameters of the objects inside the list
    /// </summary>
    /// <param name="savedGameName"> The directory where to create the files </param>
    public static void SaveTransform(string savedName, List<Transform> savedTransforms)
    {
        string savedDirectory = _saveDirectoryPath + "/" + savedName;

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
