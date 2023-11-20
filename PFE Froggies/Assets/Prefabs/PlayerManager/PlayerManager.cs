using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    protected PlayerInputManager _inputManager;
    protected List<PlayerInput> players = new List<PlayerInput>();
    int playerIndex = 0;

    public Transform[] spawnPoints;

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        players.Add(player);
        playerIndex++;
        if(player.transform.TryGetComponent<PlayerController>(out PlayerController ctrl))
        {
            ctrl.playerNbr = playerIndex;
            ctrl.SpawnPlayer(spawnPoints[playerIndex - 1].position);
            ctrl.spawnPoint = spawnPoints[playerIndex - 1].position;
        }
        if(playerIndex >= spawnPoints.Length)
        {
            playerIndex = 0;
        }
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
