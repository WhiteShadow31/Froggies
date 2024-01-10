using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnZone : MonoBehaviour
{
    protected int m_unsuccessIndex = 0;
    public List<Transform> unsuccessRespawnPoints = new List<Transform>();
    protected int m_successIndex = 0;
    public List<Transform> successRespawnPoints = new List<Transform>();

    protected List<SmallFrogEntity> m_playersSuccess = new List<SmallFrogEntity>();

    private void Start()
    {
        this.transform.GetComponent<BoxCollider>().isTrigger = true;

        if(RespawnZoneSelector.Instance != null)
        {
            RespawnZoneSelector.Instance.AddRespawn(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // It is a frog
        if (other.transform.TryGetComponent<SmallFrogEntity>(out SmallFrogEntity frog))
        {
            // Player frog has been registered as succeeded the zone
            if (m_playersSuccess.Contains(frog))
            {
                // Respawn to success pos
                if(successRespawnPoints.Count > 0)
                {
                    // Look if index is outside list
                    m_successIndex = m_successIndex >= successRespawnPoints.Count ? 0 : m_successIndex;

                    frog.transform.position = successRespawnPoints[m_successIndex].position;
                    m_successIndex++;
                }
            }
            // hasnt been registered
            else
            {
                // Respawn to unsuccess pos
                if (unsuccessRespawnPoints.Count > 0)
                {
                    // Look if index is outside list
                    m_unsuccessIndex = m_unsuccessIndex >= unsuccessRespawnPoints.Count ? 0 : m_unsuccessIndex;

                    frog.transform.position = unsuccessRespawnPoints[m_unsuccessIndex].position;
                    m_unsuccessIndex++;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Color col = Color.red;
        col.a = 0.3f;
        Gizmos.color = col;

        Gizmos.DrawCube(Vector3.zero, GetComponent<BoxCollider>().size);
    }

    public void AddFrogToSuccess(SmallFrogEntity frog)
    {
        if(!m_playersSuccess.Contains(frog))
            m_playersSuccess.Add(frog);
    }

    public void RespawnSuccessPlayer(GameObject frog)
    {
        // Respawn to success pos
        if (successRespawnPoints.Count > 0)
        {
            // Look if index is outside list
            m_successIndex = m_successIndex >= successRespawnPoints.Count ? 0 : m_successIndex;

            frog.transform.position = successRespawnPoints[m_successIndex].position;
            m_successIndex++;
        }
    }

    public void RespawnUnsuccessPlayer(GameObject frog)
    {
        Debug.Log("Try to respawn");
        // Respawn to unsuccess pos
        if (unsuccessRespawnPoints.Count > 0)
        {
            // Look if index is outside list
            m_unsuccessIndex = m_unsuccessIndex >= unsuccessRespawnPoints.Count ? 0 : m_unsuccessIndex;

            frog.transform.position = unsuccessRespawnPoints[m_unsuccessIndex].position;
            m_unsuccessIndex++;
        }
    }
}
