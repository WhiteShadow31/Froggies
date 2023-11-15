using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

public class ChaseGamePoint : MonoBehaviour
{
    [FunctionButton("Get Near Points", "GetNearPoints", typeof(ChaseGamePoint))]
    public GameObject nearPointsParent;
    [SerializeField] LayerMask getNearPointsLayerMask;
    [SerializeField] float getNearPointRadius = 10f;

    public List<GameObject> nearPoints = new List<GameObject>();

    [SerializeField] bool drawDebugLines, drawDebugSpheres;
    [SerializeField] Color debugColor = Color.red;

    void GetNearPoints()
    {
        List<GameObject> nearpoints = new List<GameObject>();
        foreach(Transform point in nearPointsParent.GetComponentsInChildren<Transform>())
        {
            if(Vector3.Distance(transform.position, point.position) <= getNearPointRadius)
            {
                if (Physics.Raycast(transform.position, point.position - transform.position, out RaycastHit hit, getNearPointRadius, getNearPointsLayerMask))
                {
                }
                else
                {
                    nearpoints.Add(point.gameObject);
                }
            }
        }

        nearPoints.Clear();
        nearPoints = nearpoints;
    }

    private void OnDrawGizmos()
    {
        if (drawDebugLines)
        {
            Gizmos.color = debugColor;
            foreach(GameObject point in nearPoints)
            {
                if(Vector3.Distance(transform.position, point.transform.position) <= getNearPointRadius)
                {
                    Gizmos.DrawLine(transform.position, point.transform.position);
                }
            }
        }
        if (drawDebugSpheres)
        {
            Gizmos.DrawWireSphere(transform.position, getNearPointRadius);
        }
    }
}