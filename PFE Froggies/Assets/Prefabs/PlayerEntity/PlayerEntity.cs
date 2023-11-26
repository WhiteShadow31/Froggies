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

    bool _tongueAnimEnded = true, _tongueIn = false, _tongueOut = false;
   




    [Header("--- MOUNT OTHER ---")]
    public Transform onFrogTransform;
    [SerializeField] protected float _mountRadius = 3f;
    [SerializeField] protected LayerMask _playerLayer;
    protected Transform _otherPlayerMountTransform = null;
    public Transform GetMountTransform { get { return _otherPlayerMountTransform; } }

    [Header("--- UI ---")]
    public GameObject WherePlayerBubble;


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
    float _horizontalInput = 0;
    float _verticalInput = 0;
    public bool StartTongueAimInput { set { _startTongueAimInput = value; } }
    public bool EndTongueAimInput { get { return _endTongueAimInput; } set { _endTongueAimInput = value; } }

    // ===== MOUNT INPUT =====
    public bool MountInput;

    protected StateMachinePlayer _smPlayer;
    [HideInInspector] public bool isOnFrog = false;


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

    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _smPlayer.FixedUpdate(Time.fixedDeltaTime);


        if (MoveInput && RotaInput != Vector2.zero)
           Move();     
    }

    // 
    // =================================== MOVEMENT METHODS ===================================
    //

    public override void Jump()
    {
        _rigidbodyController.StopYVelocity();
        if (LongJumpInput)
        {
            _rigidbodyController.AddForce(this.transform.up, _longJumpForceUp, _jumpMode);
            _rigidbodyController.AddForce(this.transform.forward, _longJumpForceFwd, _jumpMode);
        }
        else
        {
            _rigidbodyController.AddForce(this.transform.up, _jumpForceUp, _jumpMode);
            _rigidbodyController.AddForce(this.transform.forward, _jumpForceFwd, _jumpMode);
        }

        LongJumpInput = false;
        JumpInput = false;

    }

    public void Rotate()
    {
        Rotate(RotaInput.x, RotaInput.y);
    }

    // 
    // =================================== TONGUE METHODS ===================================
    //

    protected virtual void ChoseTongueState()
    {
        if (_startTongueAimInput && _tongueAnimEnded) // Aiming with tongue
        {
            ShowTongueAim(TongueAimPosition());
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

    public virtual void ShowTongueAim(Vector3 pos)
    {
        _tongueDirectionObject.SetActive(true);
        _tongueDirectionObject.transform.position = Vector3.Lerp(_tongueStartTransform.position, pos, 0.5f);
    }

    // SHOW VISUAL AIM FOR THE TONGUE
    public virtual Vector3 TongueAimPosition()
    {
        // Place tongueDirectionObject on the middle of the tongueStart and the raycast hit point (or tongueMaxLenght if raycast not hit)
        if(Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
        {
            return hit.point;
            //_tongueDirectionObject.transform.position = Vector3.Lerp(_tongueStartTransform.position, hit.point, 0.5f);           
        }
        else
        {
            return _tongueStartTransform.position + (transform.forward * _tongueMaxLenght);
            //_tongueDirectionObject.transform.position = Vector3.Lerp(_tongueStartTransform.position, _tongueStartTransform.position + (transform.forward * _tongueMaxLenght), 0.5f);
        }
    }

    protected virtual void TongueHit()
    {        
        //if (_tongueDirectionObject.activeInHierarchy) // If TongueDirectionObject is active, deactivate it
            _tongueDirectionObject.SetActive(false);
        
        

        //if (!_tongueLineRenderer.enabled) // If tongueLineRenderer is not disable, enable it
            _tongueLineRenderer.enabled = true;
        
        

        // Set tongueLineRenderer point position
        _tongueLineRenderer.SetPosition(0, _tongueStartTransform.position);
        _tongueLineRenderer.SetPosition(1, _tongueEndTransform.position);

        // Get Raycast hit point
        Vector3 hitPoint;
        if(Physics.Raycast(_tongueStartTransform.position, transform.forward, out RaycastHit hit, _tongueMaxLenght, _tongueLayerMask))
            hitPoint = hit.point;
        else
            hitPoint = _tongueStartTransform.position + (transform.forward * _tongueMaxLenght);
        
        
        
        

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
    
    protected virtual void UseTongue()
    {
        //if (_tongueDirectionObject.activeInHierarchy) // If TongueDirectionObject is active, deactivate it
        _tongueDirectionObject.SetActive(false);

        //if (!_tongueLineRenderer.enabled) // If tongueLineRenderer is not disable, enable it
        _tongueLineRenderer.enabled = true;
    }

    // 
    // =================================== MOUNT METHODS ===================================
    //

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

