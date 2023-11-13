using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleRigidbody : MonoBehaviour
{
    [Header("--- GRAVITY ---")]
    [SerializeField] bool _useGravity = true;
    [SerializeField] float _normalGravity = 9.81f;
    [SerializeField] float _fallingGravity = 30f;
    float _actualGravity = 0f;

    [Header("--- VELOCITY CLAMP ---")]
    [SerializeField] bool _useVelocityClampX = true;
    [SerializeField] Vector2 _clampVelocityX = new Vector2(-100, 100);
    [SerializeField] bool _useVelocityClampY = true;
    [SerializeField] Vector2 _clampVelocityY = new Vector2(-100, 100);
    [SerializeField] bool _useVelocityClampZ = true;
    [SerializeField] Vector2 _clampVelocityZ = new Vector2(-100, 100);

    bool _rbInitialized = false;
    Rigidbody _rb;
    public bool UseGRavity { set { _useGravity = value; } }
    public bool  NormalGravity { set { _actualGravity = value == true ? _normalGravity : _fallingGravity; } }
    public bool FallingGravity { set { _actualGravity = value == true ? _fallingGravity : _normalGravity; } }
    public bool IsFalling { get { return _rb.velocity.y < 0; } }
    public Vector3 Velocity { get { return _rb.velocity; } }

    private void Awake()
    {
        InitRigidbody();
    }

    private void Start()
    {
        InitRigidbody();
    }

    private void Update()
    {
        if (this.IsFalling)
            this.FallingGravity = true;
        else
            this.NormalGravity = true;
    }

    private void FixedUpdate()
    {
        if (_useGravity)
            ApplyGravity();

        ClampVelocity();
    }

    protected void InitRigidbody()
    {
        if( !_rbInitialized)
        {
            _rb = this.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) ? rb : this.transform.AddComponent<Rigidbody>();
            _rb.useGravity = false;
            _rbInitialized = true;

            _actualGravity = _normalGravity;

            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // Apply the gravity on the Rigidbody based on mass
    public void ApplyGravity()
    {
        _rb.AddForce(-Vector3.up * _rb.mass * _actualGravity);
    }

    // AddForce on Rigidbody
    public void AddForce(Vector3 direction, float force, ForceMode mode)
    {
        _rb.AddForce(direction * force, mode);
    }

    // AddForce on Rigidbody based on mass
    public void AddRelativeForce(Vector3 direction, float force, ForceMode mode)
    {
        _rb.AddRelativeForce(direction * force, mode);
    }

    public void ClampVelocity()
    {
        Vector3 velocity = _rb.velocity;

        velocity.x = _useVelocityClampX ? Mathf.Clamp(velocity.x, _clampVelocityX.x, _clampVelocityX.y) : velocity.x;
        velocity.y = _useVelocityClampY ? Mathf.Clamp(velocity.y, _clampVelocityY.x, _clampVelocityY.y) : velocity.y;
        velocity.z = _useVelocityClampZ ? Mathf.Clamp(velocity.z, _clampVelocityZ.x, _clampVelocityZ.y) : velocity.z;

        _rb.velocity = velocity;
    }
}
