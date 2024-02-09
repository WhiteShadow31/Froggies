using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class TriggerEvent : MonoBehaviour
{
    public float radiusCollider = 3f;
    public int nbrPlayersRequired = 2;
    [Tooltip("Event to be used, once, when there is enough players in collider.")]
    public UnityEvent activateEvent;
    [Tooltip("Event to be used, once, when there isn't enough players in collider.")]
    public UnityEvent deactivateEvent;

    List<GameObject> _gameObjects = new List<GameObject>();
    SphereCollider _sphereCol;

    private void Awake()
    {
        _sphereCol = GetComponent<SphereCollider>();
        _sphereCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            if (!_gameObjects.Contains(player.gameObject)) 
            { 
                _gameObjects.Add(player.gameObject);

                if (_gameObjects.Count >= nbrPlayersRequired)
                    activateEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            if (_gameObjects.Contains(player.gameObject))
            {
                _gameObjects.Remove(player.gameObject);

                if(_gameObjects.Count < nbrPlayersRequired)
                    deactivateEvent.Invoke();
            }
        }
    }
}
