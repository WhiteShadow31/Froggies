using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected GameObject _prefabPlayerEntity;
    protected PlayerEntity _playerEntity;

    private void Start()
    {
        SpawnPlayer();
    }

    void OnMove(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            _playerEntity.RotaInput = ctx.Get<Vector2>();
        }
    }

    void OnJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.JumpInput = true;
        }
    }

    void OnLongJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.LongJumpInput = true;
        }
    }

    void OnStartTongueAim(InputValue ctx)
    {
        if( _playerEntity != null)
        {
            _playerEntity.StartTongueAimInput = true;
        }
    }

    void OnEndTongueAim(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            _playerEntity.EndTongueAimInput = true;          
        }
    }

    void OnMount(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            _playerEntity.MountInput = true;
        }
    }

    public void SpawnPlayer()
    {
        if (_prefabPlayerEntity != null)
        {
            GameObject go = Instantiate(_prefabPlayerEntity);
            _playerEntity = go.GetComponent<PlayerEntity>();
            go.transform.position = this.transform.position;
        }
    }
    public void SpawnPlayer(Vector3 pos)
    {
        if (_prefabPlayerEntity != null)
        {
            GameObject go = Instantiate(_prefabPlayerEntity);
            _playerEntity = go.GetComponent<PlayerEntity>();
            go.transform.position = pos;
        }
    }
}
