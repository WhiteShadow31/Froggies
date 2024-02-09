using System.Collections.Generic;
using UnityEngine;

namespace DrawingTool
{
    public class DebugDrawExampleScene : MonoBehaviour
{
    [Header("Paths")]
    [Tooltip("Draw a path between GameObjects")]
    public Transform path;
    [Tooltip("Color of the path")]
    public Color pathColor;
    [Tooltip("Is the path looping ?")]
    public bool pathLoop = false;

    [Header("Draw PathTension")]
    [Tooltip("Draw a path between GameObjects with vizualisation of the distance")]
    public Transform pathTension;
    [Tooltip("Gradient used to show the distance")]
    public Gradient tensionGradient;
    [Tooltip("Minimal distance for left color in gradient")]
    public float minimalDistance = 0;
    [Tooltip("Maximal distance for right color in gradient")]
    public float maximalDistance = 3;
    [Tooltip("Is the path looping ?")]
    public bool tensionLoop = false;

    [Header("Colliders")]
    [Tooltip("Draw a box collider")]
    public BoxCollider boxCollider;
    public Color boxColor;
    [Tooltip("Draw a sphere collider")]
    public SphereCollider sphereCollider;
    public Color sphereColor;
    [Tooltip("Draw a capsule collider")]
    public CapsuleCollider capsuleCollider;
    public Color capsuleColor;
    [Tooltip("Draw a mesh collider")]
    public MeshCollider meshCollider;
    public Color meshColor;
    [Tooltip("Draw any collider")]
    public Transform anyCollider;
    public Color colliderColor;


        private void OnDrawGizmos()
        {
            List<Transform> list = new List<Transform>();

            // ----- PATH
            if (path != null)
            {
                foreach (Transform childPath in path)
                {
                    list.Add(childPath);
                }

                DebugDraw.Path(list, pathColor, pathLoop);
                list.Clear();
            }

            if (pathTension != null)
            {
                foreach (Transform childTension in pathTension)
                {
                    list.Add(childTension);
                }

                DebugDraw.PathTension(list, tensionGradient, minimalDistance, maximalDistance, tensionLoop);
                list.Clear();
            }

            if (boxCollider != null)
                DebugDraw.WireBoxCollider(boxCollider, boxColor);
            if (sphereCollider != null)
                DebugDraw.WireSphereCollider(sphereCollider, sphereColor);
            if (capsuleCollider != null)
                DebugDraw.WireCapsuleCollider(capsuleCollider, capsuleColor);
            if (meshCollider != null)
                DebugDraw.WireMeshCollider(meshCollider, meshColor);
            if (anyCollider != null && anyCollider.TryGetComponent<Collider>(out Collider anyCol))
                DebugDraw.WireCollider(anyCol, colliderColor);


        }
    }
}
