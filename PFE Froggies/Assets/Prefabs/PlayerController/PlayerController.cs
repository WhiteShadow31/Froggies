using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected CameraEntity _cameraEntity;
    [Space]
    [SerializeField] protected GameObject _prefabPlayerEntity;
    protected PlayerEntity _playerEntity;
    [Space]
    public int playerNbr = 0;

    private void Awake()
    {
        _cameraEntity = Camera.main.GetComponent<CameraEntity>();
    }

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
        if (_playerEntity != null && _playerEntity.IsGrounded)
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

            _playerEntity.gameObject.name = _prefabPlayerEntity.name + " " + playerNbr.ToString();

            _cameraEntity.AddPlayer(go);
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
