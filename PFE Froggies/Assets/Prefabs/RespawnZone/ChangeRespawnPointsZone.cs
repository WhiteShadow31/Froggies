using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeRespawnPointsZone : MonoBehaviour
{
    [SerializeField] RespawnZone _respawnZone;
    [SerializeField] LayerMask _playerLayerMask;
    [SerializeField] Vector3[] _respawnPoints = new Vector3[2];

    //private void OnTriggerEnter(Collider other)
    //{
    //    if ((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
    //    {
    //        _respawnZone.respawnPoints = _respawnPoints;
    //    }
    //}







}
