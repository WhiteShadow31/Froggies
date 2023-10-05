using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/* Entity Controller for frog entity used by players */
[RequireComponent(typeof(Rigidbody))]
public class FrogEntityController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] Transform _groundCheckTransform;
    [SerializeField] float _groundCheckRadius = 0.08f;
    [SerializeField] LayerMask _groundCheckLayerMask;
    TriggerCollider _groundCheckCollider;
    bool _grounded = false;

    [Header("Jump Parameters")]
    [Header("UP - Axis Y")]
    [SerializeField] float _jumpForceY = 1f;

    [Header("FORWARD - Axis X")]
    [SerializeField] float _jumpForceX = 1f;

    [Header("Gravity Parameters")]
    [SerializeField] float _gravityGround = 9.81f;
    [SerializeField] float _gravityGoingUp = 9.81f;
    [SerializeField] float _gravityGoingDown = 30f;
    float _actualGravity;


    Rigidbody _rb;

    private void Awake()
    {
        InitRigidbody();
    }

    private void Start()
    {
        InitRigidbody();
        InitGroundCheck();
        InitGravity();
    }

    private void Update()
    {
        _grounded = GroundCheck();
    }

    private void FixedUpdate()
    {
        ApplyGravity();    
    }

    //  ============ RIGIDBODY INIT ============
    void InitRigidbody()
    {
        _rb = TryGetComponent<Rigidbody>(out Rigidbody rb) ? rb : this.transform.AddComponent<Rigidbody>();
        _rb.useGravity = false;

        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    //  ============ GRAVITY FUNCTIONS ============
    void InitGravity()
    {
        ModifyGravity(_gravityGround);
    }
    void ModifyGravity(float grav)
    {
        _actualGravity = grav;
    }
    void ApplyGravity()
    {
        _rb.AddForce(-Vector3.up * _rb.mass * _actualGravity);
    }
    void ChooseGravity()
    {
        // Falling
        if(_rb.velocity.y < -0.1f)
        {
            ModifyGravity(_gravityGoingDown);
        }
        // Going Up
        else if( _rb.velocity.y > 0.1f)
        {
            ModifyGravity(_gravityGoingUp);
        }
        // On ground
        else
        {
            ModifyGravity(_gravityGround);
        }
    }

    //  ============ GROUND CHECK ============
    bool GroundCheck()
    {
        return _groundCheckCollider.HasObjectsDetected;
    }

    void InitGroundCheck()
    {
        if(_groundCheckTransform == null)
        {
            Transform tr = new GameObject("GroundCheck").transform;
            tr.parent = this.transform;
            tr.position = Vector3.zero;
            tr.rotation = Quaternion.identity;
        }
                
        _groundCheckCollider = _groundCheckTransform.TryGetComponent<TriggerCollider>(out TriggerCollider col) ? col : _groundCheckTransform.AddComponent<TriggerCollider>();
        _groundCheckCollider.InitTriggerCollider(_groundCheckRadius, _groundCheckLayerMask);
    }
}
