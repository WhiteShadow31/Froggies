using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    protected PlayerInputManager _inputManager;
    protected List<PlayerInput> _players = new List<PlayerInput>();

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        _players.Add(player);  
    }
}
