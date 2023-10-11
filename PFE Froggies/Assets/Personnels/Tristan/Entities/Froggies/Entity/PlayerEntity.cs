using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEntity : LivingEntity
{
    [Header("--- TONGUE ---")]
    [SerializeField] protected Transform _tongueStartTransform;
    [SerializeField] protected float _lengthTongue;


    bool _moveInput = false;
    bool _jumpInput = false;
    float _horizontalInput = 0;
    float _verticalInput = 0;
    Vector2 _rotaInput = Vector2.zero;
    public Vector2 RotaInput { set { _rotaInput = value; } }
    public bool JumpInput {  set { _jumpInput = value; } }
    public bool MoveInput { set { _moveInput = value; } }


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
        if(_jumpInput)
        {
            _rigidbodyController.AddForce(this.transform.up, _jumpForceUp, _jumpMode);
            _rigidbodyController.AddForce(this.transform.forward, _jumpForceFwd, _jumpMode);
            _jumpInput = false;
        }
        //base.Jump();
    }

    public void Rotate()
    {
        Rotate(_rotaInput.x, _rotaInput.y);
    }

    protected virtual void TongueHit()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_tongueStartTransform.position, (this.transform.forward * _lengthTongue) + _tongueStartTransform.position);
    }
}

public interface IFrogEntity
{
    public void TongueHit();
    public void Jump();
    public void Rotate();
}
