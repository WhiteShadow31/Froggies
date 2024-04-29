using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyDebugger : MonoBehaviour
{
    public Rigidbody body;

    private void Start()
    {
        if(body == null)
            body = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        if(body != null)
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(body.position, body.position + body.velocity);
            Gizmos.DrawSphere(body.position + body.velocity, 0.1f);
        }
    }
}
