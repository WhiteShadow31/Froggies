using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerIdle : StatePlayer
{

    bool _isGrounded = false;

    // Constructor
    public StatePlayerIdle(StateMachinePlayer sm) : base(sm) { }

    public override void Start()
    {
        
    }
    public override void Update(float time)
    {
        _isGrounded = _smPlayer.entity.IsGrounded;

        if (_isGrounded)
        {
            _smPlayer.entity.Jump();
        }
        /*else
        {
            _smPlayer.Exit(_smPlayer.jump);
        }*/

        if(_smPlayer.entity.MountInput)
        {
            if (_smPlayer.entity.TryMount())
            {
                _smPlayer.Exit(_smPlayer.onFrog);
            }
        }
    }
    public override void FixedUpdate(float time)
    {
        if (_isGrounded)
        {
            _smPlayer.entity.Rotate();
            //_smPlayer.Exit(_smPlayer.onFrog);
        }
    }
    public override void Exit()
    {

    }
}
