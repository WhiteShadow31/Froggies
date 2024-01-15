using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UltimateAttributesPack;


public class PlayerEntity : LivingEntity
{
    [HideInInspector] public PlayerController controller;

    [Header("--- MODEL ---")]
    public Transform model;
    [HideInInspector] public Color playerColor;

    [Header("--- TONGUE ---")]
    [SerializeField] protected Transform _tongueStartTransform;
    [SerializeField] protected Transform _tongueEndTransform;
    [SerializeField] protected float _tongueMaxLenght = 5f;
    [Tooltip("Raycast and detect layer mask")]
    [SerializeField] protected LayerMask _tongueLayerMask;
    [SerializeField] protected TongueGrabType _tongueGrabType;
    protected enum TongueGrabType
    {
        Tirer,
        Attirer
    }

    [Space]
    [SerializeField] protected float _tongueOutTime = 0.15f;
    [SerializeField] protected AnimationCurve _tongueOutCurve;
    [SerializeField] protected float _tongueInTime = 0.15f;
    [SerializeField] protected AnimationCurve _tongueInCurve;
    float _tongueOutDelay = 0, _tongueInDelay = 0;

    [Space]
    [SerializeField] protected LineRenderer _tongueLineRenderer;
    [SerializeField] protected GameObject _tongueDirectionObject;
    [Space]
    [SerializeField] protected float _tongueHitRadius = 0.3f;
    [SerializeField] protected float _tongueHitForce = 10f;
    [Tooltip("Cast sphere around the hit point and detect layer mask")]
    [SerializeField] protected LayerMask _tongueHitLayerMask;

    //bool _tongueAnimEnded = true, _tongueIn = false, _tongueOut = false;

    [Header("--- MOUNT OTHER ---")]
    public Transform onFrogTransform;
    [SerializeField] protected float _mountRadius = 3f;
    [SerializeField] protected LayerMask _playerLayer;
    protected Transform _otherPlayerMountTransform = null;
    public Transform GetMountTransform { get { return _otherPlayerMountTransform; } }

    [Header("--- PREDICTED JUMP ---")]
    [SerializeField] protected LineRenderer _jumpPredictionLine;
    [SerializeField] protected bool _showTrajectoryLine = true;
    [SerializeField] protected GameObject _landingPointObject;
    [SerializeField] protected bool _showLandingPoint = true;
    [Space]
    [SerializeField] protected float _jumpInteructedIfSitckLessThan = 0.05f;
    [SerializeField] protected float _timeToChargeJump = 0.5f;
    [SerializeField] protected float _landingPointSmoothSpeed = 0.15f;
    [Space]
    [SerializeField] protected float _interruptLandingPointTime = 0.2f;
    [SerializeField] protected AnimationCurve _interruptLandingPointMovementCurve;
    [Space]
    [SerializeField] protected int _jumpPredictionLinePointCount = 200;
    [SerializeField] protected float _jumpPredictiontDuration = 5;
    [SerializeField] protected LayerMask _jumpPredictionLayerMask;

    MeshRenderer _jumpPredictionObjectRenderer;
    public bool IsJumping { get { return _isJumping; } }
    protected bool _isJumping = false;
    protected bool _jumpCharged = false;
    protected bool _wasGroundedLastFrame = false;
    protected float _interruptLandingPointTimer;
    protected float _jumpMaxLenghtTimer;
    protected float _currentJumpForceForward;
    protected float _currentJumpForceUp;
    protected Vector3 _predictionLandingPoint;
    protected Vector3 _landingPointLastPosition;
    protected Vector3 _velocityRef = Vector3.zero;

    [Header("--- INPUTS ---")]
    [SerializeField] bool _showInputDebug = false;
    [ShowIf("_showInputDebug", true)] public bool MoveInput;
    [ShowIf("_showInputDebug", true)] public Vector2 RotaInput = Vector2.zero;
    [ShowIf("_showInputDebug", true)] public bool JumpPressInput = false;
    [ShowIf("_showInputDebug", true)] public bool JumpReleaseInput = false;
    [ShowIf("_showInputDebug", true)] bool _startTongueAimInput = false;
    public bool StartTongueAimInput { set { _startTongueAimInput = value; } }
    [ShowIf("_showInputDebug", true)] bool _endTongueAimInput = false;
    public bool EndTongueAimInput { get { return _endTongueAimInput; } set { _endTongueAimInput = value; } }
    [ShowIf("_showInputDebug", true)] public bool MountInput;

