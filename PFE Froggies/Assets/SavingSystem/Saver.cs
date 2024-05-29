using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
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

        string jsonToRead = File.ReadAllText(scenePath);
        SceneSaver sceneSaver = JsonUtility.FromJson<SceneSaver>(jsonToRead);

        isLoading = true;
        saveIndex = index;
        SceneManager.LoadScene(sceneSaver.buildIndex);
    }

    public void SavePlayers(int index, List<PlayerController> controllers)
    {
        for(int i = 0; i < 2; i++)
        {
            SavePlayer(index, controllers[i], i);
        }
    }
    public static void SavePlayer(int index, PlayerController player, int indexPlayer)
    {
        if(player.Player != null)
        {
            string saveDirectory = _saveDirectoryPath + "/Save " + index + "/Player " + indexPlayer + ".json";

            PlayerSaver playerSaver = new PlayerSaver(player.Player);
            string savedData = JsonUtility.ToJson(playerSaver);

            File.WriteAllText(saveDirectory + "/Player "+ indexPlayer +".json", savedData);
        }
    }
    public static void LoadPlayers(int index, List<PlayerController> players)
    {
        for(int i = 0; i < 2; i++)
        {
            string scenePath = _saveDirectoryPath + "/Save " + index + "/Player "+i + ".json"; // Path for player file

            if (File.Exists(scenePath))
            {
                string jsonToRead = File.ReadAllText(scenePath);
                PlayerSaver playerSaver = JsonUtility.FromJson<PlayerSaver>(jsonToRead);

                // SET THE COLOR
                players[i].SetPlayerColor(playerSaver.playerEntity.playerColor);
                Transform trans = players[i].Player.transform;

                trans.position = playerSaver.position;
                trans.rotation = playerSaver.rotation;
            }
        }
    }

    public static void SaveTransforms(int index, List<Transform> trans)
    {
        string saveDirectory = _saveDirectoryPath + "/Save " + index + "/Transforms";

        if (Directory.Exists(saveDirectory))
        {
            Directory.Delete(saveDirectory, true);
        }

        Directory.CreateDirectory(saveDirectory);
        for(int i = 0; i < trans.Count; i++)
        {
            SaveTransform(index, trans[i], i);
        }
    }
    public static void SaveTransform(int index, Transform trans, int transformIndex)
    {
        string saveDirectory = _saveDirectoryPath + "/Save " + index + "/Transforms";

        TransformSaver saver = new TransformSaver(trans.name, trans.position, trans.rotation, trans.localScale);
        string savedData = JsonUtility.ToJson(saver);

        File.WriteAllText(saveDirectory + "/Transform " + transformIndex, savedData);
    }
    public static void LoadTransforms(int index, List<Transform> trans)
    {
        string saveDirectory = _saveDirectoryPath + "/Save " + index + "/Transforms";

        if (File.Exists(saveDirectory))
        {
            for (int i = 0; i < trans.Count; i++)
            {
                if (File.Exists(saveDirectory + "/Transform " + i))
                {
                    string jsonToRead = File.ReadAllText(saveDirectory + "/Transform "+i);
                    TransformSaver saver = JsonUtility.FromJson<TransformSaver>(jsonToRead);

                    if(saver.name == trans[i].name)
                    {
                        trans[i].position = saver.position;
                        trans[i].rotation = saver.rotation;
                        trans[i].localScale = saver.scale;
                    }
                }
            }
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
    public PlayerEntity playerEntity;
    public Vector3 position;
    public Quaternion rotation;

    public PlayerSaver(PlayerEntity playerEntity, Vector3 position, Quaternion rotation)
    {
        this.playerEntity = playerEntity;
        this.position = position;
        this.rotation = rotation;
    }

    public PlayerSaver(PlayerEntity playerEntity)
    {
        this.playerEntity = playerEntity;
        this.position = playerEntity.transform.position;
        this.rotation = playerEntity.transform.rotation;
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
