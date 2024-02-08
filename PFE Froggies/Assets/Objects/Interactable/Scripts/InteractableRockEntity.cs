using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRockEntity : InteractableEntity, IInteractableEntity
{
    [Header("Time to hit Parameters")]
    [SerializeField] public float timeTriedToBePushed = 1f;
    [SerializeField] float _timeTried = 0;
    [SerializeField] bool _isTriedToBePushed = false;
    [SerializeField] GameObject _frogFirstHit;

    [Header("Moving Parameters")]
    [SerializeField] public float distanceToMove = 1;
    bool canBePushed = true;

    private void FixedUpdate()
    {
        // A player tried to push it
        if (_isTriedToBePushed)
        {
            // Timer waiting for other player to hit it
            if (_timeTried < timeTriedToBePushed)
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
        {
            // base.Push(dir, force, frog);

            // TAKE ONLY 1 AXIS
            Vector3 direction = (this.transform.position - frog.transform.position);
            direction.y = 0;

            if(direction.x > direction.z)
            {
                direction.x = 1;
                direction.z = 0;
            }
            else
            {
                direction.x = 0;
                direction.z = 1;
            }

            if (canBePushed)
                StartCoroutine(MoveBoulder(1, this.transform.position + direction));
            //_rb.AddForce(direction * force, ForceMode.VelocityChange);

        }
        // 1st time hit
        else
        {
            _isTriedToBePushed = true;
            _timeTried = 0;
            _frogFirstHit = frog;
        }
    }

    IEnumerator MoveBoulder(float time, Vector3 posToGo)
    {
        float timer = 0;
        canBePushed = false;

        while (timer < time)
        {
            timer += Time.fixedDeltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, posToGo, timer / time);
            yield return null;
        }
        this.transform.position = posToGo;
        canBePushed = true;
    }
}
