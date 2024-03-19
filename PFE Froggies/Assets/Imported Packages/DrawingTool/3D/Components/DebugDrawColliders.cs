using UnityEngine;

namespace DrawingTool
{
    public class DebugDrawColliders : MonoBehaviour
    {
        [Tooltip("Color of the Colliders.")]
        [SerializeField] Color _colliderColor = Color.red;

        private void OnDrawGizmos()
        {
            RecursiveDrawCollider(this.transform);
        }

        protected void RecursiveDrawCollider(Transform parent)
        {
            if (parent.TryGetComponent<Collider>(out Collider col))
            {
                DebugDraw.WireCollider(col, _colliderColor);
            }

            foreach (Transform child in parent)
            {
                RecursiveDrawCollider(child);
            }
        }
    }
}
