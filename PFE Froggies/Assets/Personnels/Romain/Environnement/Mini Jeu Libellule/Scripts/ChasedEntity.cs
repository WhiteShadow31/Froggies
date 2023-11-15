using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasedEntity : MonoBehaviour
{
    [SerializeField] CameraEntity _cameraEntity;

    MeshRenderer _meshRenderer;
    ChaseGamePoint _currentTargetPoint;

    [Header("Other params")]
    [SerializeField] ChaseGamePoint _startPoint;
    [SerializeField] float _detectionDistance;
    [Space]

    [Header("Movement")]
    [SerializeField] float _timeBetweenMovement;
    [SerializeField] AnimationCurve _movementCurve;
    [SerializeField] float _movementTime;

    float _movementTimer;
    bool _isMoving = false;
    bool _isStopped = true;

    [Header("Stuck")]
    [SerializeField] float _stuckDistance;
    [SerializeField] float _stuckDistanceSafestPointFromPlayer = 3f;
    [SerializeField] Material _normalMat;
    [SerializeField] Material _stuckMat;

    bool _isStuck;

    [Header("Debug gizmos")]
    [SerializeField] bool _drawDebug = true;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _cameraEntity = Camera.main.GetComponent<CameraEntity>();
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
        GameObject[] nearPoints = _currentTargetPoint.nearPoints.ToArray();
        List<float> nearPointDistancesFromPlayers = new List<float>();

        foreach(GameObject point in nearPoints)
        {
            if (point == null)
            {
                nearPointDistancesFromPlayers.Add(0);
                continue;
            }
                
            // Get distances between players and point
            float[] distancePointsFromPlayers = new float[_cameraEntity.players.Length];
            for(int i = 0; i < _cameraEntity.players.Length; i++)
            {
                if (_cameraEntity.players[i] == null) continue;
                distancePointsFromPlayers[i] = Vector3.Distance(point.transform.position, _cameraEntity.players[i].transform.position);                              
            }
            nearPointDistancesFromPlayers.Add(Mathf.Min(distancePointsFromPlayers));
        }

        // Get distances from players
        float[] distanceFromPlayers = new float[_cameraEntity.players.Length];
        for (int i = 0; i < _cameraEntity.players.Length; i++)
        {
            if (_cameraEntity.players[i] == null) continue;
            distanceFromPlayers[i] = Vector3.Distance(transform.position, _cameraEntity.players[i].transform.position);
        }

        // Set next target point to safest point arround
        float safestPointDistanceFromPlayer = Mathf.Max(nearPointDistancesFromPlayers.ToArray());

        // Set stuck
        if(safestPointDistanceFromPlayer < _stuckDistanceSafestPointFromPlayer)
        {
            SetStuck(true);
            _isStopped = true;
            yield break;
        }
        else if(_isStuck && Mathf.Max(distanceFromPlayers) > _stuckDistance)
        {
            SetStuck(false);
        }

        int nextTargetPointIndex = nearPointDistancesFromPlayers.IndexOf(Mathf.Max(nearPointDistancesFromPlayers.ToArray()));
        _currentTargetPoint = nearPoints[nextTargetPointIndex].GetComponent<ChaseGamePoint>();

        yield return new WaitForSeconds(_timeBetweenMovement);

        // Start movement to next target point
        _movementTimer = 0;
        _isMoving = true;
    }

    void SetStuck(bool state)
    {
        if (state)
        {
            _isStuck = true;
            _meshRenderer.sharedMaterial = _stuckMat;
        }
        else
        {
            _isStuck = false;
            _meshRenderer.sharedMaterial = _normalMat;
        }
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
        if (_currentTargetPoint.nearPoints.Count <= 2)
        {
            List<float> distancesFromPlayer = new List<float>();
            foreach(GameObject player in _cameraEntity.players)
            {
                distancesFromPlayer.Add(Vector3.Distance(transform.position, player.transform.position));
            }
        }

        _meshRenderer.sharedMaterial = _normalMat;

        foreach (GameObject player in _cameraEntity.players)
        {
            if(player == null) continue;

            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceFromPlayer <= _detectionDistance)
            {
                _isStopped = false;
                StartCoroutine(ChangePoint());
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _stuckDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionDistance);
        }
    }
}