using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PushObject : MonoBehaviour
{
    public LayerMask _layerMask;

    [Header("Push Force")]
    [SerializeField] float _forcePush;
    [SerializeField] ForceMode _forceMode;
    
    List<Rigidbody> _bodies = new List<Rigidbody>();
    BoxCollider _boxCollider;



    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in _bodies)
        {
            //Vector3 dir = (rb.position - transform.position).normalized;
            Vector3 dir = this.transform.forward;
            dir *= _forcePush;
            rb.AddRelativeForce(dir, _forceMode);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // Look if it contains it 
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && !_bodies.Contains(rb))
                _bodies.Add(rb); // Add it 
            //Debug.Log("Hit with Layermask");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // Look if it contains it 
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && _bodies.Contains(rb))
                _bodies.Remove(rb); // Remove it 
            //Debug.Log("Hit with Layermask");
        }
    }
}
