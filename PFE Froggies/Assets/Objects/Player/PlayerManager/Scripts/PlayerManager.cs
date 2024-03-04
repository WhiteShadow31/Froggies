using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    protected PlayerInputManager _inputManager;
    protected List<PlayerInput> players = new List<PlayerInput>();
    int playerIndex = 0;
    public int Index { get { return playerIndex; } }


    public List<Color> playerColors = new List<Color>();

    public Transform[] spawnPoints;

    protected List<PlayerController> m_controllers = new List<PlayerController>();
    public List<PlayerController> Controllers { get { return m_controllers; } }

    private void Awake()
    {
        Instance = this;

        _inputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        players.Add(player);
        //playerIndex++;
        if(playerIndex  < 2 && player.transform.TryGetComponent<PlayerController>(out PlayerController ctrl))
        {
            // Change name of controller to differentiate
            ctrl.name = "PlayerController" + playerIndex.ToString();

            // Save the controller
            m_controllers.Add(ctrl);

            // Set player index
            ctrl.playerNbr = playerIndex;

            // Save the spawn point
            ctrl.spawnPoint = spawnPoints[playerIndex].position;
            // Spawn the player frog
            ctrl.SpawnPlayer(spawnPoints[playerIndex].position);
            // Set the color of the frog
            ctrl.SetPlayerColor(playerColors[playerIndex - 1]);

            // Increase index player 
            playerIndex++;
        }
        /*
        if(playerIndex >= spawnPoints.Length)
        {
            playerIndex = 0;
        }
        */
    }

    public void RespawnPlayers()
    {
        foreach(PlayerInput player in players)
        {
            if(player.TryGetComponent<PlayerController>(out PlayerController ctrl))
            {
                ctrl.RespawnPlayer();
            }
        }
    }
}
