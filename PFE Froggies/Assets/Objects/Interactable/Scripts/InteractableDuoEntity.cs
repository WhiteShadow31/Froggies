using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDuoEntity : InteractableEntity, IInteractableEntity
{
    [Header("Time to hit Parameters")]
    [SerializeField] public float timeTriedToBePushed = 1f;
    protected float _timeTried = 0;
    protected bool _isTriedToBePushed = false;
    protected GameObject _frogFirstHit;



    protected virtual void FixedUpdate()
    {
        // A player tried to push it
        if (_isTriedToBePushed)
        {
            // Timer waiting for other player to hit it
            if (_timeTried < timeTriedToBePushed)
                _timeTried += Time.fixedDeltaTime;
            // Timer reached
            else
            {
                // Stop try to push
                _isTriedToBePushed = false;
                // Reset timer
                _timeTried = 0;
                // Clear 1st hit
                _frogFirstHit = null;
            }
        }
    }

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        // Has already been hit
        if (_isTriedToBePushed && _frogFirstHit != frog)
            base.Push(dir, force, frog);
        // 1st time hit
        else
        {
            _isTriedToBePushed = true;
            _timeTried = 0;
            _frogFirstHit = frog;
        }
    }
}
