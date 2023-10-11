using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerIdle : State
{
    protected StateMachinePlayer _smPlayer;

    bool _isGrounded = false;

    // Constructor
    public StatePlayerIdle(StateMachinePlayer sm) : base(sm) { _smPlayer = sm; }

    public override void Start()
    {
        
    }
    public override void Update(float time)
    {
        _isGrounded = _smPlayer.entity.IsGrounded;

        if(_isGrounded)
            _smPlayer.entity.Jump();
    }
    public override void FixedUpdate(float time)
    {
        if (_isGrounded)
        {
            _smPlayer.entity.Rotate();
        }
    }
    public override void Exit()
    {

    }
}
