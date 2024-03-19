using System.Collections.Generic;
using UnityEngine;
using DrawingTool.Attribute;

namespace DrawingTool
{
    public class DebugDrawPathTension : MonoBehaviour
    {
        [Header("Paths")]
        public bool pathLoop = false;
        [Space]
        public bool simplePath = false;
        [ShowIf("simplePath", true)]
        [SerializeField] Color _pathColor = Color.black;

        [ShowIf("simplePath", false)]
        [SerializeField] Gradient _tensionGradient;
        [ShowIf("simplePath", false)]
        [SerializeField] float _minimalDistance = 0;
        [ShowIf("simplePath", false)]
        [SerializeField] float _maximalDistance = 3;

        private void OnDrawGizmos()
        {
            List<Transform> childs = new List<Transform>();
            foreach (Transform child in this.transform)
            {
                childs.Add(child);
            }

            if (simplePath)
                DebugDraw.Path(childs, _pathColor, pathLoop);
            else
                DebugDraw.PathTension(childs, _tensionGradient, _minimalDistance, _maximalDistance, pathLoop);
        }
    }
}
