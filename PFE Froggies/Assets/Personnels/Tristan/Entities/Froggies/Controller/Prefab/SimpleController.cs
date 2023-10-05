using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class SimpleController : MonoBehaviour
{
    // GRAVITY SECTION
    // 
    [Header("Gravity")]
    [Tooltip("The gravity applied when on ground or during a jump/rising the air.")]
    [SerializeField] float _normalGravity = 9.81f; // Stored value 
    [Tooltip("The gravity applied when falling or dropping.")]
    [SerializeField] float _modifiedGravity = 12f; // Stored value 
    // The gravity used when adding force
    float _actualGravity;

    // GROUND CHECK SECTION
    //
    [Header("On ground check")]
    [Tooltip("Transform object from whom a sphere is cast for detection of the ground.")]
    [SerializeField] Transform _groundCheck;
    [Tooltip("The radius of the casted sphere.")]
    [SerializeField] float _groundCheckRadius = 0.25f;
    [Tooltip("The layer mask of detection.")]
    [SerializeField] LayerMask _groundCheckLayerMask;
    // Boolean representing if the gameObject is on the ground 
    bool _grounded = false;

    [Space]
    [Tooltip("Will add a component Sphere Collider on trigger to the GroundCheck object, it use more memory. If not it will cast OverlapSphere from the object, it's less efficient.")]
    [SerializeField] bool _useTriggerCollider = true;
    // The component holding the Sphere Collider for detection
    TriggerCollider _groundCheckTrigger;

    // JUMP SECTION
    //
    [Header("Jump parameters")]
    [Header("Charge jump")]
    [SerializeField] float _chargeJumpIncrease = 0.1f;
    [SerializeField] Vector2 _chargeJumpClamp = new Vector2(1, 1.5f);
    float _chargeJumpValue = 1f;
    bool _isChargingJump = false;

    [Header("Axis Y - Up")]
    [Tooltip("The force applied for jumping on the Y axis, Up.")]
    [SerializeField] float _jumpForceUp = 5f;

    [Space]
    [Header("Axis Z - Forward")]
    [Tooltip("The force applied for jumping on the Z axis, Forward (only used if jumping up.")]
    [SerializeField] float _jumpForceFwd = 5f;
    
    [Space]
    [Tooltip("The ForceMode used for adding force on the Rigidbody.")]
    [SerializeField] ForceMode _jumpForceMode = ForceMode.Impulse;

    [Tooltip("Normalize the forces direction when jumping.")]
    [SerializeField] bool _areForceNormalized = false;
    [Tooltip("Force used when forces are normalized.")]
    [SerializeField] float _jumpForceNormalized = 5f;

    // If double jump or more are possibles
    int _nbrJumpMAX = 1;
    // Number of jumps used
    int _nbrJump = 0;
    // Store bool true if the player jumped
    bool _hasJumped = false;
    
    // Stored value of Input Space (UP) and grounded
    bool _tryToJumpUp = false;
    // Stored value of Input Q (Forward) and tryo to jump up
    bool _tryToJumpFwd = false;

    // DEBUG SECTION
    //
    [Header("Debug UI")]
    [Tooltip("Activate an UI for debugging.")]
    [SerializeField] bool _useDebugUI = false;
    [Tooltip("Canvas UI holding the debugging UI.")]
    [SerializeField] Canvas _debugCanvas;
    [Tooltip("UI object showing the gravity applied.")]
    [SerializeField] Text _gravityUsed;
    [Tooltip("UI object showing the velocity of the object on the Y axis.")]
    [SerializeField] Text _velocityY;
    [Tooltip("UI object showing if on ground.")]
    [SerializeField] Text _isOnGround;
    [Tooltip("UI object showing number of jumps.")]
    [SerializeField] Text _numberOfJump;
    [Tooltip("UI object showing if can jump.")]
    [SerializeField] Text _canJump;

    Rigidbody _rb;




    //



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _nbrJump = 0;
        
        // Set the gravity to normal
        ModifyGravity(_normalGravity);

        // Init debug UI, deactivate if not used
        ShowDebugUI();

        // Init and create the component for detection, if not use overlapshere
        if (_useTriggerCollider)
            InitTriggerCollider();
    }

    // Update is called once per frame
    void Update()
    {
        // Look if it is _grounded
        _grounded = GroundCheck();

        // Press Space to jump and isnt still jumping
        _tryToJumpUp = Input.GetKey(KeyCode.Space) /*&& !_hasJumped*/;
        

        // Press forward and is trying to jump
        _tryToJumpFwd = Input.GetKey(KeyCode.W) && _tryToJumpUp;

        // Show the debugUI if used
        ShowDebugUI();
        Debug.Log(_tryToJumpUp);
    }

    private void FixedUpdate()
    {
        /*
        // On ground 
        if (_grounded)
        {
            ModifyGravity(_normalGravity); // Normal gravity on ground

            // Player used Space key and hasnt jumped
            if (_tryToJumpUp)
            {
                
                // Can still jump
                if (_nbrJump < _nbrJumpMAX)
                    Jump();
                    
            }
            // Player is on ground not trying to jump and was falling (or still due to small errors)
            else if(_rb.velocity.y <= 0)
            {
                _hasJumped = false;
                _nbrJump = 0;
            }
            
        }
        // Not on ground
        else 
        {
            // Gravity for falling
            if (_rb.velocity.y < -0.1f)
                ModifyGravity(_modifiedGravity);
            else
                ModifyGravity(_normalGravity); // Normal gravity on ground
        }

        // Apply the choosed gravity
        ApplyGravity();
        */

        // Try jumping
        if(_tryToJumpUp && _grounded && !_hasJumped && _nbrJump < _nbrJumpMAX)
        {
            ModifyGravity(_normalGravity); // Normal gravity
            Jump();
        }
        // On ground
        else if (_grounded)
        {
            ModifyGravity(_normalGravity);
            _hasJumped = false;
            _nbrJump = 0;
        }
        // In air
        else
        {
            if (_rb.velocity.y < -0.1f)
                ModifyGravity(_modifiedGravity); // Gravity for falling
            else
                ModifyGravity(_normalGravity); // Normal gravity on ground
        }
        ApplyGravity();

    }

    // -------------- GRAVITY --------------
    // Modify the gravity used 
    void ModifyGravity(float gravity)
    {
        _actualGravity = gravity;
    }
    // Apply the gravity for on the Rigidbody based on mass
    void ApplyGravity()
    {
        _rb.AddForce(-Vector3.up * _rb.mass * _actualGravity);
    }

    // -------------- GROUND CHECK --------------
    // Return the result of TriggerCollider or overlapSphere, both result the same and return if its on ground or not 
    bool GroundCheck()
    {
        if(!_useTriggerCollider)
            return (Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundCheckLayerMask).Length > 0);
        return _groundCheckTrigger.HasObjectsDetected;
    }
    // Function to use on OnDrawGizmos for debugging area for detection Ground
    void GizmosGroundCheck()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius);
    }

    // -------------- TRIGGER COLLIDER --------------
    // Initialise the debugging UI 
    void InitTriggerCollider()
    {
        //Debug.Log("Init");
        _groundCheckTrigger = _groundCheck.TryGetComponent<TriggerCollider>(out TriggerCollider col) ? col : _groundCheck.AddComponent<TriggerCollider>();
        _groundCheckTrigger.InitTriggerCollider(_groundCheckRadius, _groundCheckLayerMask);
    }

    // -------------- JUMP --------------
    // Jump function, use the Rigidbody to propulse the player
    void ChargeJump()
    {
        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _isChargingJump = true;
            IncreaseChargeJumpValue();
        }

        if(_isChargingJump && Input.GetKeyUp(KeyCode.Space))
        {
            _isChargingJump = false;
            // JUMP
        }
    }
    void Jump()
    {
        Vector3 force = Vector3.zero;

        // Use a jump
        _nbrJump++;
        _hasJumped = true;
        // Add Up forces
        force += Vector3.up * _jumpForceUp;

        // Look for forward jump
        if (_tryToJumpFwd)
        {
            // Add Forward forces
            force += Vector3.forward * _jumpForceFwd;
        }

        // Look if normalized is used
        force = _areForceNormalized ? force.normalized * _jumpForceNormalized : force;
        force *= _chargeJumpValue;
        // Forces on Rigidbody
        _rb.AddRelativeForce(force, _jumpForceMode);
        _chargeJumpValue = 1;
    }

    void IncreaseChargeJumpValue()
    {
        _chargeJumpValue += _chargeJumpIncrease;
        _chargeJumpValue = Mathf.Clamp(_chargeJumpValue, _chargeJumpClamp.x, _chargeJumpClamp.y);
    }

    // -------------- UI DEBUG --------------
    // Initialise the debugging UI and display
    void InitDebugUI()
    {
        if(_debugCanvas != null)
            _debugCanvas.gameObject.SetActive(true);
        else
            Debug.LogWarning("Missing debug Canvas on " + this.name);

        if (_gravityUsed != null)
            _gravityUsed.text = _actualGravity.ToString();
        else
            Debug.LogWarning("Missing debug UI on " + this.name);

        if (_velocityY != null)
            _velocityY.text = _rb.velocity.y.ToString();
        else
            Debug.LogWarning("Missing debug UI on " + this.name);

        if (_isOnGround != null)
            _isOnGround.text = _grounded.ToString();
        else
            Debug.LogWarning("Missing debug UI on " + this.name);

        if (_numberOfJump != null)
            _numberOfJump.text = _nbrJump.ToString();
        else
            Debug.LogWarning("Missing debug UI on " + this.name);



        _canJump.text = (_grounded && !_hasJumped && _nbrJump < _nbrJumpMAX).ToString();
    }
    // Show the debugging UI
    void ShowDebugUI()
    {
        if (_useDebugUI)
            InitDebugUI();
        else if (_debugCanvas != null)
            _debugCanvas.gameObject.SetActive(false);
    }
    // Draw gizmos sphere, etc for the TriggerCollider or else
    private void OnDrawGizmos()
    {
        if (_useDebugUI)
            GizmosGroundCheck();
    }
}
// Jump to height --> velocity = RacineCarree(-2 * gravity * height)
// float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));