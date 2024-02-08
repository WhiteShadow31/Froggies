using System;
using System.Collections;
using UnityEngine;
using UltimateAttributesPack;
using Unity.VisualScripting;

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
    ForceMode _jumpMode = ForceMode.Impulse;
    [Space]
    [SerializeField] LineRenderer _jumpPredictionLine;
    [SerializeField] bool _showTrajectoryLine = true;
    [SerializeField] GameObject _landingPointObject;
    [SerializeField] bool _showLandingPoint = true;
    [Space]
    [SerializeField] float _jumpInteructedIfSitckLessThan = 0.05f;
    [SerializeField] float _timeToChargeJump = 0.5f;
    [SerializeField] AnimationCurve _landingPointSmoothCurve;
    [SerializeField] float _landingPointSmoothSpeed = 0.02f;
    [Space]
    [SerializeField] int _jumpPredictionLinePointCount = 200;
    [SerializeField] float _jumpPredictiontDuration = 5;
    [SerializeField] LayerMask _jumpPredictionLayerMask;

    MeshRenderer _jumpPredictionObjectRenderer;
    public bool IsJumping { get { return _isJumping; } }
    bool _isJumping = false;
    bool _jumpCharged = false;
    bool _wasGroundedLastFrame = false;
    float _jumpMaxLenghtTimer;
    float _currentJumpForceForward;
    float _currentJumpForceUp;
    Vector3 _landingPointLastPosition;
    Vector3 _velocityRef = Vector3.zero;

    [Header("--- DEBUG ---")]
    [SerializeField] bool _showDebug = false;
    [ShowIf("_showDebug", true), SerializeField] Color _groundCheckDebugColor = Color.red;
    [ShowIf("_showDebug", true), SerializeField] Color _tongueDebugColor = Color.blue;
    [ShowIf("_showDebug", true), SerializeField] Color _mountRadiusDebugColor = Color.yellow;
    [ShowIf("_showDebug", true), SerializeField] Color _refreshRotationLineDebugColor = Color.green;

    [Header("--- INPUTS ---")]
    [ShowIf("_showDebug", true)] public bool MoveInput;
    [ShowIf("_showDebug", true)] public Vector2 RotaInput = Vector2.zero;
    [ShowIf("_showDebug", true)] public bool JumpPressInput = false;
    [ShowIf("_showDebug", true)] public bool JumpReleaseInput = false;
    [ShowIf("_showDebug", true)] bool _startTongueAimInput = false;
    public bool StartTongueAimInput { set { _startTongueAimInput = value; } }
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

        JumpPressInput = false;
        JumpReleaseInput = false;

        _jumpPredictionObjectRenderer = _landingPointObject.GetComponent<MeshRenderer>();

        // Set jump prediction color
        SetJumpPredictionColor(playerColor);

        _smPlayer = new StateMachinePlayer(this);
        _smPlayer.Start();
    }

    protected void Update()
    {
        _smPlayer.Update(Time.deltaTime);

        if (EndTongueAimInput)
        {            
            EndTongueAimInput = false;
            UseTongue();
        }

        Rotate();
        ManageJump();
    }
    
    protected void FixedUpdate()
    {
        _smPlayer.FixedUpdate(Time.fixedDeltaTime);

        if (MoveInput && RotaInput != Vector2.zero)
           Move();     
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
        _rigidbodyController.AddPreciseForce(this.transform.forward, _moveForce, _moveMode);
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
        Vector3 up = transform.up;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, _setGroundRotationRaycastLenght, _setGroundRotationRaycastLayer))
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
        // Interrupt jump
        if (RotaInput.magnitude < _jumpInteructedIfSitckLessThan && _jumpCharged)
        {
            ResetJump();
            return;
        }

        _rigidbodyController.StopVelocity();

        // Jump
        Vector3 jumpVector = (transform.forward * _currentJumpForceForward) + (transform.up * _currentJumpForceUp);
        _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);

        SetPredictionRenderer(false);
        ResetJump();
        _isJumping = true;
        _wasGroundedLastFrame = true;
    }

    // =====================================================================================
    //                                   JUMP METHODS 
    // =====================================================================================

    public void ManageJump()
    {
        // Check if player is in jump
        if (_isJumping && !_wasGroundedLastFrame && IsGrounded)
        {
            _rigidbodyController.StopVelocity();
            _isJumping = false;
        }
        _wasGroundedLastFrame = IsGrounded;

        // Predicted jump
        if (IsGrounded && JumpPressInput && RotaInput.magnitude > _jumpInteructedIfSitckLessThan)
        {
            // Display or not the landing point and line
            if (_showTrajectoryLine)
                _jumpPredictionLine.enabled = true;
            if (_showLandingPoint)
                _jumpPredictionObjectRenderer.enabled = true;

            // Charge jump to max if it's not
            if (!_jumpCharged)
                ChargeJump();

            SetJumpForce(_jumpCharged); // Set jump force to current force if jump is charged

            // Show prediction if jump isn't interrupted
            if (RotaInput.magnitude > _jumpInteructedIfSitckLessThan && _jumpCharged)
                ShowJumpPrediction();
            else
                SetPredictionRenderer(false);
        }
        // Set landing point to player position with time and disable it
        else
        {
            _landingPointObject.transform.position = new Vector3(transform.position.x, _landingPointObject.transform.position.y, transform.position.z);
            SetPredictionRenderer(false);
        }
    }

    void ChargeJump()
    {
        if(_jumpMaxLenghtTimer < _timeToChargeJump)
        {
            SetJumpForceToMinOrMax(false); // Set jump force to min

            _jumpMaxLenghtTimer += Time.deltaTime;
        }
        else
        {
            SetJumpForceToMinOrMax(true); // Set jump force to max

            _jumpCharged = true;
            _jumpMaxLenghtTimer = 0; // Reset timer
        } 
    }

    void SetJumpPredictionColor(Color color)
    {
        // Set landing point color
        Material jumpLandingPointMat = _jumpPredictionObjectRenderer.material;
        jumpLandingPointMat.color = playerColor;
        _jumpPredictionObjectRenderer.sharedMaterial = jumpLandingPointMat;

        // Set line color
        Material jumpPredictionLineMat = _jumpPredictionLine.sharedMaterial;
        jumpPredictionLineMat.color = color;
        _jumpPredictionLine.sharedMaterial = jumpPredictionLineMat;    
    }

    void SetJumpForce(bool jumpCharged)
    {
        if (jumpCharged && RotaInput.magnitude >= _jumpInteructedIfSitckLessThan)
        {
            _currentJumpForceForward = Mathf.Lerp(_jumpForceFwd, _longJumpForceFwd, _landingPointSmoothCurve.Evaluate(RotaInput.magnitude) - _jumpInteructedIfSitckLessThan);
            _currentJumpForceUp = Mathf.Lerp(_jumpForceUp, _longJumpForceUp, _landingPointSmoothCurve.Evaluate(RotaInput.magnitude) - _jumpInteructedIfSitckLessThan);
        }
        else
            SetJumpForceToMinOrMax(false);
    }

    void SetJumpForceToMinOrMax(bool toMax)
    {
        if (toMax)
        {
            _currentJumpForceForward = _longJumpForceFwd;
            _currentJumpForceUp = _longJumpForceUp;
        }
        else
        {
            _currentJumpForceForward = _jumpForceFwd;
            _currentJumpForceUp = _jumpForceUp;
        }
    }

    void ResetJump()
    {
        SetJumpForceToMinOrMax(false); // Set jump force to min
       
        _jumpCharged = false; // Disable charged jump
        _jumpMaxLenghtTimer = 0; // Reset timer to charge jump

        JumpPressInput = false;
        JumpReleaseInput = false;
    }

    void SetPredictionRenderer(bool state)
    {
        if(_showTrajectoryLine)
            _jumpPredictionLine.enabled = state;
        if(_showLandingPoint)
            _jumpPredictionObjectRenderer.enabled = state;
    }

    void ShowJumpPrediction()
    {
        SetPredictionRenderer(true);

        // Initialize prediction line
        _jumpPredictionLine.positionCount = _jumpPredictionLinePointCount;
        Vector3 startPosition = transform.position;
        Vector3 lastPoint = startPosition;
        Vector3 jumpVector = (transform.forward * _currentJumpForceForward) + (transform.up * _currentJumpForceUp);
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
        for(int j = 1; j < _jumpPredictionLinePointCount; j++)
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
                _landingPointObject.transform.position = new Vector3(_landingPointObject.transform.position.x, hitInfo.point.y, _landingPointObject.transform.position.z); // Set landing point y to hitinfo y
                _landingPointObject.transform.position = Vector3.SmoothDamp(_landingPointObject.transform.position, hitInfo.point, ref _velocityRef, _landingPointSmoothSpeed); // Smooth landing point position to target position
                _landingPointLastPosition = _landingPointObject.transform.position;

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
        //
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
        _tongueLineRenderer.enabled = true;
        Vector3 hitPosition;

        while (_tongueOutDelay < _tongueOutTime)
        {
            hitPosition = TongueAimPosition();

            _tongueEndTransform.position = Vector3.Lerp(_tongueStartTransform.position, hitPosition, _tongueOutCurve.Evaluate(_tongueOutDelay / _tongueOutTime));

            TongueLine();

            _tongueOutDelay += Time.fixedDeltaTime;
            yield return null;
        }

        while (_tongueInDelay < _tongueInTime)
        {
            hitPosition = TongueAimPosition();

            _tongueEndTransform.position = Vector3.Lerp(hitPosition, _tongueStartTransform.position, _tongueOutCurve.Evaluate(_tongueInDelay / _tongueInTime));

            TongueLine();
            
            _tongueInDelay += Time.fixedDeltaTime;
            yield return null;
        }
        _tongueLineRenderer.enabled = false;
        _tongueOutDelay = 0;
        _tongueInDelay = 0;
        _hasPushedOtherPlayer = false;
        _hasPushedInterractable = false;
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
    }
}

