using UnityEngine;
using DrawingTool;
namespace DrawingTool
{
    public class DebugDraw2DColliders : MonoBehaviour
    {
        [Tooltip("Color of the Colliders2D.")]
        [SerializeField] Color _colliderColor = Color.red;
        private void OnDrawGizmos()
        {
            RecursiveDrawCollider(this.transform);
        }

        protected void RecursiveDrawCollider(Transform parent)
        {
            if (parent.TryGetComponent<Collider2D>(out Collider2D col))
            {
                DebugDraw2D.Collider2D(col, _colliderColor);
            }

            foreach (Transform child in parent)
            {
                RecursiveDrawCollider(child);
            }
        }
    }
}
