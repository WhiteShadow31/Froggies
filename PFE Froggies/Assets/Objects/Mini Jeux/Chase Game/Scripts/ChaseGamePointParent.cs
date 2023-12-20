using System.Collections.Generic;
using UltimateAttributesPack;
using UnityEngine;

public class ChaseGamePointParent : MonoBehaviour
{
    [FunctionButton("Get All Near Points", "GetAllNearPoints", typeof(ChaseGamePointParent))]
    [ReadOnly] [SerializeField] List<GameObject> points = new List<GameObject>();

    void GetAllNearPoints()
    {
        points.Clear();
        foreach(Transform point in GetComponentsInChildren<Transform>())
        {
            if(point.gameObject == gameObject || point.GetComponent<ChaseGamePoint>() == null)
            {
                continue;
            }

            ChaseGamePoint pointScript = point.GetComponent<ChaseGamePoint>();
            pointScript.GetNearPoints();
            points.Add(point.gameObject);
        }
    }
}