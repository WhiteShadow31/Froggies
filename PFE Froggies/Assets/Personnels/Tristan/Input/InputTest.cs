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
        GameObject go = Instantiate(_prefabPlayerEntity);
        _playerEntity = go.GetComponent<PlayerEntity>();
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
}
