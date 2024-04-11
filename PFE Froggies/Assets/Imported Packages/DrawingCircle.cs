using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCircle : MonoBehaviour
{
    public float radius = 7;
    public int nbrPoints = 12;
    public Color color = Color.red;

    private void OnDrawGizmos()
    {
        List<Vector3> positions = new List<Vector3>();
        
        for(int i = 0; i < nbrPoints; i++)
        {
            Vector3 pos = Vector3.zero;
            pos.x = Mathf.Sin(i * 360 / nbrPoints * Mathf.Deg2Rad);
            pos.z = Mathf.Cos(i * 360 / nbrPoints * Mathf.Deg2Rad);
            pos = pos.normalized * radius;

            positions.Add(pos + this.transform.position);
        }

        for (int j = 0; j < positions.Count; j++)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(positions[j], 0.1f);

            if(j < positions.Count - 1)
                Gizmos.DrawLine(positions[j], positions[j+1]);
            else
                Gizmos.DrawLine(positions[j], positions[0]);
        }
    }
}
