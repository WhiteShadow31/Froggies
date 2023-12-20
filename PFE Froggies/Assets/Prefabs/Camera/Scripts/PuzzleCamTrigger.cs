using System.Collections.Generic;
using UnityEngine;

public class PuzzleCamTrigger : MonoBehaviour
{
    [SerializeField] Transform _camPositionTransform;

    [Space]
    [SerializeField] List<GameObject> _playersInZone = new List<GameObject>(2);
    [SerializeField] LayerMask _playersLayerMask;

    [Header("Debug")]
    [SerializeField] bool _showDebug = true;
    [SerializeField] LayerMask _cameraDebugGroundLayerMask;
    [SerializeField] Color _debugCenterLineColor = Color.red;
    [SerializeField] Color _debugCornerLinesColor = Color.yellow;
    [SerializeField] Color _debugGroundLinesColor = Color.blue;

    CameraEntity _camEntity;

    void Awake()
    {
        _camEntity = Camera.main.GetComponent<CameraEntity>();
        _camPositionTransform = GetComponentInChildren<Camera>().transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && !_playersInZone.Contains(other.gameObject))
        {
            _playersInZone.Add(other.gameObject);

            if (_playersInZone.Count == 2)
            {
                _camEntity.SetNewCamTargetPoint(_camPositionTransform);
            }
        }     
    }

    void OnTriggerExit(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && _playersInZone.Contains(other.gameObject))
        {
            _playersInZone.Remove(other.gameObject);

            if (_playersInZone.Count == 0)
            {
                _camEntity.SetExplorationCam();
            }
        }
    }

    // ---------------------  Debug ----------------------- //
    void OnDrawGizmos()
    {
        if (_showDebug)
        {
            DrawDebugLines();
        }
    }

    void DrawDebugLines()
    {
        Camera _cam = _camPositionTransform.GetComponent<Camera>();

        // Get camera center point
        Vector3 centerPoint;
        if(Physics.Raycast(_camPositionTransform.position, _camPositionTransform.forward, out RaycastHit hitinfo, 500f, _cameraDebugGroundLayerMask))
        {
            centerPoint = hitinfo.point;
        }
        else
        {
            centerPoint = _camPositionTransform.position + (_camPositionTransform.rotation * _camPositionTransform.forward * 25f);       
        }

        // Draw camera center line
        Gizmos.color = _debugCenterLineColor;
        Gizmos.DrawLine(_camPositionTransform.position, centerPoint);

        // Create lists and calculate camera corners directions
        List<Vector3> camCornersPoints = new List<Vector3>();
        Vector3[] camCornersRays = new Vector3[4];
        _cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, camCornersRays);

        // Get camera corners world point at averrage y pos
        foreach (Vector3 cornerRay in camCornersRays)
        {
            if (Physics.Raycast(_camPositionTransform.position, _camPositionTransform.rotation * cornerRay.normalized, out RaycastHit hitInfo, 500f, _cameraDebugGroundLayerMask))
            {
                camCornersPoints.Add(hitInfo.point);

                // Draw corners lines
                Gizmos.color = _debugCornerLinesColor;
                Gizmos.DrawLine(_camPositionTransform.position, hitInfo.point);
            }
            else
            {
                Vector3 newTempCornerPoint = _camPositionTransform.position + (_camPositionTransform.rotation * cornerRay.normalized * 25f);
                camCornersPoints.Add(newTempCornerPoint);
            }
        }

        Gizmos.color = _debugGroundLinesColor;
        for (int i = 0; i < 4; i++)
        {
            // Set next index
            int nextIndex = i + 1;
            if (i == 3)
            {
                nextIndex = 0;
            }

            // Draw line
            Gizmos.DrawLine(camCornersPoints[i], camCornersPoints[nextIndex]);
        }
    }
}
