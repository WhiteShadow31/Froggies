using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerIdle : StatePlayer
{
    // Constructor
    public StatePlayerIdle(StateMachinePlayer sm) : base(sm) { }

    public override void Start()
    {
        //Debug.Log(_smPlayer.entity.gameObject.name + " : " + "IDLE");
    }
    public override void Update(float time)
    {
        if (_smPlayer.entity.SmallJumpInput || _smPlayer.entity.LongJumpInput)
        {
            if (_smPlayer.entity.IsGrounded && _smPlayer.entity.CanJump)
                _smPlayer.Exit(_smPlayer.jump);
            else
            {
                _smPlayer.entity.SmallJumpInput = false;
                _smPlayer.entity.LongJumpInput = false;
            }
        }

        if (_smPlayer.entity.MountInput)
        {
            if (_smPlayer.entity.TryMount())
                _smPlayer.Exit(_smPlayer.onFrog);          
        }
    }
    public override void FixedUpdate(float time)
    {

    }
    public override void Exit()
    {

    }
}
