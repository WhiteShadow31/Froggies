using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEntity : LivingEntity
{
    [Header("--- TONGUE ---")]
    [SerializeField] protected Transform _tongueStartTransform;
    [SerializeField] protected Transform _tongueEndTransform;
    [SerializeField] protected float _tongueMaxLenght = 5f;
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
    [SerializeField] protected LayerMask _tongueHitLayerMask;

    bool _tongueAnimEnded = true, _tongueIn = false, _tongueOut = false;

    [Header("--- CHARGING JUMP ---")]
    [SerializeField] protected float _fullChargeJumpTime = 1.5f;
    [SerializeField] protected AnimationCurve _chargingJumpCurve;
    [SerializeField] protected float _minJumpForceUp = 3f;
    [SerializeField] protected float _maxJumpForceUp = 5f;
    [SerializeField] protected float _minJumpForceForward = 5f;
    [SerializeField] protected float _maxJumpForceForward = 10f;

    bool _chargingJump = false;
    float _chargingJumpTimer = 0;
    float _currentJumpForceUp = 0, _currentJumpForceForward = 0;

    [Header("--- MOUNT OTHER ---")]
    public Transform onFrogTransform;
    [SerializeField] protected float _mountRadius = 3f;
    [SerializeField] protected LayerMask _playerLayer;
    protected Transform _otherPlayerMountTransform = null;
    public Transform GetMountTransform { get { return _otherPlayerMountTransform; } }



    [HideInInspector] public bool isOnFrog = false;

    bool _moveInput = false;
    bool _startJumpInput = false;
    bool _endJumpInput = false;
    bool _startTongueAimInput = false;
    bool _endTongueAimInput = false;
    float _horizontalInput = 0;
    float _verticalInput = 0;
    Vector2 _rotaInput = Vector2.zero;
    public Vector2 RotaInput { set { _rotaInput = value; } }
    public bool StartJumpInput {  set { _startJumpInput = value; } }
    public bool EndJumpInput { set { _endJumpInput = value; } }
    public bool MoveInput { set { _moveInput = value; } }
    public bool StartTongueAimInput { set { _startTongueAimInput = value; } }
    public bool EndTongueAimInput { set { _endTongueAimInput = value; } }
    public bool MountInput;

    protected StateMachinePlayer _smPlayer;

    protected override void Start()
    {
        base.Start();
        _smPlayer = new StateMachinePlayer(this);
        _smPlayer.Start();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _smPlayer.Update(Time.deltaTime);

        ChoseTongueState();

        ChargingJump();

        /*
        if (_mountInput)
        {
            TryMount();
            _mountInput = false;
        }
        */

    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _smPlayer.FixedUpdate(Time.fixedDeltaTime);


        if (_moveInput)
            Move();     
    }

    public override void Jump()
    {        
        if(_startJumpInput)
        {
            _chargingJumpTimer = 0;
            _currentJumpForceForward = _minJumpForceForward;
            _currentJumpForceUp = _minJumpForceUp;

            _chargingJump = true;
            _startJumpInput = false;
        }
        //base.Jump();
    }

    void ChargingJump()
    {
        if (_chargingJump)
        {
            // Charging jump forces up and forward
            if(_chargingJumpTimer < _fullChargeJumpTime)
            {
                _currentJumpForceForward = Mathf.Lerp(_minJumpForceForward, _maxJumpForceForward, _chargingJumpCurve.Evaluate(_chargingJumpTimer / _fullChargeJumpTime));
                _currentJumpForceUp = Mathf.Lerp(_minJumpForceUp, _maxJumpForceUp, _chargingJumpCurve.Evaluate(_chargingJumpTimer / _fullChargeJumpTime));

                _chargingJumpTimer += Time.deltaTime;
            }
            else
            {
                _currentJumpForceForward = _maxJumpForceForward;
                _currentJumpForceUp = _maxJumpForceUp;
            }

            // When jump button release, jump
            if (_endJumpInput)
            {
                _rigidbodyController.AddForce(this.transform.up, _currentJumpForceUp, _jumpMode);
                _rigidbodyController.AddForce(this.transform.forward, _currentJumpForceForward, _jumpMode);

                _endJumpInput = false;
                _chargingJump = false;
            }

        }
        else if (_endJumpInput)
        {
            _endJumpInput = false;
        }
    }

    public void Rotate()
    {
        Rotate(_rotaInput.x, _rotaInput.y);
    }

    protected virtual void ChoseTongueState()
    {
        if (_startTongueAimInput && _tongueAnimEnded) // Aiming with tongue
        {
            TongueAim();
        }
        else
        {
            _tongueDirectionObject.SetActive(false);
        }

        if (_endTongueAimInput) // Tongue hit when release button
        {
            if (_startTongueAimInput && _tongueAnimEnded)
            {
                _tongueOut = true;
                _tongueOutDelay = 0;
                _tongueInDelay = 0;

                _tongueAnimEnded = false;
            }

            _startTongueAimInput = false;

            TongueHit();
        }
    }

    protected virtual void TongueAim()
    {
        // If TongueDirectionObject is not active, activate it
        if (!_tongueDirectionObject.activeInHierarchy)
        {
            _tongueDirectionObject.SetActive(true);
        }

        // Place tongueDirectionObject on the middle of the tongueStart and the raycast hit point (or tongueMaxLenght if raycast not hit)
        if(Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
        {
            _tongueDirectionObject.transform.position = Vector3.Lerp(_tongueStartTransform.position, hit.point, 0.5f);           
        }
        else
        {
            _tongueDirectionObject.transform.position = Vector3.Lerp(_tongueStartTransform.position, _tongueStartTransform.position + (transform.forward * _tongueMaxLenght), 0.5f);
        }
    }

    protected virtual void TongueHit()
    {        
        if (_tongueDirectionObject.activeInHierarchy) // If TongueDirectionObject is active, deactivate it
        {
            _tongueDirectionObject.SetActive(false);
        }

        if (!_tongueLineRenderer.enabled) // If tongueLineRenderer is not disable, enable it
        {
            _tongueLineRenderer.enabled = true;
        }

        // Set tongueLineRenderer point position
        _tongueLineRenderer.SetPosition(0, _tongueStartTransform.position);
        _tongueLineRenderer.SetPosition(1, _tongueEndTransform.position);

        // Get Raycast hit point
        Vector3 hitPoint;
        if(Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = _tongueStartTransform.position + (transform.forward * _tongueMaxLenght);
        }

        // Tongue anim in and out
        if (!_tongueAnimEnded)
        {
            if (_tongueOut)
            {
                if(_tongueOutDelay < _tongueOutTime)
                {
                    _tongueEndTransform.position = Vector3.Lerp(_tongueStartTransform.position, hitPoint, _tongueOutCurve.Evaluate(_tongueOutDelay / _tongueOutTime));

                    _tongueOutDelay += Time.deltaTime;
                }
                else
                {
                    _tongueEndTransform.position = hitPoint;
                    _tongueIn = true;
                    _tongueOut = false;

                    TongueHitObject(hitPoint);
                }
            }
            else if (_tongueIn)
            {
                if (_tongueInDelay < _tongueInTime)
                {
                    _tongueEndTransform.position = Vector3.Lerp(hitPoint, _tongueStartTransform.position, _tongueOutCurve.Evaluate(_tongueInDelay / _tongueInTime));

                    _tongueInDelay += Time.deltaTime;
                }
                else
                {
                    _tongueEndTransform.position = _tongueStartTransform.position;
                    _tongueLineRenderer.enabled = false;
                    _tongueIn = false;
                    _tongueAnimEnded = true;
                    _endTongueAimInput = false;
                }
            }
        }
    }

    protected virtual void TongueHitObject(Vector3 target)
    {

    }

    public virtual bool TryMount()
    {
        if (!isOnFrog)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _mountRadius, _playerLayer);

            foreach (Collider col in colliders)
            {
                // Get a player
                if (col.TryGetComponent<PlayerEntity>(out PlayerEntity otherPlayerEntity) && otherPlayerEntity != this)
                {
                    _otherPlayerMountTransform = otherPlayerEntity.onFrogTransform;
                    isOnFrog = true;
                    return true;
                }
            }
        }
        MountInput = false;
        return false;
    }

    public virtual void StopMount()
    {
        if (isOnFrog)
        {
            _otherPlayerMountTransform = null;
            isOnFrog = false;
        }
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