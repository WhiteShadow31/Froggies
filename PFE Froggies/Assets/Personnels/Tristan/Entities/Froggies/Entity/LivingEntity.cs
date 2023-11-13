using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody))]
public class LivingEntity : MonoBehaviour, ILivingEntity
{
    ILivingEntity entity;

    [Header("--- SIMPLE RIGIDBODY ---")]
    [SerializeField] protected SimpleRigidbody _rigidbodyController;

    [Header("--- GROUND CHECK ---")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundRadius;
    [SerializeField] protected LayerMask _groundMask;
    protected GroundedController _groundController;

    [Header("--- MOVEMENT ---")]
    [SerializeField] protected float _moveForce = 1;
    [SerializeField] protected ForceMode _moveMode = ForceMode.Impulse;

    [Header("--- JUMP ---")]
    [SerializeField] protected float _jumpForceUp = 1;
    [SerializeField] protected float _jumpForceFwd = 1;
    [SerializeField] protected ForceMode _jumpMode = ForceMode.Impulse;
    [SerializeField] protected int _nbrJumpMAX = 1;
    protected int _nbrJump = 0;

    [Header("--- ROTATION ---")]
    [SerializeField] protected Camera _camera;
    [SerializeField] float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    // Initialized
    protected bool _initialized = false;


    protected virtual void Awake()
    {
        InitComponents();
    }

    protected virtual void Start()
    {
        InitComponents();
        if(_camera == null) 
            _camera = Camera.main;
    }

    protected virtual void Update()
    {
        _nbrJump = _groundController.IsGrounded ? 0 : _nbrJump;
    }

    protected virtual void FixedUpdate()
    {

    }

    protected void InitComponents()
    {
        if (!_initialized)
        {
            InitSimpleRigidbody();
            InitGroundController();
            _initialized = true;
        }
    }
    protected void InitSimpleRigidbody()
    {
        _rigidbodyController = this.transform.TryGetComponent<SimpleRigidbody>(out SimpleRigidbody rb) ? rb : this.transform.AddComponent<SimpleRigidbody>();
    }
    protected void InitGroundController()
    {
        if(_groundCheck == null)
        {
            GameObject go = new GameObject("GroundCheck");
            go.transform.parent = this.transform;
            go.transform.position = Vector3.zero;
            _groundCheck = go.transform;
        }
        _groundController = new GroundedController(_groundCheck, _groundRadius, _groundMask);
    }

    public virtual void Rotate(float horizontal, float vertical)
    {
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public virtual void Move()
    {
        _rigidbodyController.AddForce(this.transform.forward, _moveForce, _moveMode);
    }
    public virtual void Jump()
    {
        if (_groundController.IsGrounded && _nbrJump < _nbrJumpMAX && Mathf.Abs(_rigidbodyController.Velocity.y) < 0.1f)
        {
            _nbrJump++;
            
            _rigidbodyController.AddForce(this.transform.up, _jumpForceUp, _jumpMode);
            _rigidbodyController.AddForce(this.transform.forward, _jumpForceFwd, _jumpMode);
        }
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_groundCheck.position, _groundRadius);
    }
}

public interface ILivingEntity
{
    public void Rotate(float horizontal, float vertical);
    public void Move();
    public void Jump();
}

public interface ITEST
{
    public void Test();
}
