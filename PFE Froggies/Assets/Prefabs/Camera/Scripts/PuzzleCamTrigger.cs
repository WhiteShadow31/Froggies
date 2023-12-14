using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCamTrigger : MonoBehaviour
{
    CameraEntity _camEntity;
    Transform _camPositionTransform;

    [Space]
    [SerializeField] List<GameObject> _playersInZone = new List<GameObject>(2);
    [SerializeField] LayerMask _playersLayerMask;

    [Header("Debug")]
    [SerializeField] bool _showDebug = true;
    [SerializeField] Color _debugColor = Color.red;

    private void Awake()
    {
        _camEntity = Camera.main.GetComponent<CameraEntity>();
        _camPositionTransform = GetComponentInChildren<Camera>().transform;
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && _playersInZone.Contains(other.gameObject))
        {
            _playersInZone.Remove(other.gameObject);

            if (_playersInZone.Count < 2)
            {
                _camEntity.SetExplorationCam();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showDebug)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = _debugColor;
            Gizmos.DrawWireCube(Vector3.zero, GetComponent<BoxCollider>().size);
        }
    }
}
