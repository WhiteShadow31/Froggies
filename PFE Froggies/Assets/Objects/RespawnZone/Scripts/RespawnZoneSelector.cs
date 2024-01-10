using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZoneSelector : MonoBehaviour
{
    public static RespawnZoneSelector Instance;

    protected List<RespawnZone> m_respawnZones = new List<RespawnZone>();
    protected int m_index = 0;
    protected List<GameObject> m_players = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddRespawn(RespawnZone zone)
    {
        m_respawnZones.Add(zone);
    }

    public void AddPlayer(GameObject player)
    {
        m_players.Add(player);
    }

    public void TeleportPlayerNextRespawn()
    {
        foreach(GameObject player in m_players)
        {
            m_respawnZones[m_index].RespawnUnsuccessPlayer(player);
        }

        m_index++;
        m_index = m_index >= m_respawnZones.Count ? 0 : m_index;
    }

    public void TeleportPlayerSuccessRespawn()
    {
        foreach (GameObject player in m_players)
        {
            m_respawnZones[m_index].RespawnSuccessPlayer(player);
        }
    }
}
