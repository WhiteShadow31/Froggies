using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawingTool
{
    public class DebugDraw2DExampleScene : MonoBehaviour
    {
        [Header("Draw Path")]
        [Tooltip("Draw a path between GameObjects")]
        public Transform path2D;
        [Tooltip("Color of the path")]
        public Color pathColor;
        [Tooltip("Is the path looping ?")]
        public bool pathLoop = false;

        [Header("Draw PathTension")]
        [Tooltip("Draw a path between GameObjects with vizualisation of the distance")]
        public Transform pathTension2D;
        [Tooltip("Gradient used to show the distance")]
        public Gradient tensionGradient;
        [Tooltip("Minimal distance for left color in gradient")]
        public float minimalDistance = 0;
        [Tooltip("Maximal distance for right color in gradient")]
        public float maximalDistance = 3;
        [Tooltip("Is the path looping ?")]
        public bool tensionLoop = false;


        [Header("Draw Shape2D")]
        [Tooltip("Draw a circle in 2D space")]
        public Transform circle2D;
        [Tooltip("Radius of the circle")]
        public float circle2DRadius;
        [Tooltip("Draw a square in 2D space")]
        public Transform square2D;
        [Tooltip("Size of the square")]
        public Vector2 squareSize;
        [Tooltip("Draw a capsule in 2D space")]
        public Transform capsule2D;
        [Tooltip("Axis used for drawing (Y : Vertical, X : Horizontal")]
        public DrawingTool.AXIS capsuleAxis = DrawingTool.AXIS.Y;
        [Tooltip("Size of the capsule")]
        public Vector2 capsuleSize;

        [Header("Draw Colliders2D")]
        [Tooltip("Draw a box collider")]
        public BoxCollider2D boxCollider2D;
        public Color boxColor;
        [Tooltip("Draw a circle collider")]
        public CircleCollider2D circleCollider;
        public Color circleColor;
        [Tooltip("Draw a capsule collider")]
        public CapsuleCollider2D capsuleCollider2D;
        public Color capsuleColor;
        [Tooltip("Draw an edge collider")]
        public EdgeCollider2D edgeCollider2D;
        public Color edgeColor;
        [Tooltip("Draw a polygon collider")]
        public PolygonCollider2D polygonCollider2D;
        public Color polygonColor;
        [Tooltip("Draw any collider attached to this object")]
        public Transform anyCollider2D;
        public Color colliderColor;

        private void OnDrawGizmos()
        {
            // List for paths
            List<Transform> list = new List<Transform>();

            // ----- PATH
            if (path2D != null)
            {
                // Add Transform child for drawing
                foreach (Transform childPath in path2D)
                {
                    list.Add(childPath);
                }

                // Need a List of Transform or Vector3, a Color and if it loops or not
                DebugDraw2D.Path2D(list, pathColor, pathLoop);

                // Clear list for next method
                list.Clear();
            }

            // ----- PATH TENSION
            if (pathTension2D != null)
            {
                // Add Transform child for drawing
                foreach (Transform childTension in pathTension2D)
                {
                    list.Add(childTension);
                }
                // Need a List of Transform or Vector3, a Gradient to show the distance, a minimal and maximal distance and if it loops or not
                DebugDraw2D.PathTension2D(list, tensionGradient, minimalDistance, maximalDistance, tensionLoop);

                // Clear list for next method
                list.Clear();
            }

            if (circle2D != null)
            {
                // Vector2 position of the drawing
                Vector2 circle2DPos = circle2D.position;
                DebugDraw2D.Circle2D(circle2DPos, circle2DRadius, Color.red, circle2D.rotation, circle2D.lossyScale);
            }

            if (square2D != null)
            {
                // Vector2 position of the drawing
                Vector2 square2DPos = square2D.position;
                DebugDraw2D.Square2D(square2DPos, squareSize, Color.red, square2D.rotation, square2D.lossyScale);
            }

            if (capsule2D != null)
            {
                // Vector2 position of the drawing
                Vector2 capsule2DPos = capsule2D.position;
                DebugDraw2D.Capsule2D(capsule2DPos, capsuleSize, Color.red, capsuleAxis, capsule2D.rotation, capsule2D.lossyScale);
            }

            // ----- COLLIDERS
            if (circleCollider != null)
                DebugDraw2D.CircleCollider2D(circleCollider, circleColor);

            if (boxCollider2D != null)
                DebugDraw2D.BoxCollider2D(boxCollider2D, boxColor);

            if (capsuleCollider2D != null)
                DebugDraw2D.CapsuleCollider2D(capsuleCollider2D, capsuleColor);

            if (edgeCollider2D != null)
                DebugDraw2D.EdgeCollider2D(edgeCollider2D, edgeColor);

            if (polygonCollider2D != null)
                DebugDraw2D.PolygonCollider2D(polygonCollider2D, polygonColor);

            if (anyCollider2D != null && anyCollider2D.TryGetComponent<Collider2D>(out Collider2D anyCol))
                DebugDraw2D.Collider2D(anyCol, colliderColor);
        }
    }
}
