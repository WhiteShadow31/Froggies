using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavingSystem
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves

    private static string[] _savedGames = null;

    public static bool initialized = false;

    public static void Awake()
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
    /// Save the game
    /// </summary>
    /// <param name="savedGameName"> Name of the save </param>
    public static void CreateSavedGame(string savedGameName)
    {
        string playerDirectorySavePath = _saveDirectoryPath + "/" + savedGameName;

        // Look if saved game already exist
        if (Directory.Exists(playerDirectorySavePath))
        {
            // There is so overwrite by deleting
            Directory.Delete(playerDirectorySavePath, true);
        }

        // Create the directory
        Directory.CreateDirectory(playerDirectorySavePath);

        //SaveTransform(playerDirectorySavePath);
        //SavePlayers(playerDirectorySavePath);
    }

    /// <summary>
    /// Load the game
    /// </summary>
    /// <param name="savedGameName"> Name of the save </param>
    public static void LoadSavedGame(string savedGameName, InGameSaving saver)
    {
        string playerDirectorySavePath = _saveDirectoryPath + "/" + savedGameName;

        // Look if saved game already exist
        if (Directory.Exists(playerDirectorySavePath))
        {
            LoadTransform(playerDirectorySavePath, saver.transformsToSave);
            LoadPlayers(playerDirectorySavePath);
        }
    }


    /// <summary>
    /// Save the transform parameters of the objects inside the list
    /// </summary>
    /// <param name="directorySavePath"> The directory where to create the files </param>
    public static void SaveTransform(string directorySavePath, List<Transform> savedTransforms)
    {
        for(int i = 0; i < savedTransforms.Count; i++)
        {
            if (savedTransforms[i] != null)
            {
                SaveTransform savedTransform = new SaveTransform(savedTransforms[i]);
                string savedData = JsonUtility.ToJson(savedTransform);
            
                File.WriteAllText(directorySavePath + "/" + savedTransforms[i].name, savedData);
            }
        }
    }
    /// <summary>
    /// Load the transform datas on the objects in the list
    /// </summary>
    /// <param name="directorySavePath"> The directory where to load the files </param>
    public static void LoadTransform(string directorySavePath, List<Transform> savedTransforms)
    {
        // Load each transform
        for (int i = 0; i < savedTransforms.Count; i++)
        {
            // Not null
            if (savedTransforms[i] != null)
            {
                // Get path with object name
                string path = directorySavePath + "/" + savedTransforms[i].name;
                if (File.Exists(path))
                {
                    string loadData = File.ReadAllText(path);
                    SaveTransform savedTransform = JsonUtility.FromJson<SaveTransform>(loadData);

                    savedTransform.LoadOnTransform(savedTransforms[i]);
                }
            }
        }
    }

    public static void SavePlayers(string directorySavePath)
    {
        // Look for all controllers
        for(int i = 0; i < PlayerManager.Instance.Controllers.Count; i++)
        {
            // Save the color 
            Color color = PlayerManager.Instance.playerColors.Count > i ? PlayerManager.Instance.playerColors[i] : PlayerManager.Instance.playerColors[0];
            Vector3 spawnPoint = PlayerManager.Instance.spawnPoints.Length > i ? PlayerManager.Instance.spawnPoints[i].position : PlayerManager.Instance.spawnPoints[0].position;

            SavePlayer savedPlayer = new SavePlayer(color, spawnPoint);
            string savedData = JsonUtility.ToJson(savedPlayer);

            File.WriteAllText(directorySavePath + "/Players/" + PlayerManager.Instance.Controllers[i].name, savedData);
        }
    }

    public static void LoadPlayers(string directorySavePath)
    {
        // Load for 2 players
        for (int i = 0; i < 2; i++)
        {
            // Get the path for a player controller 0-1
            string path = directorySavePath + "/Players/" + "PlayerController"+i.ToString();
            if (File.Exists(path))
            {
                string loadData = File.ReadAllText(path);
                SavePlayer savedPlayer = JsonUtility.FromJson<SavePlayer>(loadData);

                PlayerManager.Instance.playerColors[i] = savedPlayer.color;
                PlayerManager.Instance.spawnPoints[i].position = savedPlayer.spawnPoint;
            }
        }
    }
}

public class SaveTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public SaveTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public SaveTransform(Transform saved)
    {
        this.position = saved.position;
        this.rotation = saved.rotation;
        this.scale = saved.localScale;
    }

    public void LoadOnTransform(Transform toLoad)
    {
        toLoad.position = this.position;
        toLoad.rotation = this.rotation;
        toLoad.localScale = this.scale;
    }
}
public class SavePlayer
{
    public Color color;
    public Vector3 spawnPoint;

    public SavePlayer(Color color, Vector3 spawnPoint)
    {
        this.color = color;
        this.spawnPoint = spawnPoint;
    }

}