    protected StateMachinePlayer _smPlayer;
    protected bool _isOnFrog = false;
    protected bool _hasPushedOther = false;

    // =====================================================================================
    //                                   UNITY METHODS 
    // =====================================================================================
    protected override void Start()
    {
        base.Start();

        JumpPressInput = false;
        JumpReleaseInput = false;

        _jumpPredictionObjectRenderer = _landingPointObject.GetComponent<MeshRenderer>();

        // Set jump prediction color
        SetJumpPredictionColor(playerColor);

        _smPlayer = new StateMachinePlayer(this);
        _smPlayer.Start();
    }

    protected override void Update()
    {
        base.Update();

        _smPlayer.Update(Time.deltaTime);

        if (EndTongueAimInput)
        {            
            EndTongueAimInput = false;
            UseTongue();
        }

        ManageJump();
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _smPlayer.FixedUpdate(Time.fixedDeltaTime);

        if (MoveInput && RotaInput != Vector2.zero)
           Move();     
    }

    // =====================================================================================
    //                                   MOVEMENT METHODS 
    // =====================================================================================

    public void Rotate()
    {
        Rotate(RotaInput.x, RotaInput.y);
    }

    public override void Jump()
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

            _interruptLandingPointTimer = 0; // Reset timer of landing point lerp to player position
        }
        // Lerp landing point to player position with time and disable it
        else
        {
            // Set landing point target point for lerp
            Vector3 landingPointTargetPosition = new Vector3(transform.position.x, _landingPointLastPosition.y, transform.position.z);

            if (_interruptLandingPointTimer < _interruptLandingPointTime) // Timer and lerp of landing point to player position
            {
                _landingPointObject.transform.position = Vector3.Lerp(_landingPointLastPosition, landingPointTargetPosition, _interruptLandingPointMovementCurve.Evaluate(_interruptLandingPointTimer / _interruptLandingPointTime));
                _interruptLandingPointTimer += Time.deltaTime;
            }
            else // Set landing point to player position and disable it
            {
                _landingPointObject.transform.position = landingPointTargetPosition;
                SetPredictionRenderer(false);
            }
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
        if (jumpCharged)
        {
            _currentJumpForceForward = Mathf.Lerp(_jumpForceFwd, _longJumpForceFwd, RotaInput.magnitude);
            _currentJumpForceUp = Mathf.Lerp(_jumpForceUp, _longJumpForceUp, RotaInput.magnitude);
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
    public virtual Vector3 TongueAimPosition()
    {
        if (Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
        {
            if(hit.transform.TryGetComponent<PlayerEntity>(out  PlayerEntity otherPlayer) && !_hasPushedOther)
            {
                otherPlayer.PushPlayer(transform.forward, _tongueHitForce);
                _hasPushedOther = true;
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
        _hasPushedOther = false;
    }
    protected void TongueLine()
    {
        // Set tongueLineRenderer point position
        _tongueLineRenderer.SetPosition(0, _tongueStartTransform.position);
        _tongueLineRenderer.SetPosition(1, _tongueEndTransform.position);
    }
    protected virtual void TongueHitObject(Vector3 target)
    {

    }
    public void UseTongue()
    {
        StartCoroutine(UseTongueCoroutine());
    }

    // =====================================================================================
    //                                   MOUNT METHODS 
    // =====================================================================================
    public virtual bool TryMount()
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

    public virtual void StopMount()
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

    public virtual void PushPlayer(Vector3 dir, float force)
    {
        _rigidbodyController.AddForce(dir, force, ForceMode.Impulse);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_tongueStartTransform.position, (this.transform.forward * _tongueMaxLenght) + _tongueStartTransform.position);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _mountRadius);
    }
}

