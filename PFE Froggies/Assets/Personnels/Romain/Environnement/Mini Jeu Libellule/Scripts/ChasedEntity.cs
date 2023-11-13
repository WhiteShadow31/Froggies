using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasedEntity : MonoBehaviour
{
    [SerializeField] CameraEntity _cameraEntity;
    [Space]
    [SerializeField] ChaseGamePoint _startPoint;
    [Space]
    [SerializeField] float _timeBetweenMovement;
    [SerializeField] AnimationCurve _movementCurve;
    [SerializeField] float _movementTime;
    [Space]
    [SerializeField] float _moveIfPlayersDistance;
    [SerializeField] List<GameObject> _cornersPoints = new List<GameObject>(4);
    [SerializeField] float _stuckDistance;
    [SerializeField] Material _normalMat;
    [SerializeField] Material _stuckMat;

    MeshRenderer _meshRenderer;

    float _movementTimer;

    bool _isMoving = false;
    bool _isStopped = true;

    ChaseGamePoint _currentTargetPoint;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _currentTargetPoint = _startPoint;
        transform.position = _currentTargetPoint.transform.position;
    }

    private void Update()
    {
        if (_isStopped)
        {
            Stopped();
        }
        else if (_isMoving)
        {
            MoveToNextTarget();
        }
    }

    IEnumerator ChangePoint()
    {
        GameObject[] nearPoints = {_currentTargetPoint.pointHaut, _currentTargetPoint.pointGauche, _currentTargetPoint.pointBas, _currentTargetPoint.pointDroite };
        List<float> nearPointDistancesFromPlayers = new List<float>(4);

        foreach(GameObject point in nearPoints)
        {
            if (point == null)
            {
                nearPointDistancesFromPlayers.Add(0);
                continue;
            }
                
            // Get distances between players and point
            float[] distanceFromPlayers = new float[_cameraEntity.players.Length];
            for(int i = 0; i < _cameraEntity.players.Length; i++)
            {
                if (_cameraEntity.players[i] == null) continue;
                distanceFromPlayers[i] = Vector3.Distance(point.transform.position, _cameraEntity.players[i].transform.position);                              
            }
            nearPointDistancesFromPlayers.Add(Mathf.Min(distanceFromPlayers));
        }
        
        // Set next target point to safest point arround
        int nextTargetPointIndex = nearPointDistancesFromPlayers.IndexOf(Mathf.Max(nearPointDistancesFromPlayers.ToArray()));
        _currentTargetPoint = nearPoints[nextTargetPointIndex].GetComponent<ChaseGamePoint>();

        yield return new WaitForSeconds(_timeBetweenMovement);

        // Start movement to next target point
        _movementTimer = 0;
        _isMoving = true;
    }

    void MoveToNextTarget()
    {
        if(_movementTimer < _movementTime)
        {
            transform.position = Vector3.Lerp(transform.position, _currentTargetPoint.transform.position, _movementCurve.Evaluate(_movementTimer / _movementTime));
            _movementTimer += Time.deltaTime;
        }
        else
        {
            transform.position = _currentTargetPoint.transform.position;
            _isStopped = true;
            _isMoving = false;
        }
    }

    void Stopped()
    {
        if (_cornersPoints.Contains(_currentTargetPoint.gameObject))
        {
            List<float> distancesFromPlayer = new List<float>();
            foreach(GameObject player in _cameraEntity.players)
            {
                distancesFromPlayer.Add(Vector3.Distance(transform.position, player.transform.position));
            }

            if(Mathf.Min(distancesFromPlayer.ToArray()) <= _stuckDistance)
            {
                _meshRenderer.sharedMaterial = _stuckMat;
                return;
            }
        }

        _meshRenderer.sharedMaterial = _normalMat;

        foreach (GameObject player in _cameraEntity.players)
        {
            if(player == null) continue;

            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceFromPlayer <= _moveIfPlayersDistance)
            {
                _isStopped = false;
                StartCoroutine(ChangePoint());
                return;
            }
        }
    }
}