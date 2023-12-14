using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public LayerMask interactiveMask;
    public int nbrPlayerNeeded = 2;
    public Transform checkZone;
    public float checkZoneRadius = 0.1f;

    protected List<Transform> _targets = new List<Transform>();

    private void OnCollisionEnter(Collision collision)
    {
        // Collide with an objet from 
        if ((interactiveMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            //Debug.Log("Hit with Layermask");
            if(IsInside(collision.transform))
                _targets.Add(collision.transform);
        }
    }

    // CAST A SPHERE AFTER COLLISION TO LOOK IF TRANSFORM IS INSIDE THE OBJECT (A VOIR)
    protected bool IsInside(Transform newTarget)
    {
        bool result = false;
        if (!_targets.Contains(newTarget))
        {
            Collider[] cols = Physics.OverlapSphere(checkZone.position, checkZoneRadius, interactiveMask);

            for(int i = 0; i < cols.Length; i++)
            {
                if (cols[i].transform ==  newTarget)
                    result = true;
            }
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Color col = Color.red;
        col.a = 0.1f;
        Gizmos.color = col; 
        Gizmos.DrawSphere(checkZone.position, checkZoneRadius);
    }
}
