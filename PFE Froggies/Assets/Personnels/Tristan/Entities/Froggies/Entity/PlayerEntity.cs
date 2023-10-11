using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEntity : LivingEntity
{
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


        // _jumpInput = Input.GetKey(KeyCode.Space);
        // _rotaInput.x = Input.GetAxisRaw("Horizontal");
        // _rotaInput.y = Input.GetAxisRaw("Vertical");
        // _moveInput = Input.GetKey(KeyCode.Q);
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _smPlayer.FixedUpdate(Time.fixedDeltaTime);

        //if (_jumpInput)
        //    Jump();

        if (_moveInput)
            Move();

        Rotate(_rotaInput.x, _rotaInput.y);
        
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
}

public interface IFrogEntity
{
    public void TongueHit();
}
