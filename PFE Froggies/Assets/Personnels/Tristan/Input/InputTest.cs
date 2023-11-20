using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField] protected GameObject _prefabPlayerEntity;
    protected PlayerEntity _playerEntity;

    private void Start()
    {
        SpawnPlayer();
    }

    void OnMove(InputValue ctx)
    {
        Debug.Log(ctx.Get<Vector2>());
    }

    void OnStartAim()
    {
        Debug.Log("Start");
    }

    void OnEndAim()
    {
        Debug.Log("End");
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
        if(_prefabPlayerEntity != null)
        {
            GameObject go = Instantiate(_prefabPlayerEntity);
            _playerEntity = go.GetComponent<PlayerEntity>();
            go.transform.position = pos;
        }
    }
}
