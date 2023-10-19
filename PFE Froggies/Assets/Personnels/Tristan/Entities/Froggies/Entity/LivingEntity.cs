using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody))]
public class LivingEntity : MonoBehaviour, ILivingEntity
{
    [Header("--- SIMPLE RIGIDBODY ---")]
    [SerializeField] protected SimpleRigidbody _rigidbodyController;

    [Header("--- GROUND CHECK ---")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundRadius;
    [SerializeField] protected LayerMask _groundMask;

    public bool IsGrounded { get { return Physics.OverlapSphere(_groundCheck.position, _groundRadius, _groundMask).Length > 0; } }




    [Header("--- MOVEMENT ---")]
    [SerializeField] protected float _moveForce = 1;
    [SerializeField] protected ForceMode _moveMode = ForceMode.Impulse;

    [Header("--- JUMP ---")]
    [SerializeField] protected float _jumpForceUp = 1;
    [SerializeField] protected float _jumpForceFwd = 1;
    [Space]
    [SerializeField] protected float _longJumpForceUp = 1;
    [SerializeField] protected float _longJumpForceFwd = 2;

    protected ForceMode _jumpMode = ForceMode.Impulse;
    protected int _nbrJumpMAX = 1;
    protected int _nbrJump = 0;



    [Header("--- ROTATION ---")]
    [SerializeField] protected Camera _camera;
    [SerializeField] float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    

    // Initialized
    protected bool _initialized = false;

    // ================= UNITY METHODS =================
    //
    protected virtual void Awake()
    {
        InitComponents();
        _camera = Camera.main;
    }

    protected virtual void Start()
    {
        InitComponents();
        if(_camera == null) 
            _camera = Camera.main;
    }

    protected virtual void Update()
    {
        _nbrJump = IsGrounded ? 0 : _nbrJump;
    }

    protected virtual void FixedUpdate()
    {

    }
    // =================================================

    // ================= INITITIALISATION METHODS =================
    //
    protected void InitComponents()
    {
        if (!_initialized) // If it hasnt been initialized
        {
            InitSimpleRigidbody(); // Get the rigidbody 
            InitGroundController(); // Create a grounded controller
            _initialized = true;
        }
    }
    protected void InitSimpleRigidbody()
    {
        // Get the rigidbody or create it if there is none
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
        //_groundController = new GroundedController(_groundCheck, _groundRadius, _groundMask);
    }
    // =================================================

    // ================= MOVEMENT METHODS =================
    //
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
        if (IsGrounded && Mathf.Abs(_rigidbodyController.Velocity.y) < 0.2f)
        {           
            _rigidbodyController.AddForce(this.transform.up, _jumpForceUp, _jumpMode);
            _rigidbodyController.AddForce(this.transform.forward, _jumpForceFwd, _jumpMode);
        }
    }
    // =================================================

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
