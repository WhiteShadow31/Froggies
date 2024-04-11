using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnZoneDetector : MonoBehaviour
{
    protected RespawnZone m_respawnZone;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        m_respawnZone = this.transform.parent.GetComponent<RespawnZone>();
        this.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<PlayerEntity>(out PlayerEntity frog))
            m_respawnZone.AddFrogToSuccess(frog);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Color col = Color.yellow;
        col.a = 0.3f;
        Gizmos.color = col;

        Gizmos.DrawCube(Vector3.zero, GetComponent<BoxCollider>().size);
    }
}
