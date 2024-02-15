using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDuoEntity : InteractableEntity, IInteractableEntity
{
    [Header("Time to hit Parameters")]
    [SerializeField] public float timeTriedToBePushed = 1f;
    [SerializeField] float _timeTried = 0;
    [SerializeField] bool _isTriedToBePushed = false;
    [SerializeField] GameObject _frogFirstHit;

    private void FixedUpdate()
    {
        // A player tried to push it
        if(_isTriedToBePushed)
        {
            // Timer waiting for other player to hit it
            if(_timeTried < timeTriedToBePushed)
                _timeTried += Time.fixedDeltaTime;
            else
            {
                _isTriedToBePushed = false;
                _timeTried = 0;
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
