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

    void OnJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.JumpInput = ctx.Get<float>() > 0.1f;
            //_playerEntity.Jump();
        }
    }

    void OnStartAim(InputValue ctx)
    {

    }

    void OnEndAim(InputValue ctx)
    {

    }

    void OnTongue(InputValue ctx)
    {

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
