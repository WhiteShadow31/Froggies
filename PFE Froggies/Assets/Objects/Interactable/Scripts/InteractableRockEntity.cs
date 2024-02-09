using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRockEntity : InteractableDuoEntity, IInteractableEntity
{
    [Header("Time to hit Parameters")]
    [SerializeField] public float timeToMove = 0.55f;
    Vector3 _frogFirstHitDirection = Vector3.zero;

    [Header("Moving Parameters")]
    [SerializeField] public float distanceToMove = 1;
    bool canBePushed = true;

    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        Vector3 direction = (this.transform.position - frog.transform.position).normalized;
        // Take only 1 axis
        direction.y = 0;
        float signe = 0;

        // Move to direction right 
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            signe = Mathf.Abs(direction.x) / direction.x;
            direction = Vector3.right * signe;
        }
        // Move to direction forward 
        else
        {
            signe = Mathf.Abs(direction.z) / direction.z;
            direction = Vector3.forward * signe;
        }

        // Has already been hit
        if (_isTriedToBePushed && _frogFirstHit != frog)
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            Vector3 center = this.transform.position + distanceToMove * direction;
            center.y += size.y / 2;
            bool collide = Physics.OverlapBox(center, size / 2.1f).Length > 0;
            // Isnt pushed
            if (canBePushed && _frogFirstHitDirection == direction && !collide)
                StartCoroutine(MoveBoulder(timeToMove, this.transform.position + distanceToMove * direction));


        }
        // 1st time hit
        else
        {
            _isTriedToBePushed = true;
            _timeTried = 0;
            _frogFirstHit = frog;

            _frogFirstHitDirection = direction;
        }
    }

    IEnumerator MoveBoulder(float duration, Vector3 posToGo)
    {
        // Move timer
        float timer = 0;

        Vector3 startPosition = this.transform.position;

        // If it can be pushed
        canBePushed = false;

        // Calculate move with time
        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            this.transform.position = Vector3.Lerp(startPosition, posToGo, timer / duration);
            yield return null;
        }
        this.transform.position = posToGo;
        canBePushed = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 forwardDistance = this.transform.position + distanceToMove * Vector3.forward;
        Vector3 backDistance = this.transform.position + distanceToMove * Vector3.back;
        Vector3 rightDistance = this.transform.position + distanceToMove * Vector3.right;
        Vector3 leftDistance = this.transform.position + distanceToMove * Vector3.left;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(forwardDistance, 0.3f);
        Gizmos.DrawSphere(backDistance, 0.3f);
        Gizmos.DrawSphere(rightDistance, 0.3f);
        Gizmos.DrawSphere(leftDistance, 0.3f);
    }
}
