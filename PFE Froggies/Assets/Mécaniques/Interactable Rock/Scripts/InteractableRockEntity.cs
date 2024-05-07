using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRockEntity : InteractableDuoEntity, IInteractableEntity
{
    [Header("Time to hit Parameters")]
    [SerializeField] public float timeToMove = 0.55f;
    Vector3 _frogFirstHitDirection = Vector3.zero;

    [Header("Moving Parameters")]
    public LayerMask excludeMask;
    [SerializeField] public float distanceToMove = 1;
    bool canBePushed = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        if (_rb.isKinematic)
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
                Vector3 center = this.transform.position + size.x * direction;
                center.y += size.y / 2;
                bool collide = false;
                Collider[] cels = Physics.OverlapSphere(center, (size.x / 2.1f), ~excludeMask);
                for (int i = 0; i < cels.Length; i++) {

                    if (cels[i].transform != this.transform) {

                        collide = true;
                    }
                }
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
            timer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPosition, posToGo, timer / duration);
            yield return null;
        }
        this.transform.position = posToGo;
        canBePushed = true;
    }

    private void OnDrawGizmos()
    {
      
        Vector3 size = GetComponent<BoxCollider>().size;
        Vector3 center = this.transform.position;
        center.y += size.y / 2;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center + size.x * Vector3.forward, (size.x / 2.1f));
        Gizmos.DrawWireSphere(center + size.x * Vector3.back, (size.x / 2.1f));
        Gizmos.DrawWireSphere(center + size.x * Vector3.right, (size.x / 2.1f));
        Gizmos.DrawWireSphere(center + size.x * Vector3.left, (size.x / 2.1f));
    }
}
