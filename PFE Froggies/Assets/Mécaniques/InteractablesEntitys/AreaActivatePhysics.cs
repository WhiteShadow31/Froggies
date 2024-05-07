using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaActivatePhysics : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent<Collider>(out Collider col))
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = false;
    }
}
