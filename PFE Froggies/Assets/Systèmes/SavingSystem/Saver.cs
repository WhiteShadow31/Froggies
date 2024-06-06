using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Saver
{
    private static string _saveDirectoryPath = Application.persistentDataPath + "/Saves"; // Path for all directories saves
    public static string SaveDirectoryPath { get { return _saveDirectoryPath; } }

    public static bool initialized = false;

    public static bool isLoading = false;
    public static int saveIndex = -1;

    // ==================== INITIALIZE THE SAVING ====================
    /// <summary>
    /// Initialize the SavingManager and directories
    /// </summary>
    public static void Initialize()
    {
        Debug.Log(_saveDirectoryPath);
        // Is there a save directory
        if (!Directory.Exists(_saveDirectoryPath))
        {
            // There is none so create it
            Directory.CreateDirectory(_saveDirectoryPath);
        }

        // Create the 3 saves directories
        for (int i = 0; i < 3; i++)
        {   
            CreateSaveDirectory(i);
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
        string savedDirectory = _saveDirectoryPath + "/Save " + index;

        // Look if saved game already exist
        if (Directory.Exists(savedDirectory))
        {
            // There is so overwrite by deleting
            //Directory.Delete(savedDirectory, true);
        }
        else
        {
            // Create the directory
            Directory.CreateDirectory(savedDirectory);
        }
    }

    public static void SaveScene(int index, int buildIndex)
    {
        string saveDirectory = _saveDirectoryPath + "/Save " + index;

        SceneSaver sceneSaver = new SceneSaver(buildIndex);
        string savedData = JsonUtility.ToJson(sceneSaver);

        File.WriteAllText(saveDirectory + "/Scene.json", savedData);
    }

    public static void SaveActiveScene(int index)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SaveScene(index, buildIndex);
    }
    public static void LoadSave(int index)
    {
        string scenePath = _saveDirectoryPath + "/Save " + index + "/Scene.json";
        if(File.Exists(scenePath))
        {
            string jsonToRead = File.ReadAllText(scenePath);
            SceneSaver sceneSaver = JsonUtility.FromJson<SceneSaver>(jsonToRead);

            isLoading = true;
            saveIndex = index;
            SceneManager.LoadScene(sceneSaver.buildIndex);
        }
        else
        {
            SceneManager.LoadScene(1);
            SaveScene(index, 1);
        }
    }

    public static void SavePlayers(int index, List<PlayerEntity> controllers)
    {
        for(int i = 0; i < 2; i++)
        {
            SavePlayer(index, controllers[i], i);
        }
    }
    public static void SavePlayer(int index, PlayerEntity player, int indexPlayer)
    {
        if(player != null)
        {
            string saveDirectory = _saveDirectoryPath + "/Save " + index;

            PlayerSaver playerSaver = new PlayerSaver(player.playerColor, player.transform.position, player.transform.rotation);
            string savedData = JsonUtility.ToJson(playerSaver);

            File.WriteAllText(saveDirectory + "/Player "+ indexPlayer +".json", savedData);
        }
    }
    public static void LoadPlayers(int index, List<PlayerEntity> players)
    {
        for(int i = 0; i < 2; i++)
        {
            string scenePath = _saveDirectoryPath + "/Save " + index + "/Player "+i + ".json"; // Path for player file

            if (File.Exists(scenePath))
            {
                string jsonToRead = File.ReadAllText(scenePath);
                PlayerSaver playerSaver = JsonUtility.FromJson<PlayerSaver>(jsonToRead);

                // SET THE COLOR
                players[i].SetPlayerColor(playerSaver.color);
                Transform trans = players[i].transform;

                string sceneSavePath = $"{SaveDirectoryPath}/Save {index}/Scene";
                if (File.Exists(sceneSavePath))
                {
                    string jsonScene = File.ReadAllText(sceneSavePath);
                    SceneSaver sceneSaver = JsonUtility.FromJson<SceneSaver>(jsonScene);

                    if(sceneSaver.buildIndex == SceneManager.GetActiveScene().buildIndex)
                    {
                        trans.position = playerSaver.position;
                        trans.rotation = playerSaver.rotation;
                    }
                }
            }
        }
    }

    public static void LoadPlayer(int index, PlayerController controller, int indexPlayer)
    {
        string scenePath = _saveDirectoryPath + "/Save " + index + "/Player " + indexPlayer + ".json"; // Path for player file

        if (File.Exists(scenePath))
        {
            string jsonToRead = File.ReadAllText(scenePath);
            PlayerSaver playerSaver = JsonUtility.FromJson<PlayerSaver>(jsonToRead);

            // SET THE COLOR
            controller.SetPlayerColor(playerSaver.playerEntity.playerColor);
            Transform trans = controller.Player.transform;

            trans.position = playerSaver.position;
            trans.rotation = playerSaver.rotation;
        }
    }

    public static void SaveTransforms(int index, List<Transform> transforms)
    {
        string saveDirectory = _saveDirectoryPath + "/Save " + index;

        List<TransformSaver> transSavers = new List<TransformSaver>();

        for(int i = 0; i < transforms.Count; i++)
        {
            Transform trans = transforms[i];
            TransformSaver saver = new TransformSaver(trans.name, trans.position, trans.rotation, trans.localScale);

            transSavers.Add(saver);
        }
    }
}

public class SceneSaver
{
    public int buildIndex = 0;

    public SceneSaver(int buildIndex)
    {
        //this.scene = scene;
        this.buildIndex = buildIndex;
    }
}

public class PlayerSaver
{
    public Color color;
    public Vector3 position;
    public Quaternion rotation;

    public PlayerSaver(Color color, Vector3 position, Quaternion rotation)
    {
        this.color = color;
        this.position = position;
        this.rotation = rotation;
    }
}

public class TransformSaver
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformSaver(string name, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}
