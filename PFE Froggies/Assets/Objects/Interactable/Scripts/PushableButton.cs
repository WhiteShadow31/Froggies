using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushableButton : InteractableEntity
{
    [SerializeField] UnityEvent _event;

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        _event.Invoke();
    }
}
