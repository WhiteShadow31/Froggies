using System.Collections.Generic;
using UnityEngine;
using DrawingTool.Attribute;

namespace DrawingTool
{
    public class DebugDrawColliderDetection : MonoBehaviour
    {
        [Header("Detection Mask")]
        [Tooltip("The LayerMask for detecting GameObjects that have a layer in it.")]
        [SerializeField] LayerMask _objectMask;
        [Space]

        [Tooltip("Is there a limit for the detection ?")]
        public bool hasCapacity = false;

        [ShowIf("hasCapacity", false)]
        [Tooltip("Color for when a GameObject has been detected.")]
        [SerializeField] Color _detection = Color.green;
        [ShowIf("hasCapacity", false)]
        [Tooltip("Color for when no GameObject has been detected.")]
        [SerializeField] Color _noDetection = Color.red;

        [ShowIf("hasCapacity", true)]
        [Tooltip("Gradient to visualise how much of the capacity has been taken.")]
        [SerializeField] Gradient _detectionGradient;

        [ShowIf("hasCapacity", true)]
        [Tooltip("Minimum capacity before capacity begin to fill up.")]
        [SerializeField] int _minimumCapacity = 0;
        [ShowIf("hasCapacity", true)]
        [Tooltip("Maximum capacity that can be reached.")]
        [SerializeField] int _maximumCapacity = 2;

        List<GameObject> _objects = new List<GameObject>();

        private void OnCollisionEnter(Collision collision)
        {
            // other is a GameObject in the layermask
            if ((_objectMask.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                if (!_objects.Contains(collision.gameObject))
                    _objects.Add(collision.gameObject);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // other is a GameObject in the layermask
            if ((_objectMask.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                if (_objects.Contains(collision.gameObject))
                    _objects.Remove(collision.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // other is a GameObject in the layermask
            if ((_objectMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (!_objects.Contains(other.gameObject))
                    _objects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // other is a GameObject in the layermask
            if ((_objectMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (_objects.Contains(other.gameObject))
                    _objects.Remove(other.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            if (this.transform.TryGetComponent<Collider>(out Collider col))
            {
                Color capacityColor = Color.red;
                if (hasCapacity)
                {
                    // Calculate capacity percentage
                    float capacity = 0;
                    if (_objects != null)
                    {
                        capacity = (_objects.Count - _minimumCapacity) / (float)_maximumCapacity;
                    }
                    // Clamp for Gradient evaluate
                    capacity = Mathf.Clamp(capacity, 0, 1);

                    // Get the color from the right gradient
                    if (_detectionGradient != null)
                        capacityColor = _detectionGradient.Evaluate(capacity);

                }
                else
                {
                    bool hasObject = _objects.Count > 0;

                    // Get the good color
                    capacityColor = hasObject ? _detection : _noDetection;
                }

                // Draw
                DebugDraw.WireCollider(col, capacityColor);
            }
        }
    }
}
