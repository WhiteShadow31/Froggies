using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PlayerEntity))]
public class SimpleRigidbody : MonoBehaviour
{
    [Header("--- GRAVITY ---")]
    [SerializeField] bool _useGravity = true;
    [SerializeField] float _baseGravity = 9.81f;
    [SerializeField] float _jumpNormalGravity = 9.81f;
    [SerializeField] float _jumpFallingGravity = 30f;
    float _actualGravity = 0f;

    [Header("--- VELOCITY CLAMP ---")]
    [SerializeField] bool _useClampPreciseMove = true;
    [SerializeField] float _clampPreciseMove = 10;
    [SerializeField] bool _useVelocityClampY = true;
    [SerializeField] float _clampVelocityY = 10;
    //[SerializeField] bool _useVelocityClampZ = true;
    //[SerializeField] float _clampVelocityZ = 10;

    bool _rbInitialized = false;
    Rigidbody _rb;
    public bool UseGRavity { set { _useGravity = value; } }
    public bool  IsNormalGravity {set { _actualGravity = value == true ? _jumpNormalGravity : _jumpFallingGravity; } }
    public float NormalGravity { get { return _jumpNormalGravity; } }
    public bool IsFallingGravity { set { _actualGravity = value == true ? _jumpFallingGravity : _jumpNormalGravity; } }
    public float FallingGravity { get { return _jumpFallingGravity; } }
    public bool IsFalling { get { return _rb.velocity.y < 0; } }
    public Vector3 Velocity { get { return _rb.velocity; } }
    public float Mass { get { return _rb.mass; } }

    PlayerEntity _playerEntity;

    private void Awake()
    {
        _playerEntity = GetComponent<PlayerEntity>();   

        InitRigidbody();
    }

    private void Start()
    {
        InitRigidbody();
    }

    private void Update()
    {
        if (this.IsFalling)
            this.IsFallingGravity = true;
        else
            this.IsNormalGravity = true;
    }

    private void FixedUpdate()
    {
        if (_useGravity)
        {
            if (!_playerEntity.IsGrounded)
            {
                ApplyGravity(_actualGravity);
            }
            else
            {
                ApplyGravity(_baseGravity);
            }
        }
    }

    protected void InitRigidbody()
    {
        if( !_rbInitialized)
        {
            _rb = this.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) ? rb : this.transform.AddComponent<Rigidbody>();
            _rb.useGravity = false;
            _rbInitialized = true;

            _actualGravity = _jumpNormalGravity;

            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // Apply the gravity on the Rigidbody based on mass
    public void ApplyGravity(float gravity)
    {
        if (_useVelocityClampY)
            _rb.AddForce(-Vector3.up * _rb.mass * gravity * (_clampVelocityY - _rb.velocity.magnitude) * Time.deltaTime);
        else
            _rb.AddForce(-Vector3.up * _rb.mass * gravity);

        //ClampVelocity();

    }

    // AddForce on Rigidbody
    public void AddPreciseForce(Vector3 direction, float force, ForceMode mode)
    {
        if (_useClampPreciseMove)
            _rb.AddForce(direction * force * (_clampPreciseMove - _rb.velocity.magnitude) * Time.deltaTime, mode);
        else if(_rb.velocity.magnitude + force <= _clampPreciseMove)
            _rb.AddForce(direction * force, mode);
    }

    public void AddForce(Vector3 direction, float force, ForceMode mode)
    {
        _rb.AddForce(direction * force, mode);
    }

    // AddForce on Rigidbody based on mass
    public void AddRelativeForce(Vector3 direction, float force, ForceMode mode)
    {
        if (_useVelocityClampY)
            _rb.AddRelativeForce(direction * force * (_clampPreciseMove - _rb.velocity.magnitude) * Time.deltaTime, mode);
        else
            _rb.AddRelativeForce(direction * force, mode);
    }

    public void StopYVelocity()
    {
        Vector3 vel = _rb.velocity;
        vel.y = 0f;
        _rb.velocity = vel;
    }

    public void StopVelocity()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
