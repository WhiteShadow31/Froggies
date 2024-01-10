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

    [Header("Jump experimental")]
    [SerializeField] protected bool _useExperimentalJump = false;
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected bool _showPredictionLine = false;
    [Space]
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected float _timeToReachMaxJumpLenght = 1.5f;
    [SerializeField, ShowIf("_useExperimentalJump", true)] AnimationCurve _jumpLenghtCurve;
    [Space]
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected GameObject _jumpPredictionObject;
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected LineRenderer _jumpPredictionLine;
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected int _jumpPredictionLinePointCount;
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected float _jumpPredictiontDuration;
    [SerializeField, ShowIf("_useExperimentalJump", true)] protected LayerMask _jumpPredictionLayerMask;

    MeshRenderer _jumpPredictionObjectRenderer;
    protected bool _prepareJump;
    protected bool _atMaxJumpLenght;
    protected float _jumpMaxLenghtTimer;
    protected float _currentJumpForceForward;
    protected float _currentJumpForceUp;
    protected Vector3 _predictionLandingPoint;

    // INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS

    // ===== MOVE INPUT =====
    public bool MoveInput;

    // ===== ROTA INPUT =====
    public Vector2 RotaInput = Vector2.zero;

    // ===== JUMP INPUT =====
    public bool JumpInput = false;
    public bool JumpReleaseInput = false;
    public bool LongJumpInput = false;

    // ===== TONGUE INPUT =====
    bool _startTongueAimInput = false;
    bool _endTongueAimInput = false;
    //float _horizontalInput = 0;
    //float _verticalInput = 0;
    public bool StartTongueAimInput { set { _startTongueAimInput = value; } }
    public bool EndTongueAimInput { get { return _endTongueAimInput; } set { _endTongueAimInput = value; } }

    // ===== MOUNT INPUT =====
    public bool MountInput;

    protected StateMachinePlayer _smPlayer;
    [HideInInspector] public bool isOnFrog = false;

    // =====================================================================================
    //                                   UNITY METHODS 
    // =====================================================================================
    protected override void Start()
    {
        base.Start();

        _jumpPredictionObjectRenderer = _jumpPredictionObject.GetComponent<MeshRenderer>();

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

        if (_useExperimentalJump)
        {
            if(IsGrounded && JumpInput)
            {
                _jumpPredictionObjectRenderer.enabled = true;
                _jumpPredictionLine.enabled = true;
                PrepareJump();              
                ShowJumpPrediction();
            }
            else
            {
                _currentJumpForceForward = _jumpForceFwd;
                _currentJumpForceUp = _jumpForceUp;

                _jumpPredictionObjectRenderer.enabled = false;
                _jumpPredictionLine.enabled = false;
            }
        }
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

    public override void Jump()
    {
        _rigidbodyController.StopVelocity();

        if (_useExperimentalJump)
        {
            if(JumpReleaseInput)
            {
                Vector3 jumpVector = (transform.forward * _currentJumpForceForward) + (transform.up * _currentJumpForceUp);
                _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);

                _currentJumpForceForward = _jumpForceFwd;
                _currentJumpForceUp = _jumpForceUp;

                _prepareJump = false;
                _atMaxJumpLenght = false;
                _jumpMaxLenghtTimer = 0;

                LongJumpInput = false;
                JumpInput = false;
                JumpReleaseInput = false;
            }
        }
        else
        {
            if (LongJumpInput)
            {
                Vector3 jumpVector = (transform.forward * _longJumpForceFwd) + (transform.up * _longJumpForceUp);
                _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);
            }
            else
            {
                Vector3 jumpVector = (transform.forward * _jumpForceFwd) + (transform.up * _jumpForceUp);
                _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);
            }

            LongJumpInput = false;
            JumpInput = false;
            JumpReleaseInput = false;
        }   
    }

    public void Rotate()
    {
        Rotate(RotaInput.x, RotaInput.y);
    }

    void PrepareJump()
    {
        if (!_atMaxJumpLenght)
        {
            if(_jumpMaxLenghtTimer < _timeToReachMaxJumpLenght)
            {
                float maxForwardForce = Mathf.Lerp(_jumpForceFwd, _longJumpForceFwd, _jumpLenghtCurve.Evaluate(_jumpMaxLenghtTimer / _timeToReachMaxJumpLenght));
                _currentJumpForceForward = Mathf.Lerp(_jumpForceFwd, maxForwardForce, RotaInput.magnitude);
                float maxUpForce = Mathf.Lerp(_jumpForceUp, _longJumpForceUp, _jumpLenghtCurve.Evaluate(_jumpMaxLenghtTimer / _timeToReachMaxJumpLenght));
                _currentJumpForceUp = Mathf.Lerp(_jumpForceUp, maxUpForce, RotaInput.magnitude);

                _jumpMaxLenghtTimer += Time.deltaTime;
            }
            else
            {
                _currentJumpForceForward = Mathf.Lerp(_jumpForceFwd, _longJumpForceFwd, RotaInput.magnitude);
                _currentJumpForceUp = Mathf.Lerp(_jumpForceUp, _longJumpForceUp, RotaInput.magnitude);
            }
        }
        else
        {
            _currentJumpForceForward = Mathf.Lerp(_jumpForceFwd, _longJumpForceFwd, RotaInput.magnitude);
            _currentJumpForceUp = Mathf.Lerp(_jumpForceUp, _longJumpForceUp, RotaInput.magnitude);
        }
        
    }

    void ShowJumpPrediction()
    {
        if (_showPredictionLine)
        {
            _jumpPredictionLine.enabled = true;
        }
        else
        {
            _jumpPredictionLine.enabled = false;
        }

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

            if (!falling)
            {
                currentPoint = startPosition + startVelocity * timeOffset - (Vector3.up * -0.5f * -_rigidbodyController.NormalGravity * timeOffset * timeOffset);
            }
            else
            {
                currentPoint = startPosition + startVelocity * timeOffset - (Vector3.up * -0.5f * -_rigidbodyController.FallingGravity * timeOffset * timeOffset);
            }

            // If trajectory is falling
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

            _jumpPredictionLine.SetPosition(i, currentPoint);

            if (Physics.Linecast(lastPoint, currentPoint, out RaycastHit hitInfo, _jumpPredictionLayerMask))
            {
                _jumpPredictionLine.SetPosition(i, hitInfo.point);
                _jumpPredictionLine.positionCount = i + 1;
                _jumpPredictionObject.transform.position = hitInfo.point;
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
            return hit.point;
        }
        else
        {
            return _tongueStartTransform.position + (transform.forward * _tongueMaxLenght);
        }
    }
    IEnumerator UseTongue(Vector3 pos)
    {
        _tongueLineRenderer.enabled = true;
        while (_tongueOutDelay < _tongueOutTime)
        {
            _tongueEndTransform.position = Vector3.Lerp(_tongueStartTransform.position, pos, _tongueOutCurve.Evaluate(_tongueOutDelay / _tongueOutTime));

            TongueLine();

            _tongueOutDelay += Time.fixedDeltaTime;
            yield return null;
        }

        TongueHitObject(pos);

        while (_tongueInDelay < _tongueInTime)
        {
            _tongueEndTransform.position = Vector3.Lerp(pos, _tongueStartTransform.position, _tongueOutCurve.Evaluate(_tongueInDelay / _tongueInTime));

            TongueLine();

            _tongueInDelay += Time.fixedDeltaTime;
            yield return null;
        }
        _tongueLineRenderer.enabled = false;
        _tongueOutDelay = 0;
        _tongueInDelay = 0;
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
        Vector3 pos = TongueAimPosition();
        Debug.Log("Try tongue - pos : " + pos);
        StartCoroutine(UseTongue(pos));
    }

    // =====================================================================================
    //                                   MOUNT METHODS 
    // =====================================================================================
    public virtual bool TryMount()
    {
        if (!isOnFrog) // Is it not on a frog ?
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _mountRadius, _playerLayer); // Look for frogs

            foreach (Collider col in colliders)
            {
                // Get a player
                if (col.TryGetComponent<PlayerEntity>(out PlayerEntity otherPlayerEntity) && otherPlayerEntity != this) // Is it a frog 
                {

                    if (!otherPlayerEntity.isOnFrog)
                    {
                        _otherPlayerMountTransform = otherPlayerEntity.onFrogTransform;
                        Debug.Log(this.name + "mount on "+_otherPlayerMountTransform.parent.name);

                        // Set ignore collision between players to true
                        Physics.IgnoreCollision(GetComponent<Collider>(), _otherPlayerMountTransform.parent.GetComponent<Collider>(), true);

                        isOnFrog = true;
                        MountInput = false;
                        return true;
                    }
                }
            }
        }
        MountInput = false;

        //if()

        return false;
    }

    public virtual void StopMount()
    {
        if (isOnFrog)
        {
            // Set ignore collision between players to false
            Physics.IgnoreCollision(GetComponent<Collider>(), _otherPlayerMountTransform.parent.GetComponent<Collider>(), false);

            _otherPlayerMountTransform = null;

            isOnFrog = false;

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

