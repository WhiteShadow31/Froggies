using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    // ADD CAMERA SCRIPT REFERENCE HERE

    protected PlayerInputManager _inputManager;
    protected List<PlayerInput> _players = new List<PlayerInput>();

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    public void nPlayerJoined(PlayerInput player)
    {
        _players.Add(player);

        Transform playerTransform = player.transform;
        // ADD PLAYER TRANSFORM TO CAMERA
    }
}
