using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraEntity : MonoBehaviour
{
    [Header("Players")]
    public GameObject _player1;
    public GameObject _player2;

    [Header("Offset Forward")]
    [SerializeField] float _camMinForwardOffset = -10f;
    [SerializeField] float _camMaxForwardOffset = -15f;

    [Header("Zoom / Dezoom")]
    [SerializeField] float _maxZoomYDist = 10;
    [SerializeField] float _maxDezoomYDist = 20;
    [Space]
    [SerializeField] float _startDezoomDistance = 5;
    [SerializeField] float _maxDezoomDistance = 15;

    [Header("Interpolation")]
    [SerializeField] float _camInterpTime = 1;

    Vector3 _velocitySDPosition = Vector3.zero;

    [Header("Debug Gizmos")]
    [SerializeField] bool _drawDebugGizmos = true;

    void Update()
    {
        if(_player1 == null || _player2 == null)
        {
            return;
        }

        float distancePlayers = Vector3.Distance(_player1.transform.position, _player2.transform.position);
        Vector3 playersMidCamPosition = Vector3.Lerp(_player1.transform.position, _player2.transform.position, 0.5f);

        if(distancePlayers > _startDezoomDistance && distancePlayers < _maxDezoomDistance) 
        {
            float percent = Mathf.InverseLerp(_startDezoomDistance, _maxDezoomDistance, distancePlayers);
            float yAddDezoom = Mathf.Lerp(_maxZoomYDist, _maxDezoomYDist, percent);
            float currentForwardDezoom = Mathf.Lerp(_camMinForwardOffset, _camMaxForwardOffset, percent);
            Vector3 newCamPos = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + yAddDezoom, playersMidCamPosition.z + currentForwardDezoom);

            transform.position = Vector3.SmoothDamp(transform.position, newCamPos, ref _velocitySDPosition, _camInterpTime);
        }
        else
        {
            Vector3 newCamPos = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + _maxDezoomYDist, playersMidCamPosition.z + _camMaxForwardOffset);
            transform.position = Vector3.SmoothDamp(transform.position, newCamPos, ref _velocitySDPosition, _camInterpTime);
        }
    }

    public void AddPlayer(GameObject player)
    {       
        if (_player1 == null && _player2 == null)
        {
            _player1 = player;
            return;
        }
        else if(_player1 != null && _player2 == null && player != _player1) { }
        {
            _player2 = player;
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 500f);
        }
    }
}
