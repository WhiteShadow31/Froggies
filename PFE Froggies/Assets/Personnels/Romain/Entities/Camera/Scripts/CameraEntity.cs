using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

public class CameraEntity : MonoBehaviour
{
    [Header("Players")]
    public GameObject _player1;
    public GameObject _player2;

    //[HideInInspector]
    public GameObject[] players = new GameObject[2];

    [Title("Camera Modes", "light blue", "white")]
    [SerializeField] CameraModes _startCameraMode = CameraModes.Exploration;

    [Space]
    [SubTitle("Exploration", "light blue", "white")]

    [Header("Rotation")]
    [SerializeField] Vector3 _camExplorationRotation;

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
    [SerializeField] float _camExplorationPosInterpTime = 0.5f;
    [SerializeField] float _camExplorationRotInterpSpeed = 20;

    [Space]
    [SubTitle("Puzzle", "light green", "white")]

    [Header("Interpolation")]
    [SerializeField] float _camPuzzlePosInterpTime = 0.5f;
    [SerializeField] float _camPuzzleRotInterpSpeed = 20;

    CameraModes _currentCameraMode;
    public enum CameraModes
    {
        Exploration, Puzzle
    }
    Transform _puzzleCameraTargetPoint;
    Vector3 _velocityRef = Vector3.zero;

    [Header("Debug Gizmos")]
    [SerializeField] bool _drawDebugGizmos = true;

    private void Start()
    {
        _currentCameraMode = _startCameraMode;
    }

    void Update()
    {
        if(_player1 == null || _player2 == null)
        {
            return;
        }

        switch(_currentCameraMode)
        {
            case CameraModes.Exploration:
                ExplorationCamera();
                break;
            case CameraModes.Puzzle:
                PuzzleCamera();
                break;
        }
    }

    void ExplorationCamera()
    {
        float distancePlayers = Vector3.Distance(_player1.transform.position, _player2.transform.position);
        Vector3 playersMidCamPosition = Vector3.Lerp(_player1.transform.position, _player2.transform.position, 0.5f);

        if (distancePlayers > _startDezoomDistance && distancePlayers < _maxDezoomDistance)
        {
            float percent = Mathf.InverseLerp(_startDezoomDistance, _maxDezoomDistance, distancePlayers);
            float yAddDezoom = Mathf.Lerp(_maxZoomYDist, _maxDezoomYDist, percent);
            float currentForwardDezoom = Mathf.Lerp(_camMinForwardOffset, _camMaxForwardOffset, percent);

            Vector3 newCamPos = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + yAddDezoom, playersMidCamPosition.z + currentForwardDezoom);
            transform.position = Vector3.SmoothDamp(transform.position, newCamPos, ref _velocityRef, _camExplorationPosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_camExplorationRotation), _camExplorationRotInterpSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 newCamPos = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + _maxDezoomYDist, playersMidCamPosition.z + _camMaxForwardOffset);
            transform.position = Vector3.SmoothDamp(transform.position, newCamPos, ref _velocityRef, _camExplorationPosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_camExplorationRotation), _camExplorationRotInterpSpeed * Time.deltaTime);
        }
    }

    void PuzzleCamera()
    {
        if(_puzzleCameraTargetPoint != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _puzzleCameraTargetPoint.position, ref _velocityRef, _camPuzzlePosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _puzzleCameraTargetPoint.rotation, _camPuzzleRotInterpSpeed * Time.deltaTime);
        }
    }

    public void AddPlayer(GameObject player)
    {        
        if (_player1 == null && _player2 == null)
        {
            _player1 = player;
            players[0] = player;
            return;
        }
        else if(_player1 != null && _player2 == null && player != _player1)
        {
            _player2 = player;
            players[1] = player;
            return;
        }
    }

    public void SetNewCamTargetPoint(Transform target)
    {
        _currentCameraMode = CameraModes.Puzzle;
        _puzzleCameraTargetPoint = target;
    }

    public void SetExplorationCam()
    {
        _currentCameraMode = CameraModes.Exploration;
    }

    private void OnDrawGizmos()
    {
        if (_drawDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 30f);
        }
    }
}
