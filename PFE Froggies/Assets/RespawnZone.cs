using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnZone : MonoBehaviour
{
    protected int _index = 0;
    public List<Transform> respawnPoints = new List<Transform>();

    private void Start()
    {
        this.transform.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(respawnPoints.Count > 0)
        {
            if(other.transform.TryGetComponent<SmallFrogEntity>(out SmallFrogEntity frog))
            {
                frog.transform.position = respawnPoints[_index].position;
                _index++;
                if(_index >= respawnPoints.Count)
                    _index = 0;
            }

        }
    }
}
