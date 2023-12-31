using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;


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

    [Header("--- Jump prediction ---")]
    [SerializeField] protected GameObject _jumpPredictionObject;
    [SerializeField] protected LineRenderer _jumpPredictionLine;
    [SerializeField] protected float _jumpPredictionLinePointCount;
    [SerializeField] protected float _jumpPredictiontTimeBetweenPoints;
    [SerializeField] protected LayerMask _jumpPredictionLayerMask;

    // INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS INPUTS

    // ===== MOVE INPUT =====
    public bool MoveInput;

    // ===== ROTA INPUT =====
    public Vector2 RotaInput = Vector2.zero;

    // ===== JUMP INPUT =====
    public bool JumpInput = false;
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

        if(RotaInput != Vector2.zero)
        {
            ShowJumpPrediction();
        }
        else
        {
            _jumpPredictionLine.enabled = false;
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
        if (LongJumpInput)
        {
            Vector3 jumpForward = transform.forward * _longJumpForceFwd;
            Vector3 jumpUp = transform.up * _longJumpForceUp;
            Vector3 jumpVector = jumpForward + jumpUp;

            _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);
        }
        else
        {
            Vector3 jumpForward = transform.forward * _jumpForceFwd;
            Vector3 jumpUp = transform.up * _jumpForceUp;
            Vector3 jumpVector = jumpForward + jumpUp;

            _rigidbodyController.AddForce(jumpVector.normalized, jumpVector.magnitude, _jumpMode);
        }

        LongJumpInput = false;
        JumpInput = false;
    }

    public void Rotate()
    {
        Rotate(RotaInput.x, RotaInput.y);
    }

    void ShowJumpPrediction()
    {
        _jumpPredictionLine.enabled = true;
        _jumpPredictionLine.positionCount = Mathf.CeilToInt(_jumpPredictionLinePointCount / _jumpPredictiontTimeBetweenPoints) + 1;
        Vector3 startPosition = transform.position;
        Vector3 jumpVector = (transform.forward * _jumpForceFwd) + (transform.up * _jumpForceUp);
        Vector3 startVelocity = jumpVector / _rigidbodyController.Mass;
        int i = 0;
        _jumpPredictionLine.SetPosition(i, startPosition);

        Vector3 lastPosition = startPosition;
        for(float time = 0; time < _jumpPredictionLinePointCount; time += _jumpPredictiontTimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            if(lastPosition.y > point.y)
            {
                point.y = startPosition.y + startVelocity.y * time - (_rigidbodyController.FallingGravity / 2 * time * time);
            }
            else
            {
                point.y = startPosition.y + startVelocity.y * time - (_rigidbodyController.NormalGravity / 2 * time * time);
            }

            lastPosition = point;
            _jumpPredictionLine.SetPosition(i, point);

            Vector3 lastPoint = _jumpPredictionLine.GetPosition(i - 1);
            
            if(Physics.Raycast(lastPoint, (point - lastPoint).normalized, out RaycastHit hitInfo, (point - lastPoint).magnitude, _jumpPredictionLayerMask))
            {
                _jumpPredictionLine.SetPosition(i, hitInfo.point);
                _jumpPredictionLine.positionCount = i + 1;
                return;
            }
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

