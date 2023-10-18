using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
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

    void OnStartJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.StartJumpInput = ctx.Get<float>() > 0.1f;
        }
    }

    void OnEndJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.EndJumpInput = true;
        }
    }

    void OnStartTongueAim(InputValue ctx)
    {
        if( _playerEntity != null)
        {
            _playerEntity.StartTongueAimInput = ctx.Get<float>() > 0.1f;
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
