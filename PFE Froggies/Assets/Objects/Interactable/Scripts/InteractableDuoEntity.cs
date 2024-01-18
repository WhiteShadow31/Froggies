using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDuoEntity : InteractableEntity
{
    public float timeTriedToBePushed = 1f;
    bool _isTriedToBePushed = false;

    public override void Push(Vector3 dir, float force)
    {
        base.Push(dir, force);
    }
}
