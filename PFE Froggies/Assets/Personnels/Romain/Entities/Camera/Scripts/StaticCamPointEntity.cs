using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamPointEntity : MonoBehaviour
{
    [SerializeField] CameraEntity camEntity;
    [SerializeField] Transform camPositionTransform;
    [Space]
    [SerializeField] List<GameObject> playersInZone = new List<GameObject>(2);
    [SerializeField] LayerMask playersLayerMask;

    [Header("Debug")]
    [SerializeField] bool showDebug = true;
    [SerializeField] Color debugColor = Color.red;

    private void Awake()
    {
        camEntity = Camera.main.GetComponent<CameraEntity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if((playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && !playersInZone.Contains(other.gameObject))
        {
            playersInZone.Add(other.gameObject);

            if (playersInZone.Count == 2)
            {
                camEntity.SetNewCamTargetPoint(camPositionTransform);
            }
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && playersInZone.Contains(other.gameObject))
        {
            playersInZone.Remove(other.gameObject);

            if (playersInZone.Count < 2)
            {
                camEntity.SetExplorationCam();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
        }
    }
}
