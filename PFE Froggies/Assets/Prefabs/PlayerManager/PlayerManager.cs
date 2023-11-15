using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    protected PlayerInputManager _inputManager;
    protected List<PlayerInput> _players = new List<PlayerInput>();
    protected int _playerNbr = 0;

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = this.transform.position;
        _players.Add(player);

        _playerNbr++;
        if(player.transform.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            player.gameObject.name += " " + _playerNbr.ToString(); 
            controller.playerNbr = _playerNbr;
        }
    }
}
