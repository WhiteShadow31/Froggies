using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TriggerCollider : MonoBehaviour
{
    // SPHERE COLLIDER PARAMETERS
    //
    SphereCollider _collider; // The collider
    float _radius; // Radius for the collider and detection
    LayerMask _layerMask; // Layermask of detection
    List<GameObject> _objects = new List<GameObject>(); // List of objects detected
    public List<GameObject> ObjectsDetected { get { return _objects; } } // Allow to get the list
    public bool HasObjectsDetected { get { return _objects.Count > 0; } } // Return true if detect anything on the LayerMask






    private void Awake()
    {
        // Get the collider
        if( _collider == null )
            _collider = this.transform.TryGetComponent<SphereCollider>(out SphereCollider col) ? col : this.transform.AddComponent<SphereCollider>();
    }

    private void Start()
    {
        // Get the collider
        if (_collider == null)
            _collider = this.transform.TryGetComponent<SphereCollider>(out SphereCollider col) ? col : this.transform.AddComponent<SphereCollider>();

        // Set the collider has a trigger
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Look if object is in the layer mask
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // Look if doesnt already contains it
            if(!_objects.Contains(other.gameObject))
                _objects.Add(other.gameObject); // Add it
            //Debug.Log("Hit with Layermask");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Look if object leaving is in the layer mask
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            // Look if it contains it 
            if (_objects.Contains(other.gameObject))
                _objects.Remove(other.gameObject); // Remove it 
            //Debug.Log("Hit with Layermask");
        }
    }

    // Init the collider with the values
    public void InitTriggerCollider(float radius, LayerMask layerMask)
    {
        _radius = radius;
        _layerMask = layerMask;

        _collider.radius = _radius;
    }
}
