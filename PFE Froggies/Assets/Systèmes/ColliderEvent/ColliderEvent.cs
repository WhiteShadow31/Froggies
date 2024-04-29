using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ColliderEvent : MonoBehaviour
{


    public LayerMask mask;
    public bool colliderIsTrigger = false;
    public int nbrObjectsRequired = 1;
    [Tooltip("Event to be used, once, when there is enough objects in collider.")]
    public UnityEvent activateEvent;
    [Tooltip("Event to be used, once, when there isn't enough objects in collider.")]
    public UnityEvent deactivateEvent;

    List<GameObject> _gameObjects = new List<GameObject>();

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = colliderIsTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!colliderIsTrigger)
        {
            if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                if (!_gameObjects.Contains(collision.gameObject))
                {
                    _gameObjects.Add(collision.gameObject);

                    if (_gameObjects.Count >= nbrObjectsRequired)
                        activateEvent.Invoke();
                }
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!colliderIsTrigger)
        {
            if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                if (!_gameObjects.Contains(collision.gameObject))
                {
                    _gameObjects.Add(collision.gameObject);

                    if (_gameObjects.Count < nbrObjectsRequired)
                        deactivateEvent.Invoke();
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colliderIsTrigger)
        {
            if ((mask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (!_gameObjects.Contains(other.gameObject))
                {
                    _gameObjects.Add(other.gameObject);

                    if (_gameObjects.Count >= nbrObjectsRequired)
                        activateEvent.Invoke();
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (colliderIsTrigger)
        {
            if ((mask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (!_gameObjects.Contains(other.gameObject))
                {
                    _gameObjects.Add(other.gameObject);

                    if (_gameObjects.Count < nbrObjectsRequired)
                        deactivateEvent.Invoke();
                }
            }

        }
    }

    public void DestroyEvent()
    {
        this.gameObject.SetActive(false);
    }
}
