using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventObject : MonoBehaviour
{
    public bool canBeTriggeredOnce = true;
    bool _hasBeenTriggered = false;
    public UnityEvent triggerredEvent;

    private void Awake()
    {
        if (this.transform.TryGetComponent<Collider>(out Collider col))
            col.isTrigger = true;
        else
            Debug.LogWarning("There is no collider to be triggered for an event");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBeTriggeredOnce)
        {
            if (!_hasBeenTriggered)
                triggerredEvent.Invoke();
                _hasBeenTriggered = true;
        }
        else
            triggerredEvent.Invoke();
            _hasBeenTriggered = true;
    }
}