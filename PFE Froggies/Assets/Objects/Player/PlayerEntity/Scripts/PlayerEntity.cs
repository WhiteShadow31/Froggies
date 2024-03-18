using System;
using System.Collections;
using UnityEngine;
using UltimateAttributesPack;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerEntity : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;
    SimpleRigidbody _rigidbodyController;
    StateMachinePlayer _smPlayer;

    [Header("--- ROTATION ---")]
    [SerializeField] Camera _camera;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [Space]
    [SerializeField] LayerMask _setGroundRotationRaycastLayer;
    [SerializeField] float _setGroundRotationRaycastLenght = 1;
    [SerializeField] float _rotationSmoothSpeed = 2;
    float _turnSmoothVelocity;

    [Header("--- GROUND CHECK ---")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Vector3 _groundRadius;
    [SerializeField] LayerMask _groundMask;
    public bool IsGrounded { get { return LookGrounded(); } }

    [Header("--- MODEL ---")]
    public Transform model;
    [HideInInspector] public Color playerColor;

    [Header("--- MOVEMENT ---")]
    [SerializeField] float _moveForce = 1;
    [SerializeField] ForceMode _moveMode = ForceMode.Impulse;
    [SerializeField] float _startMoveAfter;
    float _startMoveTimer;
    bool _isMoving;

    [Header("--- TONGUE ---")]
    [SerializeField] Transform _tongueStartTransform;
    [SerializeField] Transform _tongueEndTransform;
    [SerializeField] float _tongueMaxLenght = 5f;
    [Tooltip("Raycast and detect layer mask")]
    [SerializeField] LayerMask _tongueLayerMask;
    [Space]
    [SerializeField] float _tongueInTime = 0.15f;
    [SerializeField] float _tongueOutTime = 0.15f;
    [SerializeField] AnimationCurve _tongueOutCurve;
    [SerializeField] AnimationCurve _tongueInCurve;
    [Space]
    [SerializeField] LineRenderer _tongueLineRenderer;
    [SerializeField] float _tongueHitRadius = 0.3f;
    [SerializeField] float _tongueHitForce = 10f;
    [Tooltip("Cast sphere around the hit point and detect layer mask")]
    [SerializeField] LayerMask _tongueHitLayerMask;
    float _tongueOutDelay = 0, _tongueInDelay = 0;
    bool _canUseTongue = true;

    [Header("--- MOUNT OTHER ---")]
    public Transform onFrogTransform;
    [SerializeField] float _mountRadius = 3f;
    [SerializeField] LayerMask _playerLayer;
    Transform _otherPlayerMountTransform = null;
    public Transform GetMountTransform { get { return _otherPlayerMountTransform; } }

    [Header("--- JUMP ---")]
    [SerializeField] float _jumpForceUp = 1;
    [SerializeField] float _jumpForceFwd = 1;
    [Space]
    [SerializeField] float _longJumpForceUp = 1;
    [SerializeField] float _longJumpForceFwd = 2;
    [SerializeField] ForceMode _jumpMode = ForceMode.Impulse;
    [Space]
    [SerializeField] Transform _jumpCollisionDetectionTransform;
    [SerializeField] int _jumpCollisionDetectionEveryTickCount;
    [SerializeField] float _jumpCollisionDetectionOffset;
    [SerializeField] LayerMask _jumpCollisionDetectionLayerMask;
    int _jumpCollisionDetectionTickCount;
    [Space]
    [SerializeField] bool _showTrajectoryLine = true;
    [SerializeField, ShowIf(nameof(_showTrajectoryLine), true)] LineRenderer _jumpPredictionLine;
    [SerializeField, ShowIf(nameof(_showTrajectoryLine), true)] int _jumpPredictionLinePointCount = 200;
    [SerializeField, ShowIf(nameof(_showTrajectoryLine), true)] float _jumpPredictiontDuration = 5;
    [SerializeField, ShowIf(nameof(_showTrajectoryLine), true)] LayerMask _jumpPredictionLayerMask;
    bool _isJumping;
    public bool IsJumping { get { return _isJumping; } }
    bool _wasGroundedLastFrame = false;

    [Header("--- DEBUG ---")]
    [SerializeField] bool _showDebug = false;
    [ShowIf("_showDebug", true), SerializeField] Color _groundCheckDebugColor = Color.red;
    [ShowIf("_showDebug", true), SerializeField] Color _tongueDebugColor = Color.blue;
    [ShowIf("_showDebug", true), SerializeField] Color _mountRadiusDebugColor = Color.yellow;
    [ShowIf("_showDebug", true), SerializeField] Color _refreshRotationLineDebugColor = Color.green;
    [ShowIf("_showDebug", true), SerializeField] Color _jumpCollisionDetectionDebugColor = Color.cyan;

    [Header("--- INPUTS ---")]
    [ShowIf("_showDebug", true)] public bool MoveInput;
    [ShowIf("_showDebug", true)] public Vector2 RotaInput = Vector2.zero;
    [ShowIf("_showDebug", true)] public bool SmallJumpInput = false;
    [ShowIf("_showDebug", true)] public bool LongJumpInput;
    [ShowIf("_showDebug", true)] bool _startTongueAimInput = false;
    public bool StartTongueAimInput { get { return _startTongueAimInput; } set { _startTongueAimInput = value; } }
    [ShowIf("_showDebug", true)] bool _endTongueAimInput = false;
    public bool EndTongueAimInput { get { return _endTongueAimInput; } set { _endTongueAimInput = value; } }
    [ShowIf("_showDebug", true)] public bool MountInput;

    bool _initialized = false;    
    bool _isOnFrog = false;
    bool _hasPushedOtherPlayer = false;
    bool _hasPushedInterractable = false;

    // =====================================================================================
    //                                   UNITY METHODS 
    // =====================================================================================

    protected void Awake()
    {
        InitComponents();
    }

    protected void Start()
    {
        InitComponents();
        if (_camera == null)
            _camera = Camera.main;

        _smPlayer = new StateMachinePlayer(this);
        _smPlayer.Start();
    }

    protected void Update()
    {
        Debug.Log(IsGrounded);
        _smPlayer.Update(Time.deltaTime);

        // If the tongue button is pressed
        if (StartTongueAimInput && _canUseTongue)
        {
            StartTongueAimInput = false;           
            UseTongue(); // Use tongue
        }

        Rotate();
        ManageJump();
    }
    
    protected void FixedUpdate()
    {
        _smPlayer.FixedUpdate(Time.fixedDeltaTime);

        Move();

        if (_isJumping)
        {
            ManageJumpCollision();
        }
        else
        {
            foreach (Collider col in transform.GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }
        }
    }

    // =====================================================================================
    //                                   INITIALISATION METHODS 
    // =====================================================================================

    void InitComponents()
    {
        if (!_initialized) // If it hasnt been initialized
        {
            InitSimpleRigidbody(); // Get the rigidbody 
            InitGroundController(); // Create a grounded controller
            _initialized = true;
        }
    }
    void InitSimpleRigidbody()
    {
        // Get the rigidbody or create it if there is none
        _rigidbodyController = this.transform.TryGetComponent<SimpleRigidbody>(out SimpleRigidbody rb) ? rb : this.transform.AddComponent<SimpleRigidbody>();
    }
    void InitGroundController()
    {
        if (_groundCheck == null)
        {
            GameObject go = new GameObject("GroundCheck");
            go.transform.parent = this.transform;
            go.transform.position = Vector3.zero;
            _groundCheck = go.transform;
        }
    }

    // =====================================================================================
    //                                   MOVEMENT METHODS 
    // =====================================================================================

    bool LookGrounded()
    {
        Collider[] cols = Physics.OverlapBox(_groundCheck.position, _groundRadius, Quaternion.identity, _groundMask); //Physics.OverlapSphere(_groundCheck.position, _groundRadius, _groundMask);
        bool grounded = false;

        foreach (Collider col in cols)
        {
            if ((col.transform != this.transform))
            {
                grounded = true;
            }
        }
        return grounded;
    }

    public void Move()
    {
        // Reset timer if stick is zero
        if(RotaInput.magnitude == 0)
            _startMoveTimer = 0;

        if(RotaInput.magnitude != 0 && IsGrounded)
        {
            // Increase timer if it's not finished and stick is not null
            if(_startMoveTimer < _startMoveAfter)
                _startMoveTimer += Time.fixedDeltaTime;
            else // If timer is finished, move
                _rigidbodyController.AddPreciseForce(this.transform.forward, _moveForce, _moveMode);
        }
    }

    public void Rotate()
    {
        // Get rotation Y (stick)
        Vector3 dir = new Vector3(RotaInput.x, 0, RotaInput.y).normalized;
        if (dir.magnitude >= 0.1f)
        {
            Vector3 newRotation = transform.eulerAngles;
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            newRotation.y = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.eulerAngles = newRotation;
        }

        // Get new up
        Vector3 up = Vector3.up;
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, _setGroundRotationRaycastLenght, _setGroundRotationRaycastLayer))
        {
            up = hitInfo.normal;      
        }

        // Get new forward and set rotation
        Vector3 forward = transform.forward.normalized - up * Vector3.Dot(transform.forward.normalized, up);
        Quaternion targetRotation = Quaternion.LookRotation(forward.normalized, up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSmoothSpeed);
    }

    public void Jump()
    {
        _rigidbodyController.StopVelocity();
        
        Vector3 jumpForce = (transform.forward * _jumpForceFwd) + (Vector3.up * _jumpForceUp);
        if (LongJumpInput)
            jumpForce = (transform.forward * _longJumpForceFwd) + (Vector3.up * _longJumpForceUp);

        // Jump
        _rigidbodyController.AddForce(jumpForce.normalized, jumpForce.magnitude, _jumpMode);

        ResetJump();
        _isJumping = true;
        _wasGroundedLastFrame = true;
    }

    // =====================================================================================
    //                                   JUMP METHODS 
    // =====================================================================================

    public void ManageJump()
    {
        //Check if player is in jump
        if (_isJumping && !_wasGroundedLastFrame && IsGrounded)
        {
            _rigidbodyController.StopVelocity();
            _isJumping = false;
        }
        _wasGroundedLastFrame = IsGrounded;

        if (_showTrajectoryLine)
        {
            _jumpPredictionLine.enabled = true;
            ShowJumpPrediction();
        }
        else
            _jumpPredictionLine.enabled = false;
    }

    void ManageJumpCollision()
    {
        if(_jumpCollisionDetectionTickCount == _jumpCollisionDetectionEveryTickCount && !IsGrounded)
        {
            if (Physics.Raycast(_jumpCollisionDetectionTransform.position, Vector3.down, transform.localScale.y / 2 + _jumpCollisionDetectionOffset, _jumpCollisionDetectionLayerMask))
            {
                foreach(Collider col in transform.GetComponentsInChildren<Collider>())
                {                    
                    col.enabled = false;
                }
            }
            else
            {
                foreach (Collider col in transform.GetComponentsInChildren<Collider>())
                {
                    col.enabled = true;
                }
            }
            _jumpCollisionDetectionTickCount = 0;
        }
        _jumpCollisionDetectionTickCount++;
    }

    void ResetJump()
    {
        SmallJumpInput = false;
        LongJumpInput = false;
    }

    void ShowJumpPrediction()
    {
        // Initialize prediction line
        _jumpPredictionLine.positionCount = _jumpPredictionLinePointCount;
        Vector3 startPosition = transform.position;
        Vector3 lastPoint = startPosition;
        Vector3 jumpVector = (transform.forward * _longJumpForceFwd) + (transform.up * _longJumpForceUp);
        Vector3 startVelocity = jumpVector / _rigidbodyController.Mass;
        float timeStep = _jumpPredictiontDuration / _jumpPredictionLinePointCount;
        _jumpPredictionLine.SetPosition(0, startPosition);

        // Calculate falling gravity prediction points
        Vector3[] fallingTrajectoryPoints = new Vector3[_jumpPredictionLinePointCount];
        fallingTrajectoryPoints[0] = startPosition;
        for (int g = 1; g < _jumpPredictionLinePointCount; g++)
        {
            float fallingTimeOffset = timeStep * g;
            Vector3 fallingPredictionPoint = startPosition + startVelocity * fallingTimeOffset - (Vector3.up * -0.5f * -_rigidbodyController.FallingGravity * fallingTimeOffset * fallingTimeOffset);
            fallingTrajectoryPoints[g] = fallingPredictionPoint;
        }

        // Get highest point of falling gravity trajectory points
        float[] fallingPointsY = new float[_jumpPredictionLinePointCount];
        fallingPointsY[0] = startPosition.y;
        for (int j = 1; j < _jumpPredictionLinePointCount; j++)
        {
            fallingPointsY[j] = fallingTrajectoryPoints[j].y;
        }
        float fallingTrajectoryHighPoint = Mathf.Max(fallingPointsY);
        int indexOfFallingTrajectoryHighPoint = Array.IndexOf(fallingPointsY, fallingTrajectoryHighPoint);

        // Calculate trajectory line
        bool falling = false;
        int fallIndex = indexOfFallingTrajectoryHighPoint;
        for (int i = 1; i < _jumpPredictionLinePointCount; i++)
        {
            float timeOffset = timeStep * i;
            Vector3 currentPoint;

            // Calculate current point if the trajectory is falling or not
            if (!falling)
                currentPoint = startPosition + startVelocity * timeOffset - (Vector3.up * -0.5f * -_rigidbodyController.NormalGravity * timeOffset * timeOffset);
            else
                currentPoint = startPosition + startVelocity * timeOffset - (Vector3.up * -0.5f * -_rigidbodyController.FallingGravity * timeOffset * timeOffset);

            // If trajectory is falling, get point of falling trajectory
            if (lastPoint.y > currentPoint.y)
            {
                falling = true;
                if (fallingTrajectoryPoints[fallIndex + 1] != null)
                {
                    Vector3 DirToNext = fallingTrajectoryPoints[fallIndex + 1] - fallingTrajectoryPoints[fallIndex];
                    currentPoint = lastPoint + DirToNext;
                    fallIndex++;
                }
                else
                    return;
            }

            _jumpPredictionLine.SetPosition(i, currentPoint); // Set new point in line renderer

            // If the trajectory hit something
            if (Physics.Linecast(lastPoint, currentPoint, out RaycastHit hitInfo, _jumpPredictionLayerMask))
            {
                _jumpPredictionLine.SetPosition(i, hitInfo.point); // Set last line renderer point to hit point
                _jumpPredictionLine.positionCount = i + 1;
                return;
            }
            lastPoint = currentPoint;
        }
    }

    // =====================================================================================
    //                                   TONGUE METHODS 
    // =====================================================================================

    /// <summary>
    /// Draw a raycast to try to hit an object, if not then return a position in front
    /// </summary>
    /// <returns> Vector3 position </returns>
    public Vector3 TongueAimPosition()
    {
        if (Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
        {
            if(hit.transform.TryGetComponent<PlayerEntity>(out  PlayerEntity otherPlayer) && !_hasPushedOtherPlayer)
            {
                otherPlayer.PushPlayer(transform.forward, _tongueHitForce);
                _hasPushedOtherPlayer = true;
            }
            if(hit.transform.TryGetComponent<IInteractableEntity>(out  IInteractableEntity otherInteractable) && !_hasPushedInterractable)
            {
                otherInteractable.Push(transform.forward, _tongueHitForce, this.gameObject);
                _hasPushedInterractable = true;
            }
            return hit.point;
        }
        else         
            return _tongueStartTransform.position + (transform.forward * _tongueMaxLenght);
    }

    IEnumerator UseTongueCoroutine()
    {
        _canUseTongue = false;
        _tongueLineRenderer.enabled = true;
        Vector3 hitPosition;

        while (_tongueOutDelay < _tongueOutTime)
        {
            hitPosition = TongueAimPosition();

            _tongueEndTransform.position = Vector3.Lerp(_tongueStartTransform.position, hitPosition, _tongueOutCurve.Evaluate(_tongueOutDelay / _tongueOutTime));

            TongueLine();

            _tongueOutDelay += Time.deltaTime;
            yield return null;
        }

        while (_tongueInDelay < _tongueInTime)
        {
            hitPosition = TongueAimPosition();

            _tongueEndTransform.position = Vector3.Lerp(hitPosition, _tongueStartTransform.position, _tongueOutCurve.Evaluate(_tongueInDelay / _tongueInTime));

            TongueLine();
            
            _tongueInDelay += Time.deltaTime;
            yield return null;
        }
        _tongueLineRenderer.enabled = false;
        _tongueOutDelay = 0;
        _tongueInDelay = 0;
        _hasPushedOtherPlayer = false;
        _hasPushedInterractable = false;
        _canUseTongue = true;
    }
    void TongueLine()
    {
        // Set tongueLineRenderer point position
        _tongueLineRenderer.SetPosition(0, _tongueStartTransform.position);
        _tongueLineRenderer.SetPosition(1, _tongueEndTransform.position);
    }

    public void UseTongue()
    {
        StartCoroutine(UseTongueCoroutine());
    }

    // =====================================================================================
    //                                   MOUNT METHODS 
    // =====================================================================================
    public bool TryMount()
    {
        if (!_isOnFrog) // Is it not on a frog ?
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _mountRadius, _playerLayer); // Look for frogs

            foreach (Collider col in colliders)
            {
                // Get a player
                if (col.TryGetComponent<PlayerEntity>(out PlayerEntity otherPlayerEntity) && otherPlayerEntity != this) // Is it a frog 
                {
                    if (!otherPlayerEntity._isOnFrog)
                    {
                        _otherPlayerMountTransform = otherPlayerEntity.onFrogTransform;
                        Debug.Log(this.name + "mount on "+_otherPlayerMountTransform.parent.name);

                        // Set ignore collision between players to true
                        Physics.IgnoreCollision(GetComponent<Collider>(), _otherPlayerMountTransform.parent.GetComponent<Collider>(), true);

                        _isOnFrog = true;
                        MountInput = false;
                        return true;
                    }
                }
            }
        }
        MountInput = false;
        return false;
    }

    public void StopMount()
    {
        if (_isOnFrog)
        {
            // Set ignore collision between players to false
            Physics.IgnoreCollision(GetComponent<Collider>(), _otherPlayerMountTransform.parent.GetComponent<Collider>(), false);

            _otherPlayerMountTransform = null;

            _isOnFrog = false;

            MountInput = false;
        }
    }

    public void PushPlayer(Vector3 dir, float force)
    {
        _rigidbodyController.AddForce(dir, force, ForceMode.Impulse);
    }

    // =====================================================================================
    //                                   RESPAWN METHODS 
    // =====================================================================================
    public void Respawn(Vector3 pos)
    {
        this.transform.position = pos;
        _rigidbodyController.StopVelocity();
    }

    // =====================================================================================
    //                                   GIZMOS METHODS 
    // =====================================================================================

    void OnDrawGizmos()
    {
        // Draw ground check debug
        Gizmos.color = _groundCheckDebugColor;
        Gizmos.DrawCube(_groundCheck.position, _groundRadius);

        // Draw tongue debug line
        Gizmos.color = _tongueDebugColor;
        Gizmos.DrawLine(_tongueStartTransform.position, (this.transform.forward * _tongueMaxLenght) + _tongueStartTransform.position);

        // Draw mount radius debug
        Gizmos.color = _mountRadiusDebugColor;
        Gizmos.DrawWireSphere(transform.position, _mountRadius);

        // Draw refresh rotation debug line
        Gizmos.color = _refreshRotationLineDebugColor;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * _setGroundRotationRaycastLenght));

        // Draw jump collision detection line
        Gizmos.color = _jumpCollisionDetectionDebugColor;
        Gizmos.DrawLine(_jumpCollisionDetectionTransform.position, _jumpCollisionDetectionTransform.position + (Vector3.down * _jumpCollisionDetectionOffset));
    }
}

