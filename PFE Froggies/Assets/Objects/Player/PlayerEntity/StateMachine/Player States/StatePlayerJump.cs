using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerJump : State
{
    protected StateMachinePlayer _smPlayer;

    // Constructor
    public StatePlayerJump(StateMachinePlayer sm) : base(sm) { _smPlayer = sm; }

    public override void Start()
    {
        _smPlayer.entity.Jump();
    }
    public override void Update(float time)
    {
        if (_smPlayer.entity.IsGrounded)
        {
            _smPlayer.Exit(_smPlayer.idle);
        }
    }
    public override void FixedUpdate(float time)
    {

    }
    public override void Exit()
    {

    }
}
