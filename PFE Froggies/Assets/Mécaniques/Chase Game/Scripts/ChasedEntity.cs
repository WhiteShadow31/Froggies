using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasedEntity : MonoBehaviour, IInteractableEntity
{
    [SerializeField] CameraEntity _cameraEntity;

    ChaseGamePoint _currentTargetPoint;
    ChaseGamePoint _lastTargetPoint;

    [Header("Other params")]
    [SerializeField] ChaseGamePoint _startPoint;
    [SerializeField] float _detectionDistance;
    [Space]

    [Header("Movement")]
    [SerializeField] float _timeBetweenMovement;
    [SerializeField] AnimationCurve _movementCurve;
    [SerializeField] float _movementTime;
    [SerializeField] float _rotationSpeed;

    float _movementTimer;
    bool _isMoving = false;
    bool _isStopped = true;

    [Header("Stuck")]
    [SerializeField] float _stuckDistance;
    [SerializeField] float _stuckDistanceSafestPointFromPlayer = 3f;
    [SerializeField] MeshRenderer _bodyMeshRenderer;
    [SerializeField] Material _normalMat;
    [SerializeField] Material _stuckMat;

    bool _isStuck;
    public bool IsStuck { get { return _isStuck; } }

    [Header("Eated")]
    [SerializeField] float _tryToEatTime;
    [SerializeField] GameObject[] _destroyedObjectsWhenEated;

    float _tryToEatTimer;
    bool _isTriedToBeEated;
    GameObject _frogFirstEat;

    [Header("Debug gizmos")]
    [SerializeField] bool _drawDebug = true;

    bool _playSound = true;

    private void Awake()
    {
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
            _playSound = false;
            Stopped();
        }
        else if (_isMoving)
        {
            MoveToNextTarget();
        }
        else if (_isStuck)
        {
            _playSound = false;
            Stucked();
        }
    }

    private void FixedUpdate()
    {
        // A player tried to push it
        if (_isTriedToBeEated)
        {
            // Timer waiting for other player to hit it
            if (_tryToEatTimer < _tryToEatTime)
                _tryToEatTimer += Time.fixedDeltaTime;
            else
            {
                _isTriedToBeEated = false;
                _tryToEatTimer = 0;
                _frogFirstEat = null;
            }
        }
    }

    IEnumerator ChangePoint()
    {
        GameObject safestPoint = GetSafestPointArround();

        // Get distances from players
        List<float> safestPointDistanceFromPlayers = new List<float>();
        for (int i = 0; i < _cameraEntity.players.Length; i++)
        {
            if (_cameraEntity.players[i] == null) continue;
            safestPointDistanceFromPlayers.Add(Vector3.Distance(safestPoint.transform.position, _cameraEntity.players[i].transform.position));
        }

        // Set stuck
        if(Mathf.Min(safestPointDistanceFromPlayers.ToArray()) < _stuckDistanceSafestPointFromPlayer)
        {
            SetStuck(true);
            yield break;
        }

        // Set new target point
        _lastTargetPoint = _currentTargetPoint;
        _currentTargetPoint = safestPoint.GetComponent<ChaseGamePoint>();

        yield return new WaitForSeconds(_timeBetweenMovement);

        // Start movement to next target point
        _movementTimer = 0;
        _isMoving = true;
    }

    GameObject GetSafestPointArround()
    {
        GameObject[] nearPoints = _currentTargetPoint.nearPoints.ToArray();
        List<float> nearPointDistancesFromPlayers = new List<float>();

        foreach (GameObject point in nearPoints)
        {
            if (point == null)
            {
                nearPointDistancesFromPlayers.Add(0);
                continue;
            }

            // Get distances between players and point
            float[] distancePointsFromPlayers = new float[_cameraEntity.players.Length];
            for (int i = 0; i < _cameraEntity.players.Length; i++)
            {
                if (_cameraEntity.players[i] == null) continue;
                distancePointsFromPlayers[i] = Vector3.Distance(point.transform.position, _cameraEntity.players[i].transform.position);
            }
            nearPointDistancesFromPlayers.Add(Mathf.Min(distancePointsFromPlayers));
        }

        int nextTargetPointIndex = nearPointDistancesFromPlayers.IndexOf(Mathf.Max(nearPointDistancesFromPlayers.ToArray()));
        return nearPoints[nextTargetPointIndex];
    }

    void SetStuck(bool state)
    {
        if (state)
        {
            _isStuck = true;
            _isStopped = false;
            _isMoving = false;
            _bodyMeshRenderer.sharedMaterial = _stuckMat;            
        }
        else
        {
            _isStuck = false;
            _isStopped = true;
            _isMoving = false;
            _bodyMeshRenderer.sharedMaterial = _normalMat;
        }
    }

    void Stucked()
    {
        // Coder le coup de langue a deux ici

        // Get distances from players
        List<float> distanceFromPlayers = new List<float>();
        for (int i = 0; i < _cameraEntity.players.Length; i++)
        {
            if (_cameraEntity.players[i] == null) continue;
            distanceFromPlayers.Add(Vector3.Distance(transform.position, _cameraEntity.players[i].transform.position));
        }

        // If there is no player in stuck zone
        if (Mathf.Min(distanceFromPlayers.ToArray()) > _stuckDistance)
        {
            GameObject safestPoint = GetSafestPointArround();

            // Get distances between players and safest point
            List<float> safestPointDistanceFromPlayers = new List<float>();
            for (int i = 0; i < _cameraEntity.players.Length; i++)
            {
                if (_cameraEntity.players[i] == null) continue;
                safestPointDistanceFromPlayers.Add(Vector3.Distance(safestPoint.transform.position, _cameraEntity.players[i].transform.position));
            }

            if (Mathf.Min(safestPointDistanceFromPlayers.ToArray()) > _stuckDistanceSafestPointFromPlayer)
            {
                SetStuck(false);
            }
        }
    }

    void MoveToNextTarget()
    {
        if(_movementTimer < _movementTime)
        {
            if(!_playSound && AudioGenerator.Instance != null)
            {
                AudioGenerator.Instance.PlayClipAt(this.transform.position, "ENGM_Libellule_Avance_0" + Random.Range(1, 5));
                _playSound = true;
            }

            transform.position = Vector3.Lerp(transform.position, _currentTargetPoint.transform.position, _movementCurve.Evaluate(_movementTimer / _movementTime));
            _movementTimer += Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(_currentTargetPoint.transform.position - _lastTargetPoint.transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed);
        }
        else
        {
            _playSound = false;

            transform.position = _currentTargetPoint.transform.position;
            _isStopped = true;
            _isMoving = false;
        }
    }

    void Stopped()
    {
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

    // If chased entity is hitted bu tongue
    public void Push(Vector3 dir, float force, GameObject frog)
    {
        // Has already been hit
        if (_isTriedToBeEated && _frogFirstEat != frog)
        {
            foreach(GameObject obj in _destroyedObjectsWhenEated)
            {
                obj.SetActive(false);
            }

            if (ParticlesGenerator.Instance != null)
                ParticlesGenerator.Instance.PlayLibelluleDeath(this.transform.position);
        }
        // 1st time hit
        else
        {
            _isTriedToBeEated = true;
            _tryToEatTimer = 0;
            _frogFirstEat = frog;
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